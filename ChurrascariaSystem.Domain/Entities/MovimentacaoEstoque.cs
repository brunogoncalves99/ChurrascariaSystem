namespace ChurrascariaSystem.Domain.Entities
{
    public class MovimentacaoEstoque
    {
        public int idMovimentacao { get; set; }
        public int idProduto { get; set; }
        public string TipoMovimentacao { get; set; } = string.Empty; 
        public int Quantidade { get; set; }
        public int QuantidadeAnterior { get; set; }
        public int QuantidadeNova { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public int? idPedido { get; set; } 
        public int? idUsuario { get; set; } 
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;
        public string? Observacao { get; set; }

        public Produto Produto { get; set; } = null!;
        public Pedido? Pedido { get; set; }
        public Usuario? Usuario { get; set; }
    }
}