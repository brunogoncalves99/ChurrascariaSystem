using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class ItemPedidoDTO
    {
        public int idItemPedido { get; set; }
        public int idPedido { get; set; }

        [Required(ErrorMessage = "Produto é obrigatório")]
        public int idProduto { get; set; }

        public string? ProdutoNome { get; set; }

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, 100, ErrorMessage = "Quantidade deve estar entre 1 e 100")]
        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }

        [StringLength(200, ErrorMessage = "Observação deve ter no máximo 200 caracteres")]
        public string? Observacao { get; set; }
    }
}
