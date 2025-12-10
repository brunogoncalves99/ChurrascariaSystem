namespace ChurrascariaSystem.Domain.Entities
{
    public class Estoque
    {
        public int idEstoque { get; set; }
        public int idProduto { get; set; }
        public int QuantidadeAtual { get; set; }
        public int QuantidadeMinima { get; set; } = 15; 
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;

        public Produto Produto { get; set; } = null!;

        // Propriedade calculada
        public bool EstoqueBaixo => QuantidadeAtual <= QuantidadeMinima;
        public bool EstoqueEsgotado => QuantidadeAtual <= 0;

    }
}