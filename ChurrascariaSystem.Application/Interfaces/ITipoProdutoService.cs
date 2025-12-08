using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface ITipoProdutoService
    {
        Task<TipoProdutoDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TipoProdutoDTO>> GetAllAsync();
        Task<IEnumerable<TipoProdutoDTO>> GetAllActiveAsync();
        Task<TipoProdutoDTO> CreateAsync(TipoProdutoDTO tipoProdutoDto);
        Task UpdateAsync(TipoProdutoDTO tipoProdutoDto);
        Task DeleteAsync(int id);
    }
}
