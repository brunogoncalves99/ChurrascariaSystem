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
        [Range(0.01, 9999.99, ErrorMessage = "Preço deve estar entre R$ 0,01 e R$ 9.999,99")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Tipo de produto é obrigatório")]
        public int idTipoProduto { get; set; }

        public string? TipoProdutoNome { get; set; }
        public string? ImagemUrl { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
