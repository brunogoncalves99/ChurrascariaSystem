using ChurrascariaSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize]
    public class RelatorioController : Controller
    {
        private readonly IRelatorioService _relatorioService;

        public RelatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        /// <summary>
        /// Página inicial de relatórios (menu)
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gera PDF da conta do cliente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ContaCliente(int idMesa)
        {
            try
            {
                var pdf = await _relatorioService.GerarContaClienteAsync(idMesa);
                return File(pdf, "application/pdf", $"Conta_Mesa_{idMesa}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar conta: {ex.Message}";
                return RedirectToAction("Index", "Mesa");
            }
        }

        /// <summary>
        /// Gera relatório de vendas diárias
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VendasDiaria(DateTime? data)
        {
            try
            {
                var dataRelatorio = data ?? DateTime.Today;
                var pdf = await _relatorioService.GerarRelatorioVendasDiariaAsync(dataRelatorio);
                return File(pdf, "application/pdf", $"Vendas_{dataRelatorio:yyyy-MM-dd}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Gera relatório de produtos mais vendidos
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProdutosMaisVendidos(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                var inicio = dataInicio ?? DateTime.Today.AddDays(-30);
                var fim = dataFim ?? DateTime.Today;

                var pdf = await _relatorioService.GerarRelatorioProdutosMaisVendidosAsync(inicio, fim);
                return File(pdf, "application/pdf", $"ProdutosMaisVendidos_{inicio:yyyy-MM-dd}_{fim:yyyy-MM-dd}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Gera relatório de faturamento mensal
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FaturamentoMensal(int? mes, int? ano)
        {
            try
            {
                var mesRelatorio = mes ?? DateTime.Today.Month;
                var anoRelatorio = ano ?? DateTime.Today.Year;

                var pdf = await _relatorioService.GerarRelatorioFaturamentoMensalAsync(mesRelatorio, anoRelatorio);
                return File(pdf, "application/pdf", $"Faturamento_{anoRelatorio}-{mesRelatorio:00}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Gera relatório de vendas por forma de pagamento
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FormasPagamento(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                var inicio = dataInicio ?? DateTime.Today.AddDays(-30);
                var fim = dataFim ?? DateTime.Today;

                var pdf = await _relatorioService.GerarRelatorioFormasPagamentoAsync(inicio, fim);
                return File(pdf, "application/pdf", $"FormasPagamento_{inicio:yyyy-MM-dd}_{fim:yyyy-MM-dd}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Gera relatório de performance dos garçons
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PerformanceGarcom(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                var inicio = dataInicio ?? DateTime.Today.AddDays(-30);
                var fim = dataFim ?? DateTime.Today;

                var pdf = await _relatorioService.GerarRelatorioPerformanceGarcomAsync(inicio, fim);
                return File(pdf, "application/pdf", $"PerformanceGarcom_{inicio:yyyy-MM-dd}_{fim:yyyy-MM-dd}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RelatorioCustom(DateTime data)
        {
            try
            {
                var pdf = await _relatorioService.GerarRelatorioCustomAsync(data);
                return File(pdf, "application/pdf", $"RelatorioCustomizado{data:yyyy-MM-dd}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}

