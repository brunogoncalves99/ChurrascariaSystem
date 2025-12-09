namespace ChurrascariaSystem.Application.DTOs
{
    public class ContaMesaDTO
    {
        public int idMesa { get; set; }
        public string MesaNumero { get; set; } = string.Empty;
        public int QuantidadePedidos { get; set; }
        public decimal ValorTotal { get; set; }
        public List<PedidoDTO> Pedidos { get; set; } = new List<PedidoDTO>();
        public DateTime? PrimeiroPedido { get; set; }
        public DateTime? UltimoPedido { get; set; }
        public bool PossuiPedidosAbertos { get; set; }
        public string Status { get; set; } = "Livre"; // Livre/Ocupada/AguardandoPagamento
    }
}
