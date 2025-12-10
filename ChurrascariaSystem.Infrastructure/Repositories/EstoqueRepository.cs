using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly ApplicationDbContext _context;

        public EstoqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Estoque?> GetByIdAsync(int id)
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.idEstoque == id);
        }

        public async Task<Estoque?> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.idProduto == produtoId);
        }

        public async Task<IEnumerable<Estoque>> GetAllAsync()
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .OrderBy(e => e.Produto.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estoque>> GetEstoqueBaixoAsync()
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .Where(e => e.QuantidadeAtual <= e.QuantidadeMinima && e.QuantidadeAtual > 0)
                .OrderBy(e => e.QuantidadeAtual)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estoque>> GetEstoqueEsgotadoAsync()
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .Where(e => e.QuantidadeAtual <= 0)
                .ToListAsync();
        }

        public async Task AddAsync(Estoque estoque)
        {
            await _context.Estoques.AddAsync(estoque);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Estoque estoque)
        {
            estoque.UltimaAtualizacao = DateTime.Now;
            _context.Estoques.Update(estoque);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var estoque = await GetByIdAsync(id);
            if (estoque != null)
            {
                _context.Estoques.Remove(estoque);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly ApplicationDbContext _context;

        public MovimentacaoEstoqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MovimentacaoEstoque?> GetByIdAsync(int id)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .Include(m => m.Pedido)
                .FirstOrDefaultAsync(m => m.idMovimentacao == id);
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync()
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .Where(m => m.idProduto == produtoId)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .Where(m => m.DataMovimentacao >= dataInicio && m.DataMovimentacao < dataFim)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync();
        }

        public async Task AddAsync(MovimentacaoEstoque movimentacao)
        {
            await _context.MovimentacoesEstoque.AddAsync(movimentacao);
            await _context.SaveChangesAsync();
        }
    }


}
