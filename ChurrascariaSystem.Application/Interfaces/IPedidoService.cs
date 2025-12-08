using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDTO?> GetByIdAsync(int id);
        Task<IEnumerable<PedidoDTO>> GetAllAsync();
        Task<IEnumerable<PedidoDTO>> GetByMesaAsync(int mesaId);
        Task<IEnumerable<PedidoDTO>> GetPedidosAbertosAsync();
        Task<PedidoDTO> CreateAsync(PedidoDTO pedidoDto);
        Task UpdateAsync(PedidoDTO pedidoDto);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
        Task<decimal> GetValorTotalMesaAsync(int mesaId);
    }
}
