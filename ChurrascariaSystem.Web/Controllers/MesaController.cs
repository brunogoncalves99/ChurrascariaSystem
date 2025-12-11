using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurrascariaSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MesaDTO mesaDto)
        {
            if (mesaDto == null)
            {
                TempData["Error"] = "Dados inválidos. Verifique os campos.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _mesaService.CreateAsync(mesaDto);
                TempData["Success"] = $"Mesa {mesaDto.Numero} criada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao criar mesa: {ex.Message}";
            }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MesaDTO mesaDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dados inválidos. Verifique os campos.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _mesaService.UpdateAsync(mesaDto);
                TempData["Success"] = $"Mesa {mesaDto.Numero} atualizada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao atualizar mesa: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var mesa = await _mesaService.GetByIdAsync(id);
                await _mesaService.DeleteAsync(id);
                TempData["Success"] = $"Mesa {mesa?.Numero} excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao excluir mesa: {ex.Message}";
            }

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

        [HttpGet]
        public async Task<IActionResult> Contas()
        {
            var contas = await _mesaService.GetContasAbertasAsync();
            return View(contas);
        }

        [HttpGet]
        public async Task<IActionResult> DetalheConta(int id)
        {
            var conta = await _mesaService.GetContaMesaAsync(id);
            if (conta == null)
            {
                TempData["Error"] = "Nenhuma conta aberta encontrada para esta mesa.";
                return RedirectToAction(nameof(Contas));
            }

            return View(conta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FecharConta(int idMesa, string formaPagamento)
        {
            if (idMesa == 0)
            {
                TempData["Error"] = "Dados inválidos. Verifique os campos.";
                return RedirectToAction(nameof(DetalheConta));
            }

            try
            {
                var conta = await _mesaService.GetContaMesaAsync(idMesa);
                await _mesaService.FecharContaAsync(idMesa, formaPagamento);
                TempData["Success"] = $"Conta da Mesa {conta?.MesaNumero} fechada com sucesso! Total: R$ {conta?.ValorTotal:F2}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao fechar conta: {ex.Message}";
            }

            return RedirectToAction(nameof(Contas));
        }
    }
}