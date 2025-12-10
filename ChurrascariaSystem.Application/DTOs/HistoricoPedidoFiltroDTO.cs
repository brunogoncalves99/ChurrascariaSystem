using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class HistoricoPedidoFiltroDTO
    {
        [Display(Name = "Data Início")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        public DateTime? DataFim { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Mesa")]
        public int? idMesa { get; set; }

        [Display(Name = "Garçom")]
        public int? idUsuario { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public string? FormaPagamento { get; set; }

        [Display(Name = "Apenas Pagos")]
        public bool? ApenasPagos { get; set; }

        [Display(Name = "Apenas Não Pagos")]
        public bool? ApenasNaoPagos { get; set; }
    }
}
