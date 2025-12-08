using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class PedidoDTO
    {
        public int idPedido { get; set; }

        [Required(ErrorMessage = "Mesa é obrigatória")]
        public int idMesa { get; set; }
        public string? MesaNumero { get; set; }
        public int idUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Aberto";
        public decimal ValorTotal { get; set; }

        [StringLength(200, ErrorMessage = "Observação deve ter no máximo 200 caracteres")]
        public string? Observacao { get; set; }

        public List<ItemPedidoDTO> Itens { get; set; } = new List<ItemPedidoDTO>();
    }
}
