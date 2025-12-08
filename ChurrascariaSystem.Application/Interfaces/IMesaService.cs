using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IMesaService
    {
        Task<MesaDTO?> GetByIdAsync(int id);
        Task<IEnumerable<MesaDTO>> GetAllAsync();
        Task<IEnumerable<MesaDTO>> GetAllActiveAsync();
        Task<IEnumerable<MesaDTO>> GetMesasLivresAsync();
        Task<MesaDTO> CreateAsync(MesaDTO mesaDto);
        Task UpdateAsync(MesaDTO mesaDto);
        Task UpdateStatusAsync(int id, int status);
        Task DeleteAsync(int id);
    }
}
