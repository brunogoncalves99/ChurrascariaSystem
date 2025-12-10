using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public EstoqueService(
            IEstoqueRepository estoqueRepository,
            IMovimentacaoEstoqueRepository movimentacaoRepository,
            IProdutoRepository produtoRepository)
        {
            _estoqueRepository = estoqueRepository;
            _movimentacaoRepository = movimentacaoRepository;
            _produtoRepository = produtoRepository;
        }

        /// <summary>
        /// Obtém estoque por ID
        /// </summary>
        public async Task<EstoqueDTO?> GetByIdAsync(int id)
        {
            var estoque = await _estoqueRepository.GetByIdAsync(id);
            return estoque != null ? MapToDTO(estoque) : null;
        }

        /// <summary>
        /// Obtém todos os estoques
        /// </summary>
        public async Task<IEnumerable<EstoqueDTO>> GetAllAsync()
        {
            var estoques = await _estoqueRepository.GetAllAsync();
            return estoques.Select(MapToDTO);
        }

        /// <summary>
        /// Obtém estoque de um produto específico
        /// </summary>
        public async Task<EstoqueDTO?> GetByProdutoIdAsync(int produtoId)
        {
            var estoque = await _estoqueRepository.GetByProdutoIdAsync(produtoId);
            return estoque != null ? MapToDTO(estoque) : null;
        }

        /// <summary>
        /// Obtém produtos com estoque baixo
        /// </summary>
        public async Task<IEnumerable<EstoqueDTO>> GetEstoqueBaixoAsync()
        {
            var estoques = await _estoqueRepository.GetEstoqueBaixoAsync();
            return estoques.Select(MapToDTO);
        }

        /// <summary>
        /// Obtém produtos com estoque esgotado
        /// </summary>
        public async Task<IEnumerable<EstoqueDTO>> GetEstoqueEsgotadoAsync()
        {
            var estoques = await _estoqueRepository.GetEstoqueEsgotadoAsync();
            return estoques.Select(MapToDTO);
        }

        /// <summary>
        /// Registra entrada de estoque (compra, devolução)
        /// </summary>
        public async Task<EstoqueDTO> DarEntradaAsync(EntradaEstoqueDTO entrada, int usuarioId)
        {
            if (entrada.Quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            // Buscar ou criar estoque do produto
            var estoque = await _estoqueRepository.GetByProdutoIdAsync(entrada.idProduto);

            if (estoque == null)
            {
                // Criar novo registro de estoque
                var produto = await _produtoRepository.GetByIdAsync(entrada.idProduto);
                if (produto == null)
                    throw new Exception("Produto não encontrado");

                estoque = new Estoque
                {
                    idProduto = entrada.idProduto,
                    QuantidadeAtual = 0,
                    QuantidadeMinima = entrada.Quantidade > 0 ? 10 : entrada.Quantidade, 
                    UltimaAtualizacao = DateTime.Now
                };

                await _estoqueRepository.AddAsync(estoque);
                estoque = await _estoqueRepository.GetByProdutoIdAsync(entrada.idProduto);
            }

            var quantidadeAnterior = estoque.QuantidadeAtual;

            // Atualizar quantidade
            estoque.QuantidadeAtual += entrada.Quantidade;
            estoque.UltimaAtualizacao = DateTime.Now;

            await _estoqueRepository.UpdateAsync(estoque);

            // Registrar movimentação
            var movimentacao = new MovimentacaoEstoque
            {
                idProduto = entrada.idProduto,
                TipoMovimentacao = "Entrada",
                Quantidade = entrada.Quantidade,
                QuantidadeAnterior = quantidadeAnterior,
                QuantidadeNova = estoque.QuantidadeAtual,
                Motivo = entrada.Motivo ?? "Compra",
                idUsuario = usuarioId,
                DataMovimentacao = DateTime.Now,
                Observacao = entrada.Observacao
            };

            await _movimentacaoRepository.AddAsync(movimentacao);

            return MapToDTO(estoque);
        }

        /// <summary>
        /// Registra saída de estoque (venda automática)
        /// </summary>
        public async Task DarBaixaAsync(int produtoId, int quantidade, int? pedidoId = null, int? usuarioId = null)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            var estoque = await _estoqueRepository.GetByProdutoIdAsync(produtoId);

            if (estoque == null)
                throw new Exception("Estoque não encontrado para este produto");

            if (estoque.QuantidadeAtual < quantidade)
                throw new Exception($"Estoque insuficiente. Disponível: {estoque.QuantidadeAtual}, Necessário: {quantidade}");

            var quantidadeAnterior = estoque.QuantidadeAtual;

            // Atualizar quantidade
            estoque.QuantidadeAtual -= quantidade;
            estoque.UltimaAtualizacao = DateTime.Now;

            await _estoqueRepository.UpdateAsync(estoque);

            var movimentacao = new MovimentacaoEstoque
            {
                idProduto = produtoId,
                TipoMovimentacao = "Saida",
                Quantidade = quantidade,
                QuantidadeAnterior = quantidadeAnterior,
                QuantidadeNova = estoque.QuantidadeAtual,
                Motivo = "Venda",
                idPedido = pedidoId,
                idUsuario = usuarioId,
                DataMovimentacao = DateTime.Now,
                Observacao = pedidoId.HasValue ? $"Venda - Pedido #{pedidoId}" : "Venda direta"
            };

            await _movimentacaoRepository.AddAsync(movimentacao);
        }

        /// <summary>
        /// Ajusta estoque manualmente (inventário, perda, correção)
        /// </summary>
        public async Task<EstoqueDTO> AjustarEstoqueAsync(int produtoId, int novaQuantidade, string motivo, int usuarioId)
        {
            if (novaQuantidade < 0)
                throw new ArgumentException("Quantidade não pode ser negativa");

            var estoque = await _estoqueRepository.GetByProdutoIdAsync(produtoId);

            if (estoque == null)
                throw new Exception("Estoque não encontrado para este produto");

            var quantidadeAnterior = estoque.QuantidadeAtual;
            var diferenca = novaQuantidade - quantidadeAnterior;

            // Atualizar quantidade
            estoque.QuantidadeAtual = novaQuantidade;
            estoque.UltimaAtualizacao = DateTime.Now;

            await _estoqueRepository.UpdateAsync(estoque);

            // Registrar movimentação
            var movimentacao = new MovimentacaoEstoque
            {
                idProduto = produtoId,
                TipoMovimentacao = "Ajuste",
                Quantidade = Math.Abs(diferenca),
                QuantidadeAnterior = quantidadeAnterior,
                QuantidadeNova = novaQuantidade,
                Motivo = motivo,
                idUsuario = usuarioId,
                DataMovimentacao = DateTime.Now,
                Observacao = diferenca > 0
                    ? $"Ajuste positivo: +{diferenca}"
                    : $"Ajuste negativo: {diferenca}"
            };

            await _movimentacaoRepository.AddAsync(movimentacao);

            return MapToDTO(estoque);
        }

        /// <summary>
        /// Cria novo registro de estoque
        /// </summary>
        public async Task<EstoqueDTO> CreateAsync(EstoqueDTO estoqueDto)
        {
            // Verificar se já existe estoque para este produto
            var estoqueExistente = await _estoqueRepository.GetByProdutoIdAsync(estoqueDto.idProduto);
            if (estoqueExistente != null)
                throw new Exception("Já existe um registro de estoque para este produto");

            var estoque = new Estoque
            {
                idProduto = estoqueDto.idProduto,
                QuantidadeAtual = estoqueDto.QuantidadeAtual,
                QuantidadeMinima = estoqueDto.QuantidadeMinima,
                UltimaAtualizacao = DateTime.Now
            };

            await _estoqueRepository.AddAsync(estoque);

            // Se houver quantidade inicial, registrar como movimentação
            if (estoqueDto.QuantidadeAtual > 0)
            {
                var movimentacao = new MovimentacaoEstoque
                {
                    idProduto = estoqueDto.idProduto,
                    TipoMovimentacao = "Entrada",
                    Quantidade = estoqueDto.QuantidadeAtual,
                    QuantidadeAnterior = 0,
                    QuantidadeNova = estoqueDto.QuantidadeAtual,
                    Motivo = "Estoque Inicial",
                    DataMovimentacao = DateTime.Now,
                    Observacao = "Criação do registro de estoque"
                };

                await _movimentacaoRepository.AddAsync(movimentacao);
            }

            return MapToDTO(estoque);
        }

        /// <summary>
        /// Atualiza configurações de estoque
        /// </summary>
        public async Task<EstoqueDTO> UpdateAsync(EstoqueDTO estoqueDto)
        {
            var estoque = await _estoqueRepository.GetByIdAsync(estoqueDto.idEstoque);

            if (estoque == null)
                throw new Exception("Estoque não encontrado");

            // Atualizar apenas quantidade mínima (quantidade atual é atualizada via movimentações)
            estoque.QuantidadeMinima = estoqueDto.QuantidadeMinima;
            estoque.UltimaAtualizacao = DateTime.Now;

            await _estoqueRepository.UpdateAsync(estoque);

            return MapToDTO(estoque);
        }

        /// <summary>
        /// Remove registro de estoque
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var estoque = await _estoqueRepository.GetByIdAsync(id);

            if (estoque == null)
                throw new Exception("Estoque não encontrado");

            if (estoque.QuantidadeAtual > 0)
                throw new Exception("Não é possível excluir estoque com quantidade disponível. Ajuste para zero primeiro.");

            await _estoqueRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Obtém histórico de movimentações de um produto
        /// </summary>
        public async Task<IEnumerable<MovimentacaoEstoqueDTO>> GetMovimentacoesByProdutoAsync(int produtoId)
        {
            var movimentacoes = await _movimentacaoRepository.GetByProdutoIdAsync(produtoId);
            return movimentacoes.Select(MapMovimentacaoToDTO);
        }

        /// <summary>
        /// Obtém movimentações de um período
        /// </summary>
        public async Task<IEnumerable<MovimentacaoEstoqueDTO>> GetMovimentacoesByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            var movimentacoes = await _movimentacaoRepository.GetByPeriodoAsync(dataInicio, dataFim);
            return movimentacoes.Select(MapMovimentacaoToDTO);
        }

        /// <summary>
        /// Verifica se há estoque suficiente
        /// </summary>
        public async Task<bool> VerificarEstoqueDisponivel(int produtoId, int quantidadeNecessaria)
        {
            var estoque = await _estoqueRepository.GetByProdutoIdAsync(produtoId);

            if (estoque == null)
                return false;

            return estoque.QuantidadeAtual >= quantidadeNecessaria;
        }

        /// <summary>
        /// Mapeia Estoque para EstoqueDTO
        /// </summary>
        private EstoqueDTO MapToDTO(Estoque estoque)
        {
            return new EstoqueDTO
            {
                idEstoque = estoque.idEstoque,
                idProduto = estoque.idProduto,
                NomeProduto = estoque.Produto?.Nome ?? "",
                QuantidadeAtual = estoque.QuantidadeAtual,
                QuantidadeMinima = estoque.QuantidadeMinima,
                UltimaAtualizacao = estoque.UltimaAtualizacao,
                EstoqueBaixo = estoque.EstoqueBaixo,
                EstoqueEsgotado = estoque.EstoqueEsgotado
            };
        }

        /// <summary>
        /// Mapeia MovimentacaoEstoque para MovimentacaoEstoqueDTO
        /// </summary>
        private MovimentacaoEstoqueDTO MapMovimentacaoToDTO(MovimentacaoEstoque movimentacao)
        {
            return new MovimentacaoEstoqueDTO
            {
                idMovimentacao = movimentacao.idMovimentacao,
                idProduto = movimentacao.idProduto,
                NomeProduto = movimentacao.Produto?.Nome ?? "",
                TipoMovimentacao = movimentacao.TipoMovimentacao,
                Quantidade = movimentacao.Quantidade,
                QuantidadeAnterior = movimentacao.QuantidadeAnterior,
                QuantidadeNova = movimentacao.QuantidadeNova,
                Motivo = movimentacao.Motivo,
                idPedido = movimentacao.idPedido,
                idUsuario = movimentacao.idUsuario,
                UsuarioNome = movimentacao.Usuario?.Nome ?? "Sistema",
                DataMovimentacao = movimentacao.DataMovimentacao,
                Observacao = movimentacao.Observacao
            };
        }
    }
}
