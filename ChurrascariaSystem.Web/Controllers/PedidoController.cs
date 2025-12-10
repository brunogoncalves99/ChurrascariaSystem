using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly IMesaService _mesaService;
        private readonly IProdutoService _produtoService;
        private readonly ITipoProdutoService _tipoProdutoService;

        public PedidoController(
            IPedidoService pedidoService,
            IMesaService mesaService,
            IProdutoService produtoService,
            ITipoProdutoService tipoProdutoService)
        {
            _pedidoService = pedidoService;
            _mesaService = mesaService;
            _produtoService = produtoService;
            _tipoProdutoService = tipoProdutoService;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoService.GetPedidosAbertosAsync();
            return View(pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            ViewBag.Mesas = await _mesaService.GetAllActiveAsync();
            ViewBag.TiposProduto = await _tipoProdutoService.GetAllActiveAsync();
            ViewBag.Produtos = await _produtoService.GetAllActiveAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] PedidoDTO pedidoDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dados inválidos" });
            }

            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                pedidoDto.idUsuario = int.Parse(usuarioId!);

                await _pedidoService.CreateAsync(pedidoDto);
                return Json(new { success = true, message = "Pedido criado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpGet]
        public async Task<IActionResult> PorMesa(int mesaId)
        {
            var pedidos = await _pedidoService.GetByMesaAsync(mesaId);
            return View("Index", pedidos);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                await _pedidoService.UpdateStatusAsync(id, status);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetValorTotalMesa(int mesaId)
        {
            var valorTotal = await _pedidoService.GetValorTotalMesaAsync(mesaId);
            return Json(new { valorTotal });
        }

        [HttpGet]
        public async Task<IActionResult> Historico()
        {
            ViewBag.Mesas = await _mesaService.GetAllAsync();
            ViewBag.Usuarios = await _pedidoService.GetAllAsync(); 

            var filtros = new HistoricoPedidoFiltroDTO
            {
                DataInicio = DateTime.Today.AddDays(-30),
                DataFim = DateTime.Today
            };

            var pedidos = await _pedidoService.GetHistoricoAsync(filtros);
            ViewBag.Filtros = filtros;
            return View(pedidos);
        }

        [HttpPost]
        public async Task<IActionResult> Historico(HistoricoPedidoFiltroDTO filtros)
        {
            ViewBag.Mesas = await _mesaService.GetAllAsync();
            ViewBag.Usuarios = await _pedidoService.GetAllAsync();

            var pedidos = await _pedidoService.GetHistoricoAsync(filtros);
            ViewBag.Filtros = filtros;
            return View(pedidos);
        }
    }
}
