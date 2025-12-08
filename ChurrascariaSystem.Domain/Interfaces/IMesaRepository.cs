using ChurrascariaSystem.Domain.Entities;

namespace ChurrascariaSystem.Domain.Interfaces
{
    public interface IMesaRepository
    {
        Task<Mesa?> GetByIdAsync(int id);
        Task<IEnumerable<Mesa>> GetAllAsync();
        Task<IEnumerable<Mesa>> GetAllActiveAsync();
        Task<IEnumerable<Mesa>> GetMesasLivresAsync();
        Task AddAsync(Mesa mesa);
        Task UpdateAsync(Mesa mesa);
        Task DeleteAsync(int id);
    }
}
