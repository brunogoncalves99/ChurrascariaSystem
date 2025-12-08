using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardDataAsync();
        Task<decimal> GetFaturamentoDiaAsync(DateTime data);
        Task<decimal> GetFaturamentoMesAsync(int mes, int ano);
        Task<int> GetTotalPedidosDiaAsync(DateTime data);
        Task<int> GetTotalPedidosMesAsync(int mes, int ano);
    }
}
