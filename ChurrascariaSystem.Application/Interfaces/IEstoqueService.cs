using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IEstoqueService
    {
        /// <summary>
        /// Obtém estoque por ID
        /// </summary>
        Task<EstoqueDTO?> GetByIdAsync(int id);

        /// <summary>
        /// Obtém todos os estoques
        /// </summary>
        Task<IEnumerable<EstoqueDTO>> GetAllAsync();

        /// <summary>
        /// Obtém estoque de um produto específico
        /// </summary>
        Task<EstoqueDTO?> GetByProdutoIdAsync(int produtoId);

        /// <summary>
        /// Obtém produtos com estoque baixo (quantidade <= mínima)
        /// </summary>
        Task<IEnumerable<EstoqueDTO>> GetEstoqueBaixoAsync();

        /// <summary>
        /// Obtém produtos com estoque esgotado (quantidade = 0)
        /// </summary>
        Task<IEnumerable<EstoqueDTO>> GetEstoqueEsgotadoAsync();

        /// <summary>
        /// Registra entrada de estoque (compra, devolução, etc)
        /// </summary>
        Task<EstoqueDTO> DarEntradaAsync(EntradaEstoqueDTO entrada, int usuarioId);

        /// <summary>
        /// Registra saída de estoque (venda automática quando pedido é criado)
        /// </summary>
        Task DarBaixaAsync(int produtoId, int quantidade, int? pedidoId = null, int? usuarioId = null);

        /// <summary>
        /// Ajusta estoque manualmente (inventário, correção, perda)
        /// </summary>
        Task<EstoqueDTO> AjustarEstoqueAsync(int produtoId, int novaQuantidade, string motivo, int usuarioId);

        /// <summary>
        /// Cria novo registro de estoque para um produto
        /// </summary>
        Task<EstoqueDTO> CreateAsync(EstoqueDTO estoqueDto);

        /// <summary>
        /// Atualiza configurações de estoque (quantidade mínima)
        /// </summary>
        Task<EstoqueDTO> UpdateAsync(EstoqueDTO estoqueDto);

        /// <summary>
        /// Remove registro de estoque
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Obtém histórico de movimentações de um produto
        /// </summary>
        Task<IEnumerable<MovimentacaoEstoqueDTO>> GetMovimentacoesByProdutoAsync(int produtoId);

        /// <summary>
        /// Obtém movimentações de um período
        /// </summary>
        Task<IEnumerable<MovimentacaoEstoqueDTO>> GetMovimentacoesByPeriodoAsync(DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Verifica se há estoque suficiente para um produto
        /// </summary>
        Task<bool> VerificarEstoqueDisponivel(int produtoId, int quantidadeNecessaria);
    }
}
