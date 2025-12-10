using ChurrascariaSystem.Domain.Entities;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IEstoqueRepository
    {
        Task<Estoque?> GetByIdAsync(int id);
        Task<Estoque?> GetByProdutoIdAsync(int produtoId);
        Task<IEnumerable<Estoque>> GetAllAsync();
        Task<IEnumerable<Estoque>> GetEstoqueBaixoAsync();
        Task<IEnumerable<Estoque>> GetEstoqueEsgotadoAsync();
        Task AddAsync(Estoque estoque);
        Task UpdateAsync(Estoque estoque);
        Task DeleteAsync(int id);
    }

    public interface IMovimentacaoEstoqueRepository
    {
        Task<MovimentacaoEstoque?> GetByIdAsync(int id);
        Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync();
        Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoIdAsync(int produtoId);
        Task<IEnumerable<MovimentacaoEstoque>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task AddAsync(MovimentacaoEstoque movimentacao);
    }
}
