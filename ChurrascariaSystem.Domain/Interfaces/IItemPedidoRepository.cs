using ChurrascariaSystem.Domain.Entities;

namespace ChurrascariaSystem.Domain.Interfaces
{
    public interface IItemPedidoRepository
    {
        Task<ItemPedido?> GetByIdAsync(int id);
        Task<IEnumerable<ItemPedido>> GetAllAsync();
        Task<IEnumerable<ItemPedido>> GetByPedidoIdAsync(int pedidoId);
        Task<IEnumerable<ItemPedido>> GetByProdutoIdAsync(int produtoId);
        Task AddAsync(ItemPedido itemPedido);
        Task UpdateAsync(ItemPedido itemPedido);
        Task DeleteAsync(int id);
    }
}
