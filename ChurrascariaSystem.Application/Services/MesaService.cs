using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Enums;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class MesaService : IMesaService
    {
        private readonly IMesaRepository _mesaRepository;
        private readonly IPedidoRepository _pedidoRepository;
        public MesaService(IMesaRepository mesaRepository, IPedidoRepository pedidoRepository)
        {
            _mesaRepository = mesaRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<MesaDTO?> GetByIdAsync(int id)
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);
            return mesa == null ? null : MapToDTO(mesa);
        }

        public async Task<IEnumerable<MesaDTO>> GetAllAsync()
        {
            var mesas = await _mesaRepository.GetAllAsync();
            return mesas.Select(MapToDTO);
        }

        public async Task<IEnumerable<MesaDTO>> GetAllActiveAsync()
        {
            var mesas = await _mesaRepository.GetAllActiveAsync();
            return mesas.Select(MapToDTO);
        }

        public async Task<IEnumerable<MesaDTO>> GetMesasLivresAsync()
        {
            var mesas = await _mesaRepository.GetMesasLivresAsync();
            return mesas.Select(MapToDTO);
        }

        public async Task<MesaDTO> CreateAsync(MesaDTO mesaDto)
        {
            var mesa = new Mesa
            {
                Numero = mesaDto.Numero,
                Capacidade = mesaDto.Capacidade,
                Status = mesaDto.Status,
                Ativo = mesaDto.Ativo
            };

            await _mesaRepository.AddAsync(mesa);
            return MapToDTO(mesa);
        }

        public async Task UpdateAsync(MesaDTO mesaDto)
        {
            var mesa = await _mesaRepository.GetByIdAsync(mesaDto.idMesa);

            if (mesa == null) 
                throw new Exception("Mesa não encontrada");

            mesa.Numero = mesaDto.Numero;
            mesa.Capacidade = mesaDto.Capacidade;
            mesa.Status = mesaDto.Status;
            mesa.Ativo = mesaDto.Ativo;

            await _mesaRepository.UpdateAsync(mesa);
        }

        public async Task UpdateStatusAsync(int id, int status)
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);

            if (mesa == null) 
                throw new Exception("Mesa não encontrada");

            mesa.Status = (StatusMesa)status;
            await _mesaRepository.UpdateAsync(mesa);
        }

        public async Task DeleteAsync(int id)
        {
            await _mesaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ContaMesaDTO>> GetContasAbertasAsync()
        {
            var mesas = await _mesaRepository.GetAllActiveAsync();
            var contas = new List<ContaMesaDTO>();

            foreach (var mesa in mesas)
            {
                var pedidos = await _pedidoRepository.GetPedidosByMesaIdAsync(mesa.idMesa);
                var pedidosNaoPagos = pedidos.Where(p => !p.Pago).ToList();

                if (pedidosNaoPagos.Any())
                {
                    var conta = new ContaMesaDTO
                    {
                        idMesa = mesa.idMesa,
                        MesaNumero = mesa.Numero,
                        QuantidadePedidos = pedidosNaoPagos.Count,
                        ValorTotal = pedidosNaoPagos.Sum(p => p.ValorTotal),
                        Pedidos = pedidosNaoPagos.Select(MapPedidoToDTO).ToList(),
                        PrimeiroPedido = pedidosNaoPagos.Min(p => p.DataPedido),
                        UltimoPedido = pedidosNaoPagos.Max(p => p.DataPedido),
                        PossuiPedidosAbertos = pedidosNaoPagos.Any(p => p.StatusPedidoValor != "Entregue"),
                        Status = pedidosNaoPagos.Any(p => p.StatusPedidoValor != "Entregue") ? "Ocupada" : "AguardandoPagamento"
                    };
                    contas.Add(conta);
                }
            }

            return contas.OrderBy(c => c.MesaNumero);
        }

        public async Task<ContaMesaDTO?> GetContaMesaAsync(int mesaId)
        {
            var mesa = await _mesaRepository.GetByIdAsync(mesaId);
            if (mesa == null) 
                return null;

            var pedidos = await _pedidoRepository.GetPedidosByMesaIdAsync(mesaId);
            var pedidosNaoPagos = pedidos.Where(p => !p.Pago).ToList();

            if (!pedidosNaoPagos.Any()) return null;

            return new ContaMesaDTO
            {
                idMesa = mesa.idMesa,
                MesaNumero = mesa.Numero,
                QuantidadePedidos = pedidosNaoPagos.Count,
                ValorTotal = pedidosNaoPagos.Sum(p => p.ValorTotal),
                Pedidos = pedidosNaoPagos.Select(MapPedidoToDTO).ToList(),
                PrimeiroPedido = pedidosNaoPagos.Min(p => p.DataPedido),
                UltimoPedido = pedidosNaoPagos.Max(p => p.DataPedido),
                PossuiPedidosAbertos = pedidosNaoPagos.Any(p => p.StatusPedidoValor != "Entregue"),
                Status = pedidosNaoPagos.Any(p => p.StatusPedidoValor != "Entregue") ? "Ocupada" : "AguardandoPagamento"
            };
        }

        public async Task FecharContaAsync(int mesaId, string formaPagamento)
        {
            var pedidos = await _pedidoRepository.GetPedidosByMesaIdAsync(mesaId);
            var pedidosNaoPagos = pedidos.Where(p => !p.Pago).ToList();

            foreach (var pedido in pedidosNaoPagos)
            {
                pedido.Pago = true;
                pedido.DataPagamento = DateTime.Now;
                pedido.FormaPagamento = formaPagamento;
                await _pedidoRepository.UpdateAsync(pedido);
            }

            // Atualizar status da mesa para Livre
            var mesa = await _mesaRepository.GetByIdAsync(mesaId);
            if (mesa != null)
            {
                mesa.Status = StatusMesa.Livre;
                await _mesaRepository.UpdateAsync(mesa);
            }
        }

        private MesaDTO MapToDTO(Mesa mesa)
        {
            return new MesaDTO
            {
                idMesa = mesa.idMesa,
                Numero = mesa.Numero,
                Capacidade = mesa.Capacidade,
                Status = mesa.Status,
                StatusDescricao = mesa.Status.ToString(),
                Ativo = mesa.Ativo
            };
        }

        private PedidoDTO MapPedidoToDTO(Pedido pedido)
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
                Pago = pedido.Pago,
                DataPagamento = pedido.DataPagamento,
                FormaPagamento = pedido.FormaPagamento,
                Itens = pedido.Itens.Select(i => new ItemPedidoDTO
                {
                    idItemPedido = i.idItemPedido,
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
