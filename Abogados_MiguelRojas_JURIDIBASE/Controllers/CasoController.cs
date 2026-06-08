using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class CasoController : Controller
    {
        private readonly AppDbContext _context;

        public CasoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var casos = await _context.caso
                .Include(c => c.abogado)
                .Include(c => c.cliente)
                .ToListAsync();
            return View(casos);
        }

        public IActionResult Create()
        {
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado");
            ViewBag.Clientes = new SelectList(_context.cliente, "idCliente", "nombreCliente");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Caso caso)
        {
            if (ModelState.IsValid)
            {
                _context.caso.Add(caso);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Caso registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", caso.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente, "idCliente", "nombreCliente", caso.id_Cliente);
            return View(caso);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var caso = await _context.caso.FindAsync(id.Value);
            if (caso == null) return NotFound();
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", caso.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente, "idCliente", "nombreCliente", caso.id_Cliente);
            return View(caso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Caso caso)
        {
            if (id != caso.idCaso) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caso);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Caso actualizado.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.caso.Any(e => e.idCaso == caso.idCaso))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", caso.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente, "idCliente", "nombreCliente", caso.id_Cliente);
            return View(caso);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var caso = await _context.caso
                .Include(c => c.abogado)
                .Include(c => c.cliente)
                .FirstOrDefaultAsync(c => c.idCaso == id.Value);
            if (caso == null) return NotFound();
            return View(caso);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var caso = await _context.caso
                .Include(c => c.abogado)
                .Include(c => c.cliente)
                .FirstOrDefaultAsync(c => c.idCaso == id.Value);
            if (caso == null) return NotFound();
            return View(caso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caso = await _context.caso.FindAsync(id);
            if (caso != null)
            {
                _context.caso.Remove(caso);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Caso eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}