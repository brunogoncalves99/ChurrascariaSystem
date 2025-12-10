using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Enums;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Domain.ValueObjects;

namespace ChurrascariaSystem.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMesaRepository _mesaRepository;

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, IMesaRepository mesaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _mesaRepository = mesaRepository;
        }

        public async Task<PedidoDTO?> GetByIdAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            return pedido == null ? null : MapToDTO(pedido);
        }

        public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return pedidos.Select(MapToDTO);
        }

        public async Task<IEnumerable<PedidoDTO>> GetByMesaAsync(int mesaId)
        {
            var pedidos = await _pedidoRepository.GetByMesaAsync(mesaId);
            return pedidos.Select(MapToDTO);
        }

        public async Task<IEnumerable<PedidoDTO>> GetPedidosAbertosAsync()
        {
            var pedidos = await _pedidoRepository.GetPedidosAbertosAsync();
            return pedidos.Select(MapToDTO);
        }

        public async Task<PedidoDTO> CreateAsync(PedidoDTO pedidoDto)
        {
            var pedido = new Pedido
            {
                idMesa = pedidoDto.idMesa,
                idUsuario = pedidoDto.idUsuario,
                DataPedido = DateTime.Now,
                StatusPedidoValor = "Aberto",
                Observacao = pedidoDto.Observacao
            };

            foreach (var itemDto in pedidoDto.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(itemDto.idProduto);
                if (produto == null) continue;

                var item = new ItemPedido
                {
                    idProduto = itemDto.idProduto,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoValor,
                    Observacao = itemDto.Observacao
                };
                item.CalcularSubtotal();
                pedido.Itens.Add(item);
            }

            // Atualizar status da mesa para Ocupado para não deixar reservar uma mesa que já está ocupada
            var mesa = await _mesaRepository.GetByIdAsync(pedidoDto.idMesa);
            if (mesa != null)
            {
                mesa.Status = StatusMesa.Ocupada;
                await _mesaRepository.UpdateAsync(mesa);
            }

            pedido.CalcularValorTotal();
            await _pedidoRepository.AddAsync(pedido);
            return MapToDTO(pedido);
        }

        public async Task UpdateAsync(PedidoDTO pedidoDto)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoDto.idPedido);
            if (pedido == null) throw new Exception("Pedido não encontrado");

            pedido.StatusPedidoValor = pedidoDto.Status;
            pedido.Observacao = pedidoDto.Observacao;

            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) throw new Exception("Pedido não encontrado");

            pedido.Status = StatusPedido.Criar(status);
            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task DeleteAsync(int id)
        {
            await _pedidoRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetValorTotalMesaAsync(int mesaId)
        {
            var pedidos = await _pedidoRepository.GetByMesaAsync(mesaId);
            var pedidosAbertos = pedidos.Where(p => p.StatusPedidoValor == "Aberto" || p.StatusPedidoValor == "Em Preparação" || p.StatusPedidoValor == "Pronto");
            return pedidosAbertos.Sum(p => p.ValorTotal);
        }

        public async Task<IEnumerable<PedidoDTO>> GetHistoricoAsync(HistoricoPedidoFiltroDTO filtros)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var query = pedidos.AsQueryable();

            if (filtros.DataInicio.HasValue)
            {
                query = query.Where(p => p.DataPedido.Date >= filtros.DataInicio.Value.Date);
            }

            if (filtros.DataFim.HasValue)
            {
                query = query.Where(p => p.DataPedido.Date <= filtros.DataFim.Value.Date);
            }

            if (!string.IsNullOrEmpty(filtros.Status))
            {
                query = query.Where(p => p.StatusPedidoValor == filtros.Status);
            }

            if (filtros.idMesa.HasValue)
            {
                query = query.Where(p => p.idMesa == filtros.idMesa.Value);
            }

            if (filtros.idUsuario.HasValue)
            {
                query = query.Where(p => p.idUsuario == filtros.idUsuario.Value);
            }

            if (!string.IsNullOrEmpty(filtros.FormaPagamento))
            {
                query = query.Where(p => p.FormaPagamento == filtros.FormaPagamento);
            }

            if (filtros.ApenasPagos.HasValue && filtros.ApenasPagos.Value)
            {
                query = query.Where(p => p.Pago);
            }

            if (filtros.ApenasNaoPagos.HasValue && filtros.ApenasNaoPagos.Value)
            {
                query = query.Where(p => !p.Pago);
            }

            return query.OrderByDescending(p => p.DataPedido).Select(MapToDTO);
        }

        private PedidoDTO MapToDTO(Pedido pedido)
        {
            return new PedidoDTO
            {
                idPedido = pedido.idPedido,
                idMesa = pedido.idMesa,
                MesaNumero = pedido.Mesa?.Numero,
                idUsuario = pedido.idUsuario,
                NomeUsuario = pedido.Usuario?.Nome,
                DataPedido = pedido.DataPedido,
                Status = pedido.StatusPedidoValor,
                ValorTotal = pedido.ValorTotal,
                Observacao = pedido.Observacao,
                Itens = pedido.Itens.Select(i => new ItemPedidoDTO
                {
                    idItemPedido = i.idItemPedido,
                    idPedido = i.idPedido,
                    idProduto = i.idProduto,
                    ProdutoNome = i.Produto?.Nome,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Subtotal,
                    Observacao = i.Observacao
                }).ToList()
            };
        }
    }
}
