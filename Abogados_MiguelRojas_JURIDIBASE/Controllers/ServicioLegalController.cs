using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ServicioLegalController : Controller
    {
        private readonly AppDbContext _context;
        public ServicioLegalController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            return View(await _context.servicio.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicioLegal servicio)
        {
            if (ModelState.IsValid)
            {
                _context.servicio.Add(servicio);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio legal creado.";
                return RedirectToAction(nameof(Index));
            }
            return View(servicio);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var servicio = await _context.servicio.FindAsync(id);
            if (servicio == null) return NotFound();
            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicioLegal servicio)
        {
            if (id != servicio.idServicio) return NotFound();
            if (ModelState.IsValid)
            {
                _context.servicio.Update(servicio);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio legal actualizado.";
                return RedirectToAction(nameof(Index));
            }
            return View(servicio);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var servicio = await _context.servicio.FindAsync(id);
            if (servicio == null) return NotFound();
            return View(servicio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.servicio.FindAsync(id);
            if (servicio != null)
            {
                _context.servicio.Remove(servicio);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio legal eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
