using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class TipoProdutoDTO
    {
        public int idTipoProduto { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres")]
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
