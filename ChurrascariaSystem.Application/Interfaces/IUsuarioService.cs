using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO?> GetByIdAsync(int id);
        Task<UsuarioDTO?> GetByCpfAsync(string cpf);
        Task<IEnumerable<UsuarioDTO>> GetAllAsync();
        Task<IEnumerable<UsuarioDTO>> GetAllActiveAsync();
        Task<UsuarioDTO> CreateAsync(UsuarioDTO usuarioDto);
        Task UpdateAsync(UsuarioDTO usuarioDto);
        Task DeleteAsync(int id);
        Task<UsuarioDTO?> AuthenticateAsync(string cpf, string senha);
        Task<bool> CpfExistsAsync(string cpf);
    }
}
