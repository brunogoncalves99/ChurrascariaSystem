using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class ProdutoDTO
    {
        public int idProduto { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Preço é obrigatório")]
        [Range(1.0, 999.99, ErrorMessage = "Preço deve estar entre R$ 1,00 e R$ 999.99")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Tipo de produto é obrigatório")]
        public int idTipoProduto { get; set; }
        public string? TipoProdutoNome { get; set; }
        public string? ImagemUrl { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
