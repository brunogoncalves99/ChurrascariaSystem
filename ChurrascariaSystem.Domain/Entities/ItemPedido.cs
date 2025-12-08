namespace ChurrascariaSystem.Domain.Entities
{
    public class ItemPedido
    {
        public int idItemPedido { get; set; }
        public int idPedido { get; set; }
        public int idProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string? Observacao { get; set; }

        // Navigation Properties
        public Pedido Pedido { get; set; } = null!;
        public Produto Produto { get; set; } = null!;

        // Método para calcular o subtotal
        public void CalcularSubtotal()
        {
            Subtotal = Quantidade * PrecoUnitario;
        }
    }
}
