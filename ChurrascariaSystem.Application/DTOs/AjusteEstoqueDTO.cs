namespace ChurrascariaSystem.Application.DTOs
{
    public class AjusteEstoqueDTO
    {
        public int idProduto { get; set; }
        public int NovaQuantidade { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
