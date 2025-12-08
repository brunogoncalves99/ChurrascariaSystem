using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize]
    public class MesaController : Controller
    {
        private readonly IMesaService _mesaService;

        public MesaController(IMesaService mesaService)
        {
            _mesaService = mesaService;
        }

        public async Task<IActionResult> Index()
        {
            var mesas = await _mesaService.GetAllActiveAsync();
            return View(mesas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MesaDTO mesaDto)
        {
            if (!ModelState.IsValid)
            {
                return View(mesaDto);
            }

            await _mesaService.CreateAsync(mesaDto);
            TempData["Success"] = "Mesa criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mesa = await _mesaService.GetByIdAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MesaDTO mesaDto)
        {
            if (!ModelState.IsValid)
            {
                return View(mesaDto);
            }

            await _mesaService.UpdateAsync(mesaDto);
            TempData["Success"] = "Mesa atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _mesaService.DeleteAsync(id);
            TempData["Success"] = "Mesa excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> Gerenciar()
        {
            var mesas = await _mesaService.GetAllActiveAsync();
            return View(mesas);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            await _mesaService.UpdateStatusAsync(id, status);
            return Json(new { success = true });
        }
    }
}
