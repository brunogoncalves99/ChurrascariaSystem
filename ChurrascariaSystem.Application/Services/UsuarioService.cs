using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioDTO?> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario == null ? null : MapToDTO(usuario);
        }

        public async Task<UsuarioDTO?> GetByCpfAsync(string cpf)
        {
            var usuario = await _usuarioRepository.GetByCpfAsync(cpf);
            return usuario == null ? null : MapToDTO(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(MapToDTO);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllActiveAsync()
        {
            var usuarios = await _usuarioRepository.GetAllActiveAsync();
            return usuarios.Select(MapToDTO);
        }

        public async Task<UsuarioDTO> CreateAsync(UsuarioDTO usuarioDto)
        {
            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Cpf = usuarioDto.CPF,
                Senha = HashSenha(usuarioDto.Senha!), // Hash da senha
                TipoUsuario = usuarioDto.TipoUsuario,
                Ativo = usuarioDto.Ativo,
                DataCriacao = DateTime.Now
            };

            await _usuarioRepository.AddAsync(usuario);
            return MapToDTO(usuario);
        }

        public async Task UpdateAsync(UsuarioDTO usuarioDto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioDto.idUsuario);
            if (usuario == null) throw new Exception("Usuário não encontrado");

            usuario.Nome = usuarioDto.Nome;
            usuario.Cpf = usuarioDto.CPF;
            usuario.TipoUsuario = usuarioDto.TipoUsuario;
            usuario.Ativo = usuarioDto.Ativo;

            // Só atualiza a senha se ela foi informada
            if (!string.IsNullOrEmpty(usuarioDto.Senha))
            {
                usuario.Senha = HashSenha(usuarioDto.Senha);
            }

            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            await _usuarioRepository.DeleteAsync(id);
        }

        public async Task<UsuarioDTO?> AuthenticateAsync(string cpf, string senha)
        {
            var usuario = await _usuarioRepository.GetByCpfAsync(cpf);

            if (usuario == null || !usuario.Ativo)
                return null;

            // Verifica a senha
            if (VerificarSenha(senha, usuario.Senha))
            {
                return MapToDTO(usuario);
            }

            return null;
        }

        public async Task<bool> CpfExistsAsync(string cpf)
        {
            return await _usuarioRepository.CpfExistsAsync(cpf);
        }

        private string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }

        private UsuarioDTO MapToDTO(Usuario usuario)
        {
            return new UsuarioDTO
            {
                idUsuario = usuario.idUsuario,
                Nome = usuario.Nome,
                CPF = usuario.Cpf,
                TipoUsuario = usuario.TipoUsuario,
                Ativo = usuario.Ativo,
                DataCriacao = usuario.DataCriacao
            };
        }

    }
}
