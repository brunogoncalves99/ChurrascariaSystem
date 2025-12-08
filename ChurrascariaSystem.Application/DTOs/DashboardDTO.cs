namespace ChurrascariaSystem.Application.DTOs
{
    public class DashboardDTO
    {
        public decimal FaturamentoDia { get; set; }
        public decimal FaturamentoMes { get; set; }
        public int TotalPedidosDia { get; set; }
        public int TotalPedidosMes { get; set; }
        public int TotalMesasOcupadas { get; set; }
        public int TotalMesasLivres { get; set; }
        public List<PedidoDTO> PedidosAbertos { get; set; } = new List<PedidoDTO>();
        public List<ProdutoMaisVendidoDTO> ProdutosMaisVendidos { get; set; } = new List<ProdutoMaisVendidoDTO>();
        public List<FaturamentoDiarioDTO> FaturamentoUltimos7Dias { get; set; } = new List<FaturamentoDiarioDTO>();
    }

    public class ProdutoMaisVendidoDTO
    {
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class FaturamentoDiarioDTO
    {
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }
}
