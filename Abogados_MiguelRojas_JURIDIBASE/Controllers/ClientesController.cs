using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? abogadoId)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            var idAbogadoFiltro = abogadoActual?.idAbogado ?? abogadoId;

            IQueryable<Cliente> consulta = _context.cliente.Include(c => c.abogado);

            if (idAbogadoFiltro != null)
            {
                consulta = consulta.Where(c => c.idAbogado == idAbogadoFiltro);
                ViewBag.FilterAbogadoId = idAbogadoFiltro;
            }

            var clientes = await consulta
                .OrderBy(c => c.nombreCliente)
                .ToListAsync();
            return View(clientes);
        }

        public async Task<IActionResult> Create()
        {
            await CargarAbogadosAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            LimpiarValidacionesDeNavegacion();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                cliente.idAbogado = abogadoActual.idAbogado;
            }

            if (ModelState.IsValid)
            {
                _context.cliente.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente agregado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarAbogadosAsync(cliente.idAbogado);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.cliente.Include(c => c.abogado).FirstOrDefaultAsync(c => c.idCliente == id.Value);
            if (cliente == null) return NotFound();
            if (!await PuedeVerClienteAsync(cliente)) return Forbid();

            await CargarAbogadosAsync(cliente.idAbogado);
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.idCliente) return NotFound();
            LimpiarValidacionesDeNavegacion();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                var clienteDb = await _context.cliente.AsNoTracking().FirstOrDefaultAsync(c => c.idCliente == id);
                if (clienteDb == null) return NotFound();
                if (clienteDb.idAbogado != abogadoActual.idAbogado) return Forbid();
                cliente.idAbogado = abogadoActual.idAbogado;
            }

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

            await CargarAbogadosAsync(cliente.idAbogado);
            return View(cliente);
        }

        // GET: Clientes/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.cliente
                .Include(c => c.abogado)
                .Include(c => c.pago)
                    .ThenInclude(p => p.caso)
                .FirstOrDefaultAsync(c => c.idCliente == id.Value);

            if (cliente == null) return NotFound();
            if (!await PuedeVerClienteAsync(cliente)) return Forbid();

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.cliente.Include(c => c.abogado).FirstOrDefaultAsync(c => c.idCliente == id.Value);
            if (cliente == null) return NotFound();
            if (!await PuedeVerClienteAsync(cliente)) return Forbid();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.cliente.FindAsync(id);
            if (cliente != null)
            {
                if (!await PuedeVerClienteAsync(cliente)) return Forbid();
                _context.cliente.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente eliminado.";
            }
            return RedirectToAction(nameof(Index));
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

        private async Task<bool> PuedeVerClienteAsync(Cliente cliente)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            return abogadoActual == null || cliente.idAbogado == abogadoActual.idAbogado;
        }

        private async Task CargarAbogadosAsync(int? idSeleccionado = null)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            var abogados = abogadoActual != null
                ? new List<Abogado> { abogadoActual }
                : await _context.abogados.Where(a => a.estadoAbogado).OrderBy(a => a.nombreAbogado).ToListAsync();

            ViewBag.Abogados = new SelectList(abogados, "idAbogado", "nombreAbogado", idSeleccionado ?? abogadoActual?.idAbogado);
            ViewBag.AbogadoActual = abogadoActual;
        }

        private void LimpiarValidacionesDeNavegacion()
        {
            ModelState.Remove(nameof(Cliente.abogado));
            ModelState.Remove(nameof(Cliente.cita));
            ModelState.Remove(nameof(Cliente.caso));
            ModelState.Remove(nameof(Cliente.pago));
        }
    }
}
