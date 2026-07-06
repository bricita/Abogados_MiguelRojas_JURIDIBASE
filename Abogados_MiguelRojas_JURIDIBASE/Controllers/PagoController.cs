using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class PagoController : Controller
    {
        private readonly AppDbContext _context;

        public PagoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int? clienteId)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();

            IQueryable<Pago> consulta = _context.pago
                .Include(p => p.abogado)
                .Include(p => p.cliente)
                .Include(p => p.caso);

            if (abogadoActual != null)
            {
                consulta = consulta.Where(p => p.idAbogado == abogadoActual.idAbogado);
                ViewBag.AbogadoActual = abogadoActual;
            }

            if (clienteId != null)
            {
                consulta = consulta.Where(p => p.idCliente == clienteId);
                ViewBag.FilterClienteId = clienteId;
            }

            var pagos = await consulta
                .OrderByDescending(p => p.fechaPago)
                .ToListAsync();

            return View(pagos);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo(int? clienteId)
        {
            await CargarCombosAsync(clienteId: clienteId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(Pago pago)
        {
            LimpiarValidacionesDeNavegacion();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                pago.idAbogado = abogadoActual.idAbogado;
            }

            if (ModelState.IsValid)
            {
                if (!await PagoPerteneceAlAbogadoAsync(pago))
                {
                    ModelState.AddModelError("", "El cliente y el caso seleccionados no coinciden con el abogado asignado. Verifique su selección.");
                }
                else
                {
                    _context.pago.Add(pago);
                    await _context.SaveChangesAsync();

                    var cliente = await _context.cliente.FindAsync(pago.idCliente);
                    await Services.NotificationService.NotificarPagoAsync(_context, pago.idAbogado, cliente?.nombreCliente ?? "—", (decimal)pago.monto);

                    TempData["Success"] = "Pago registrado correctamente.";
                    return RedirectToAction(nameof(Listar));
                }
            }

            if (pago.fechaPago > DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError("fechaPago", "No se permiten registrar pagos con fechas futuras.");
            }
            await CargarCombosAsync(pago.idAbogado, pago.idCliente, pago.id_Caso);
            return View(pago);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var pago = await _context.pago
                .Include(p => p.abogado)
                .Include(p => p.cliente)
                .Include(p => p.caso)
                .FirstOrDefaultAsync(p => p.idPago == id);

            if (pago == null) return NotFound();
            if (!await PuedeVerPagoAsync(pago)) return Forbid();

            return View(pago);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var pago = await _context.pago
                .Include(p => p.cliente)
                .FirstOrDefaultAsync(p => p.idPago == id);

            if (pago == null) return NotFound();
            if (!await PuedeVerPagoAsync(pago)) return Forbid();

            await CargarCombosAsync(pago.idAbogado, pago.idCliente, pago.id_Caso);
            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Pago pago)
        {
            if (id != pago.idPago) return NotFound();
            LimpiarValidacionesDeNavegacion();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                var pagoDb = await _context.pago.AsNoTracking().FirstOrDefaultAsync(p => p.idPago == id);
                if (pagoDb == null) return NotFound();
                if (pagoDb.idAbogado != abogadoActual.idAbogado) return Forbid();
                pago.idAbogado = abogadoActual.idAbogado;
            }

            if (ModelState.IsValid)
            {
                if (!await PagoPerteneceAlAbogadoAsync(pago))
                {
                    ModelState.AddModelError("", "El cliente y el caso seleccionados no coinciden con el abogado asignado. Verifique su selección.");
                }
                else
                {
                    try
                    {
                        _context.pago.Update(pago);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Pago actualizado correctamente.";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await _context.pago.AnyAsync(p => p.idPago == pago.idPago)) return NotFound();
                        throw;
                    }

                    return RedirectToAction(nameof(Listar));
                }
            }

            await CargarCombosAsync(pago.idAbogado, pago.idCliente, pago.id_Caso);
            return View(pago);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var pago = await _context.pago
                .Include(p => p.abogado)
                .Include(p => p.cliente)
                .Include(p => p.caso)
                .FirstOrDefaultAsync(p => p.idPago == id);

            if (pago == null) return NotFound();
            if (!await PuedeVerPagoAsync(pago)) return Forbid();

            return View(pago);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var pago = await _context.pago.FindAsync(id);
            if (pago == null) return NotFound();
            if (!await PuedeVerPagoAsync(pago)) return Forbid();

            _context.pago.Remove(pago);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pago eliminado correctamente.";

            return RedirectToAction(nameof(Listar));
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

        private async Task<bool> PuedeVerPagoAsync(Pago pago)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            return abogadoActual == null || pago.idAbogado == abogadoActual.idAbogado;
        }

        private async Task<bool> PagoPerteneceAlAbogadoAsync(Pago pago)
        {
            var cliente = await _context.cliente.AsNoTracking().FirstOrDefaultAsync(c => c.idCliente == pago.idCliente);
            if (cliente == null || cliente.idAbogado != pago.idAbogado) return false;

            var caso = await _context.caso.AsNoTracking().FirstOrDefaultAsync(c => c.idCaso == pago.id_Caso);
            return caso != null && caso.id_Cliente == pago.idCliente && caso.id_Abogado == pago.idAbogado;
        }

        private async Task CargarCombosAsync(int? idAbogado = null, int? idCliente = null, int? idCaso = null, int? clienteId = null)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            var abogadoFiltro = abogadoActual?.idAbogado ?? idAbogado;

            var abogados = abogadoActual != null
                ? new List<Abogado> { abogadoActual }
                : await _context.abogados.Where(a => a.estadoAbogado).OrderBy(a => a.nombreAbogado).ToListAsync();

            var clientesQuery = _context.cliente.Where(c => c.estadoCliente);
            if (abogadoFiltro != null)
            {
                clientesQuery = clientesQuery.Where(c => c.idAbogado == abogadoFiltro);
            }

            var clientes = await clientesQuery.OrderBy(c => c.nombreCliente).ToListAsync();
            var clienteSeleccionado = clienteId ?? idCliente;

            var casosQuery = _context.caso.Where(c => c.estadoCaso);
            if (abogadoFiltro != null)
            {
                casosQuery = casosQuery.Where(c => c.id_Abogado == abogadoFiltro);
            }
            if (clienteSeleccionado != null)
            {
                casosQuery = casosQuery.Where(c => c.id_Cliente == clienteSeleccionado);
            }

            var casos = await casosQuery.OrderBy(c => c.tituloCaso).ToListAsync();

            ViewBag.Abogados = new SelectList(abogados, "idAbogado", "nombreAbogado", abogadoFiltro);
            ViewBag.Clientes = new SelectList(clientes, "idCliente", "nombreCliente", clienteSeleccionado);
            ViewBag.Casos = new SelectList(casos, "idCaso", "tituloCaso", idCaso);
            ViewBag.AbogadoActual = abogadoActual;
        }

        private void LimpiarValidacionesDeNavegacion()
        {
            ModelState.Remove(nameof(Pago.abogado));
            ModelState.Remove(nameof(Pago.cliente));
            ModelState.Remove(nameof(Pago.caso));
        }
    }
}
