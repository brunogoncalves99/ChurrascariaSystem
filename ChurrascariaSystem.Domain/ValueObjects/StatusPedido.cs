namespace ChurrascariaSystem.Domain.ValueObjects
{
    public class StatusPedido
    {
        public string Valor { get; private set; }

        public static readonly StatusPedido Aberto = new StatusPedido("Aberto");
        public static readonly StatusPedido EmPreparacao = new StatusPedido("Em Preparação");
        public static readonly StatusPedido Pronto = new StatusPedido("Pronto");
        public static readonly StatusPedido Entregue = new StatusPedido("Entregue");
        public static readonly StatusPedido Cancelado = new StatusPedido("Cancelado");

        private StatusPedido(string valor)
        {
            Valor = valor;
        }

        public static StatusPedido Criar(string valor)
        {
            return valor switch
            {
                "Aberto" => Aberto,
                "Em Preparação" => EmPreparacao,
                "Pronto" => Pronto,
                "Entregue" => Entregue,
                "Cancelado" => Cancelado,
                _ => throw new ArgumentException("Status inválido")
            };
        }

        public override string ToString() => Valor;
    }
}
