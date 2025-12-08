using ChurrascariaSystem.Domain.Enums;

namespace ChurrascariaSystem.Domain.Entities
{
    public class Mesa
    {
        public int idMesa { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int Capacidade { get; set; }
        public StatusMesa Status { get; set; } = StatusMesa.Livre;
        public bool Ativo { get; set; } = true;

        // Navigation Property
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
