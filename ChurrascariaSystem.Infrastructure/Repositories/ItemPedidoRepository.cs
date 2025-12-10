using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de itens de pedido
    /// </summary>
    public class ItemPedidoRepository : IItemPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemPedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemPedido?> GetByIdAsync(int id)
        {
            return await _context.ItensPedido
                .Include(i => i.Pedido)
                .Include(i => i.Produto)
                .FirstOrDefaultAsync(i => i.idItemPedido == id);
        }

        public async Task<IEnumerable<ItemPedido>> GetAllAsync()
        {
            return await _context.ItensPedido
                .Include(i => i.Pedido)
                .Include(i => i.Produto)
                .OrderByDescending(i => i.idPedido)
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemPedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.ItensPedido
                .Include(i => i.Produto)
                .Where(i => i.idPedido == pedidoId)
                .OrderBy(i => i.Produto.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemPedido>> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.ItensPedido
                .Include(i => i.Pedido)
                    .ThenInclude(p => p.Mesa)
                .Include(i => i.Produto)
                .Where(i => i.idProduto == produtoId)
                .OrderByDescending(i => i.Pedido.DataPedido)
                .ToListAsync();
        }

        public async Task AddAsync(ItemPedido itemPedido)
        {
            await _context.ItensPedido.AddAsync(itemPedido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemPedido itemPedido)
        {
            _context.ItensPedido.Update(itemPedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemPedido = await GetByIdAsync(id);
            if (itemPedido != null)
            {
                _context.ItensPedido.Remove(itemPedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
