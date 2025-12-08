using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProdutoController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ITipoProdutoService _tipoProdutoService;

        public ProdutoController(IProdutoService produtoService, ITipoProdutoService tipoProdutoService)
        {
            _produtoService = produtoService;
            _tipoProdutoService = tipoProdutoService;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoService.GetAllActiveAsync();
            return View(produtos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CarregarTiposProduto();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProdutoDTO produtoDto)
        {
            if (!ModelState.IsValid)
            {
                await CarregarTiposProduto();
                return View(produtoDto);
            }

            await _produtoService.CreateAsync(produtoDto);
            TempData["Success"] = "Produto criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            await CarregarTiposProduto();
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProdutoDTO produtoDto)
        {
            if (!ModelState.IsValid)
            {
                await CarregarTiposProduto();
                return View(produtoDto);
            }

            await _produtoService.UpdateAsync(produtoDto);
            TempData["Success"] = "Produto atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _produtoService.DeleteAsync(id);
            TempData["Success"] = "Produto excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetByTipo(int tipoId)
        {
            var produtos = await _produtoService.GetByTipoAsync(tipoId);
            return Json(produtos);
        }

        private async Task CarregarTiposProduto()
        {
            var tipos = await _tipoProdutoService.GetAllActiveAsync();
            ViewBag.TiposProduto = new SelectList(tipos, "idTipoProduto", "Nome");
        }
    }
}
