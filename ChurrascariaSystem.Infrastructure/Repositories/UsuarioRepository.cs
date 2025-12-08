using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;
using ChurrascariaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChurrascariaSystem.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> GetByCpfAsync(string cpf)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Cpf == cpf);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllActiveAsync()
        {
            return await _context.Usuarios.Where(u => u.Ativo).ToListAsync();
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                usuario.Ativo = false;
                await UpdateAsync(usuario);
            }
        }

        public async Task<bool> CpfExistsAsync(string cpf)
        {
            return await _context.Usuarios.AnyAsync(u => u.Cpf == cpf);
        }
    }
}
