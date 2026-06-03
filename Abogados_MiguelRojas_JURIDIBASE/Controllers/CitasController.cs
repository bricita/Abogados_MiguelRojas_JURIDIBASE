using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var citas = await _context.cita
                .Include(c => c.abogado)
                .Include(c => c.cliente)
                .ToListAsync();

            return View(citas);
        }

        public IActionResult Create()
        {
            ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado");
            ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.cita.Add(cita);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cita creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", cita.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            return View(cita);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.cita.FindAsync(id.Value);
            if (cita == null) return NotFound();

            ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", cita.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cita cita)
        {
            if (id != cita.idCita) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cita actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.idCita)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", cita.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            return View(cita);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.cita
                .Include(c => c.abogado)
                .Include(c => c.cliente)
                .FirstOrDefaultAsync(c => c.idCita == id.Value);

            if (cita == null) return NotFound();

            return View(cita);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.cita
                .Include(c => c.abogado)
                .Include(c => c.cliente)
                .FirstOrDefaultAsync(c => c.idCita == id.Value);

            if (cita == null) return NotFound();

            return View(cita);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.cita.FindAsync(id);
            if (cita != null)
            {
                _context.cita.Remove(cita);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cita eliminada.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.cita.Any(e => e.idCita == id);
        }
    }
}
