using ChurrascariaSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class UsuarioDTO
    {
        public int idUsuario {  get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "CPF inválido")]
        public string CPF { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 20 caracteres")]
        public string? Senha { get; set; }

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public TipoUsuario TipoUsuario { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
    }
}
