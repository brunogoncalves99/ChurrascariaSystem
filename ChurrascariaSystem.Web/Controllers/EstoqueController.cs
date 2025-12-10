using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EstoqueController : Controller
    {
        private readonly IEstoqueService _estoqueService;
        private readonly IProdutoService _produtoService;

        public EstoqueController(IEstoqueService estoqueService, 
            IProdutoService produtoService)
        {
            _estoqueService = estoqueService;
            _produtoService = produtoService;
        }

        /// <summary>
        /// Página principal - Lista todos os estoques
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var estoques = await _estoqueService.GetAllAsync();
                var produtos = await _produtoService.GetAllAsync();

                ViewBag.ProdutosSemEstoque = produtos
                    .Where(p => !estoques.Any(e => e.idProduto == p.idProduto))
                    .ToList();

                var estoqueBaixo = estoques.Count(e => e.EstoqueBaixo);
                var estoqueEsgotado = estoques.Count(e => e.EstoqueEsgotado);

                ViewBag.TotalEstoqueBaixo = estoqueBaixo;
                ViewBag.TotalEstoqueEsgotado = estoqueEsgotado;

                return View(estoques);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar estoques: {ex.Message}";
                return View(new List<EstoqueDTO>());
            }
        }

        /// <summary>
        /// Página de alertas - Estoque baixo e esgotado
        /// </summary>
        public async Task<IActionResult> Alertas()
        {
            try
            {
                var estoqueBaixo = await _estoqueService.GetEstoqueBaixoAsync();
                var estoqueEsgotado = await _estoqueService.GetEstoqueEsgotadoAsync();

                ViewBag.EstoqueBaixo = estoqueBaixo;
                ViewBag.EstoqueEsgotado = estoqueEsgotado;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar alertas: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Criar novo registro de estoque
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EstoqueDTO estoqueDto)
        {
            try
            {
                var estoque = await _estoqueService.CreateAsync(estoqueDto);
                return Json(new { success = true, message = "Estoque criado com sucesso!", data = estoque });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Dar entrada no estoque (adicionar quantidade)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DarEntrada([FromBody] EntradaEstoqueDTO entrada)
        {
            try
            {
                var idUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var estoque = await _estoqueService.DarEntradaAsync(entrada, int.Parse(idUsuario!));

                return Json(new
                {
                    success = true,
                    message = $"Entrada de {entrada.Quantidade} unidades registrada com sucesso!",
                    data = estoque
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Ajustar estoque manualmente
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AjustarEstoque([FromBody] AjusteEstoqueDTO ajuste)
        {
            try
            {
                var idUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var estoque = await _estoqueService.AjustarEstoqueAsync(ajuste.idProduto, ajuste.NovaQuantidade, ajuste.Motivo, int.Parse(idUsuario!));

                return Json(new
                {
                    success = true,
                    message = "Estoque ajustado com sucesso!",
                    data = estoque
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Atualizar configurações de estoque (quantidade mínima)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateConfig([FromBody] EstoqueDTO estoqueDto)
        {
            try
            {
                var estoque = await _estoqueService.UpdateAsync(estoqueDto);
                return Json(new
                {
                    success = true,
                    message = "Configurações atualizadas com sucesso!",
                    data = estoque
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Obter histórico de movimentações de um produto
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMovimentacoes(int idProduto)
        {
            try
            {
                var movimentacoes = await _estoqueService.GetMovimentacoesByProdutoAsync(idProduto);
                return Json(new { success = true, data = movimentacoes });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Obter detalhes de um estoque
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDetalhes(int idProduto)
        {
            try
            {
                var estoque = await _estoqueService.GetByProdutoIdAsync(idProduto);
                if (estoque == null)
                {
                    return Json(new { success = false, message = "Estoque não encontrado" });
                }
                return Json(new { success = true, data = estoque });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Excluir registro de estoque
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _estoqueService.DeleteAsync(id);
                TempData["Sucesso"] = "Estoque removido com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao remover estoque: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
