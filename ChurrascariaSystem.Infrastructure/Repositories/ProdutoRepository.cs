using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProdutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            return await _context.Produtos
                .Include(p => p.TipoProduto)
                .FirstOrDefaultAsync(p => p.idProduto == id);
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos
                .Include(p => p.TipoProduto)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetAllActiveAsync()
        {
            return await _context.Produtos
                .Include(p => p.TipoProduto)
                .Where(p => p.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetByTipoAsync(int tipoProdutoId)
        {
            return await _context.Produtos
                .Include(p => p.TipoProduto)
                .Where(p => p.idTipoProduto == tipoProdutoId && p.Ativo)
                .ToListAsync();
        }

        public async Task AddAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await GetByIdAsync(id);
            if (produto != null)
            {
                produto.Ativo = false;
                await UpdateAsync(produto);
            }
        }
    }
}
