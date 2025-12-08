using ChurrascariaSystem.Domain.ValueObjects;

namespace ChurrascariaSystem.Domain.Entities
{
    public class Produto
    {
        public int idProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal PrecoValor { get; set; } 
        public int idTipoProduto { get; set; }
        public bool Ativo { get; set; } = true;
        public string? ImagemUrl { get; set; }

        // Navigation Properties
        public TipoProduto TipoProduto { get; set; } = null!;
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();

        public Preco Preco
        {
            get => new Preco(PrecoValor);
            set => PrecoValor = value.Valor;
        }
    }
}
