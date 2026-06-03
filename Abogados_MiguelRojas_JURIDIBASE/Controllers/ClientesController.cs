using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        // Optional: filtrar por abogadoId (muestra clientes relacionados con ese abogado mediante casos o citas)
        public async Task<IActionResult> Index(int? abogadoId)
        {
            if (abogadoId == null)
            {
                var all = await _context.cliente.ToListAsync();
                return View(all);
            }

            var clientes = await _context.cliente
                .Where(cl => _context.caso.Any(c => c.id_Cliente == cl.idCliente && c.id_Abogado == abogadoId)
                             || _context.cita.Any(ct => ct.id_Cliente == cl.idCliente && ct.id_Abogado == abogadoId))
                .ToListAsync();

            ViewBag.FilterAbogadoId = abogadoId;
            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.cliente.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente agregado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.cliente.FindAsync(id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.idCliente) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cliente actualizado.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.cliente.Any(e => e.idCliente == cliente.idCliente)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.cliente.FirstOrDefaultAsync(c => c.idCliente == id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.cliente.FirstOrDefaultAsync(c => c.idCliente == id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.cliente.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
