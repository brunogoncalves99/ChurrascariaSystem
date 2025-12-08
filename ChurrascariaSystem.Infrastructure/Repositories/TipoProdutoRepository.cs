using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    public class TipoProdutoRepository : ITipoProdutoRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoProdutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TipoProduto?> GetByIdAsync(int id)
        {
            return await _context.TiposProduto.FindAsync(id);
        }

        public async Task<IEnumerable<TipoProduto>> GetAllAsync()
        {
            return await _context.TiposProduto.ToListAsync();
        }

        public async Task<IEnumerable<TipoProduto>> GetAllActiveAsync()
        {
            return await _context.TiposProduto.Where(t => t.Ativo).ToListAsync();
        }

        public async Task AddAsync(TipoProduto tipoProduto)
        {
            await _context.TiposProduto.AddAsync(tipoProduto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TipoProduto tipoProduto)
        {
            _context.TiposProduto.Update(tipoProduto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tipoProduto = await GetByIdAsync(id);
            if (tipoProduto != null)
            {
                tipoProduto.Ativo = false;
                await UpdateAsync(tipoProduto);
            }
        }
    }
}
