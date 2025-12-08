using ChurrascariaSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class MesaDTO
    {
        public int idMesa { get; set; }

        [Required(ErrorMessage = "Número da mesa é obrigatório")]
        [StringLength(10, ErrorMessage = "Número deve ter no máximo 10 caracteres")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacidade é obrigatória")]
        [Range(1, 20, ErrorMessage = "Capacidade deve estar entre 1 e 20 pessoas")]
        public int Capacidade { get; set; }
        public StatusMesa Status { get; set; } = StatusMesa.Livre;
        public string StatusDescricao { get; set; } = "Livre";
        public bool Ativo { get; set; } = true;
    }
}
