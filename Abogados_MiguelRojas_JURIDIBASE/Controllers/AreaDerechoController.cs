using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class AreaDerechoController : Controller
    {
        private readonly AppDbContext _context;

        public AreaDerechoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var areas = await _context.areasDerecho.ToListAsync();
            return View(areas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AreaDerecho areaDerecho)
        {
            if (ModelState.IsValid)
            {
                _context.areasDerecho.Add(areaDerecho);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Área de Derecho agregada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(areaDerecho);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var areaDerecho = await _context.areasDerecho.FindAsync(id.Value);
            if (areaDerecho == null) return NotFound();
            return View(areaDerecho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AreaDerecho areaDerecho)
        {
            if (id != areaDerecho.idAreaDerecho) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(areaDerecho);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Área de Derecho actualizada.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.areasDerecho.Any(e => e.idAreaDerecho == areaDerecho.idAreaDerecho))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(areaDerecho);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var areaDerecho = await _context.areasDerecho
                .FirstOrDefaultAsync(a => a.idAreaDerecho == id.Value);
            if (areaDerecho == null) return NotFound();
            return View(areaDerecho);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var areaDerecho = await _context.areasDerecho
                .FirstOrDefaultAsync(a => a.idAreaDerecho == id.Value);
            if (areaDerecho == null) return NotFound();
            return View(areaDerecho);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var areaDerecho = await _context.areasDerecho.FindAsync(id);
            if (areaDerecho != null)
            {
                _context.areasDerecho.Remove(areaDerecho);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Área de Derecho eliminada.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}