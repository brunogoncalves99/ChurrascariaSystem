using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ITipoProdutoRepository _tipoProdutoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, ITipoProdutoRepository tipoProdutoRepository)
        {
            _produtoRepository = produtoRepository;
            _tipoProdutoRepository = tipoProdutoRepository;
        }

        public async Task<ProdutoDTO?> GetByIdAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            return produto == null ? null : MapToDTO(produto);
        }

        public async Task<IEnumerable<ProdutoDTO>> GetAllAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            return produtos.Select(MapToDTO);
        }

        public async Task<IEnumerable<ProdutoDTO>> GetAllActiveAsync()
        {
            var produtos = await _produtoRepository.GetAllActiveAsync();
            return produtos.Select(MapToDTO);
        }

        public async Task<IEnumerable<ProdutoDTO>> GetByTipoAsync(int tipoProdutoId)
        {
            var produtos = await _produtoRepository.GetByTipoAsync(tipoProdutoId);
            return produtos.Select(MapToDTO);
        }

        public async Task<ProdutoDTO> CreateAsync(ProdutoDTO produtoDto)
        {
            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                PrecoValor = produtoDto.Preco,
                idTipoProduto = produtoDto.idTipoProduto,
                ImagemUrl = produtoDto.ImagemUrl,
                Ativo = produtoDto.Ativo
            };

            await _produtoRepository.AddAsync(produto);
            return MapToDTO(produto);
        }

        public async Task UpdateAsync(ProdutoDTO produtoDto)
        {
            var produto = await _produtoRepository.GetByIdAsync(produtoDto.idProduto);
            if (produto == null) throw new Exception("Produto não encontrado");

            produto.Nome = produtoDto.Nome;
            produto.Descricao = produtoDto.Descricao;
            produto.PrecoValor = produtoDto.Preco;
            produto.idTipoProduto = produtoDto.idTipoProduto;
            produto.ImagemUrl = produtoDto.ImagemUrl;
            produto.Ativo = produtoDto.Ativo;

            await _produtoRepository.UpdateAsync(produto);
        }

        public async Task DeleteAsync(int id)
        {
            await _produtoRepository.DeleteAsync(id);
        }

        private ProdutoDTO MapToDTO(Produto produto)
        {
            return new ProdutoDTO
            {
                idProduto = produto.idProduto,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.PrecoValor,
                idTipoProduto = produto.idTipoProduto,
                TipoProdutoNome = produto.TipoProduto?.Nome,
                ImagemUrl = produto.ImagemUrl,
                Ativo = produto.Ativo
            };
        }
    }
}
