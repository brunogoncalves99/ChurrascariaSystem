using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.idPedido == id);
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetByMesaAsync(int mesaId)
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(p => p.idMesa == mesaId)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetByDataAsync(DateTime data)
        {
            var dataInicio = data.Date;
            var dataFim = dataInicio.AddDays(1);

            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(p => p.DataPedido >= dataInicio && p.DataPedido < dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosAbertosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(p => p.StatusPedidoValor == "Aberto" || p.StatusPedidoValor == "Em Preparação" || p.StatusPedidoValor == "Pronto")
                .OrderBy(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Usuario)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(p => p.DataPedido >= dataInicio && p.DataPedido < dataFim)
                .ToListAsync();
        }

        public async Task AddAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pedido = await GetByIdAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
