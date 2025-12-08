using ChurrascariaSystem.Domain.Enums;

namespace ChurrascariaSystem.Domain.Entities
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty; 
        public string Senha { get; set; } = string.Empty;
        public TipoUsuario TipoUsuario { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataModificacao {  get; set; } = DateTime.Now;
    }
}
