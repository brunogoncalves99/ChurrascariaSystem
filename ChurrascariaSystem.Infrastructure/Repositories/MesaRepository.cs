using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Enums;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    public class MesaRepository : IMesaRepository
    {
        private readonly ApplicationDbContext _context;

        public MesaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Mesa?> GetByIdAsync(int id)
        {
            return await _context.Mesas.FindAsync(id);
        }

        public async Task<IEnumerable<Mesa>> GetAllAsync()
        {
            return await _context.Mesas.ToListAsync();
        }

        public async Task<IEnumerable<Mesa>> GetAllActiveAsync()
        {
            return await _context.Mesas.Where(m => m.Ativo).ToListAsync();
        }

        public async Task<IEnumerable<Mesa>> GetMesasLivresAsync()
        {
            return await _context.Mesas
                .Where(m => m.Ativo && m.Status == StatusMesa.Livre)
                .ToListAsync();
        }

        public async Task AddAsync(Mesa mesa)
        {
            await _context.Mesas.AddAsync(mesa);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mesa mesa)
        {
            _context.Mesas.Update(mesa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mesa = await GetByIdAsync(id);
            if (mesa != null)
            {
                mesa.Ativo = false;
                await UpdateAsync(mesa);
            }
        }
    }
}
