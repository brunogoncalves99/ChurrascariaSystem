using ChurrascariaSystem.Domain.Entities;

namespace ChurrascariaSystem.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int id);
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<IEnumerable<Pedido>> GetByMesaAsync(int mesaId);
        Task<IEnumerable<Pedido>> GetByDataAsync(DateTime data);
        Task<IEnumerable<Pedido>> GetPedidosAbertosAsync();
        Task<IEnumerable<Pedido>> GetPedidosByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task DeleteAsync(int id);
    }
}
