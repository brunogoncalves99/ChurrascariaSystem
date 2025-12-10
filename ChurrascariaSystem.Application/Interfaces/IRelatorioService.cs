namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IRelatorioService
    {
        /// <summary>
        /// Gera PDF da conta do cliente de uma mesa específica
        /// </summary>
        Task<byte[]> GerarContaClienteAsync(int mesaId);

        /// <summary>
        /// Gera relatório de vendas diárias
        /// </summary>
        Task<byte[]> GerarRelatorioVendasDiariaAsync(DateTime data);

        /// <summary>
        /// Gera relatório de produtos mais vendidos em um período
        /// </summary>
        Task<byte[]> GerarRelatorioProdutosMaisVendidosAsync(DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Gera relatório de faturamento mensal
        /// </summary>
        Task<byte[]> GerarRelatorioFaturamentoMensalAsync(int mes, int ano);

        /// <summary>
        /// Gera relatório de vendas por forma de pagamento
        /// </summary>
        Task<byte[]> GerarRelatorioFormasPagamentoAsync(DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Gera relatório de performance dos garçons
        /// </summary>
        Task<byte[]> GerarRelatorioPerformanceGarcomAsync(DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Gera relatório customizado conforme parâmetros fornecidos
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<byte[]> GerarRelatorioCustomAsync(DateTime data);
    }
}
