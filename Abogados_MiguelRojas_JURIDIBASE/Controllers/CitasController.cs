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
            var abogadoActual = await ObtenerAbogadoActualAsync();
            IQueryable<Cita> consulta = _context.cita
                .Include(c => c.abogado)
                .Include(c => c.cliente);

            if (abogadoActual != null)
            {
                consulta = consulta.Where(c => c.id_Abogado == abogadoActual.idAbogado);
            }

            var citas = await consulta.ToListAsync();
            return View(citas);
        }

        public async Task<IActionResult> Create()
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado");
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.idAbogado == abogadoActual.idAbogado && c.estadoCliente), "idCliente", "nombreCliente");
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado");
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cita cita)
        {
            ModelState.Remove("abogado");
            ModelState.Remove("cliente");

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                cita.id_Abogado = abogadoActual.idAbogado;
            }

            if (ModelState.IsValid)
            {
                _context.cita.Add(cita);
                await _context.SaveChangesAsync();

                var cliente = await _context.cliente.FindAsync(cita.id_Cliente);
                await Services.NotificationService.NotificarCitaCreadaAsync(_context, cita.id_Abogado, cliente?.nombreCliente ?? "—", cita.fechaHoraCita);

                TempData["Success"] = "Cita creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", cita.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.idAbogado == abogadoActual.idAbogado && c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", cita.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            }
            return View(cita);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.cita.FindAsync(id.Value);
            if (cita == null) return NotFound();
            if (!await PuedeVerCitaAsync(cita)) return Forbid();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", cita.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.idAbogado == abogadoActual.idAbogado && c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", cita.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            }
            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cita cita)
        {
            if (id != cita.idCita) return NotFound();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            ModelState.Remove("abogado");
            ModelState.Remove("cliente");

            if (abogadoActual != null)
            {
                var citaDb = await _context.cita.AsNoTracking().FirstOrDefaultAsync(c => c.idCita == id);
                if (citaDb == null) return NotFound();
                if (citaDb.id_Abogado != abogadoActual.idAbogado) return Forbid();
                cita.id_Abogado = abogadoActual.idAbogado;
            }

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

            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", cita.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.idAbogado == abogadoActual.idAbogado && c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", cita.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", cita.id_Cliente);
            }
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
            if (!await PuedeVerCitaAsync(cita)) return Forbid();

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
            if (!await PuedeVerCitaAsync(cita)) return Forbid();

            return View(cita);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.cita.FindAsync(id);
            if (cita == null) return NotFound();
            if (!await PuedeVerCitaAsync(cita)) return Forbid();

            _context.cita.Remove(cita);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cita eliminada.";

            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.cita.Any(e => e.idCita == id);
        }

        private async Task<Abogado?> ObtenerAbogadoActualAsync()
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            var idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");
            int? idUsuario = int.TryParse(idUsuarioClaim, out var claimId) ? claimId : idUsuarioSession;

            if (idUsuario == null) return null;

            return await _context.abogados
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.id_Usuario == idUsuario.Value);
        }

        private async Task<bool> PuedeVerCitaAsync(Cita cita)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            return abogadoActual == null || cita.id_Abogado == abogadoActual.idAbogado;
        }
    }
}
