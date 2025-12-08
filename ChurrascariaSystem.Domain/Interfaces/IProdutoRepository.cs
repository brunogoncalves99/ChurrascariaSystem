using ChurrascariaSystem.Domain.Entities;

namespace ChurrascariaSystem.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> GetByIdAsync(int id);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<IEnumerable<Produto>> GetAllActiveAsync();
        Task<IEnumerable<Produto>> GetByTipoAsync(int tipoProdutoId);
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
    }
}
