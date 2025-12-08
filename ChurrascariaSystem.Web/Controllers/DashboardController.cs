using ChurrascariaSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return View(dashboardData);
        }

        [HttpGet]
        public async Task<IActionResult> GetFaturamentoChart()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return Json(dashboardData.FaturamentoUltimos7Dias);
        }
    }
}
