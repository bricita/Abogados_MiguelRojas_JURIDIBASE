using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class AudienciaController : Controller
    {
        private readonly AppDbContext _context;

        public AudienciaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var audiencias = await _context.audiencia
                .Include(a => a.abogado)
                .Include(a => a.caso)
                .ToListAsync();
            return View(audiencias);
        }

        public IActionResult Create()
        {
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado");
            ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Audiencia audiencia)
        {
            if (ModelState.IsValid)
            {
                _context.audiencia.Add(audiencia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Audiencia registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
            ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso", audiencia.id_Caso);
            return View(audiencia);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var audiencia = await _context.audiencia.FindAsync(id.Value);
            if (audiencia == null) return NotFound();
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
            ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso", audiencia.id_Caso);
            return View(audiencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Audiencia audiencia)
        {
            if (id != audiencia.idAudiencia) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(audiencia);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Audiencia actualizada.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.audiencia.Any(e => e.idAudiencia == audiencia.idAudiencia))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
            ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso", audiencia.id_Caso);
            return View(audiencia);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var audiencia = await _context.audiencia
                .Include(a => a.abogado)
                .Include(a => a.caso)
                .FirstOrDefaultAsync(a => a.idAudiencia == id.Value);
            if (audiencia == null) return NotFound();
            return View(audiencia);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var audiencia = await _context.audiencia
                .Include(a => a.abogado)
                .Include(a => a.caso)
                .FirstOrDefaultAsync(a => a.idAudiencia == id.Value);
            if (audiencia == null) return NotFound();
            return View(audiencia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var audiencia = await _context.audiencia.FindAsync(id);
            if (audiencia != null)
            {
                _context.audiencia.Remove(audiencia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Audiencia eliminada.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}