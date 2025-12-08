using ChurrascariaSystem.Domain.Entities;

namespace ChurrascariaSystem.Domain.Interfaces
{
    public interface ITipoProdutoRepository
    {
        Task<TipoProduto?> GetByIdAsync(int id);
        Task<IEnumerable<TipoProduto>> GetAllAsync();
        Task<IEnumerable<TipoProduto>> GetAllActiveAsync();
        Task AddAsync(TipoProduto tipoProduto);
        Task UpdateAsync(TipoProduto tipoProduto);
        Task DeleteAsync(int id);
    }
}
