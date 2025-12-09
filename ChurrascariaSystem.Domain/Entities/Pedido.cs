using ChurrascariaSystem.Domain.ValueObjects;

namespace ChurrascariaSystem.Domain.Entities
{
    public class Pedido
    {
        public int idPedido { get; set; }
        public int idMesa { get; set; }
        public int idUsuario { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.Now;
        public string StatusPedidoValor { get; set; } = "Aberto"; 
        public decimal ValorTotal { get; set; }
        public string? Observacao { get; set; }
        public bool Pago { get; set; } = false; 
        public DateTime? DataPagamento { get; set; } 
        public string? FormaPagamento { get; set; } 

        public Mesa Mesa { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        public StatusPedido Status
        {
            get => StatusPedido.Criar(StatusPedidoValor);
            set => StatusPedidoValor = value.Valor;
        }

        // Método para calcular o valor total
        public void CalcularValorTotal()
        {
            ValorTotal = Itens.Sum(item => item.Subtotal);
        }
    }
}
