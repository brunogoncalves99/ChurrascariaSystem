using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class TipoProdutoService : ITipoProdutoService
    {
        private readonly ITipoProdutoRepository _tipoProdutoRepository;

        public TipoProdutoService(ITipoProdutoRepository tipoProdutoRepository)
        {
            _tipoProdutoRepository = tipoProdutoRepository;
        }

        public async Task<TipoProdutoDTO?> GetByIdAsync(int id)
        {
            var tipoProduto = await _tipoProdutoRepository.GetByIdAsync(id);
            return tipoProduto == null ? null : MapToDTO(tipoProduto);
        }

        public async Task<IEnumerable<TipoProdutoDTO>> GetAllAsync()
        {
            var tiposProduto = await _tipoProdutoRepository.GetAllAsync();
            return tiposProduto.Select(MapToDTO);
        }

        public async Task<IEnumerable<TipoProdutoDTO>> GetAllActiveAsync()
        {
            var tiposProduto = await _tipoProdutoRepository.GetAllActiveAsync();
            return tiposProduto.Select(MapToDTO);
        }

        public async Task<TipoProdutoDTO> CreateAsync(TipoProdutoDTO tipoProdutoDto)
        {
            var tipoProduto = new TipoProduto
            {
                Nome = tipoProdutoDto.Nome,
                Descricao = tipoProdutoDto.Descricao,
                Ativo = tipoProdutoDto.Ativo
            };

            await _tipoProdutoRepository.AddAsync(tipoProduto);
            return MapToDTO(tipoProduto);
        }

        public async Task UpdateAsync(TipoProdutoDTO tipoProdutoDto)
        {
            var tipoProduto = await _tipoProdutoRepository.GetByIdAsync(tipoProdutoDto.idTipoProduto);
            if (tipoProduto == null) throw new Exception("Tipo de produto não encontrado");

            tipoProduto.Nome = tipoProdutoDto.Nome;
            tipoProduto.Descricao = tipoProdutoDto.Descricao;
            tipoProduto.Ativo = tipoProdutoDto.Ativo;

            await _tipoProdutoRepository.UpdateAsync(tipoProduto);
        }

        public async Task DeleteAsync(int id)
        {
            await _tipoProdutoRepository.DeleteAsync(id);
        }

        private TipoProdutoDTO MapToDTO(TipoProduto tipoProduto)
        {
            return new TipoProdutoDTO
            {
                idTipoProduto = tipoProduto.idTipoProduto,
                Nome = tipoProduto.Nome,
                Descricao = tipoProduto.Descricao,
                Ativo = tipoProduto.Ativo
            };
        }
    }
}
