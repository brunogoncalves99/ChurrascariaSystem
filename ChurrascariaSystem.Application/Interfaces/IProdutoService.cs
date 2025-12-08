using ChurrascariaSystem.Application.DTOs;

namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ProdutoDTO>> GetAllAsync();
        Task<IEnumerable<ProdutoDTO>> GetAllActiveAsync();
        Task<IEnumerable<ProdutoDTO>> GetByTipoAsync(int tipoProdutoId);
        Task<ProdutoDTO> CreateAsync(ProdutoDTO produtoDto);
        Task UpdateAsync(ProdutoDTO produtoDto);
        Task DeleteAsync(int id);
    }
}
