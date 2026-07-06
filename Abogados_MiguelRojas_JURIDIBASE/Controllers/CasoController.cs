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
            var abogadoActual = await ObtenerAbogadoActualAsync();
            IQueryable<Caso> consulta = _context.caso
                .Include(c => c.abogado)
                .Include(c => c.cliente);

            if (abogadoActual != null)
            {
                consulta = consulta.Where(c => c.id_Abogado == abogadoActual.idAbogado);
            }

            var casos = await consulta.ToListAsync();
            return View(casos);
        }

        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;

            // 2. Evaluamos si el usuario actual es un Abogado
            if (User.IsInRole("Abogado") && idUsuarioClaim != null)
            {
                int idUsuario = int.Parse(idUsuarioClaim);

                // Buscamos el registro del abogado asociado a este usuario logueado
                var abogadoActual = await _context.abogados
                    .FirstOrDefaultAsync(a => a.id_Usuario == idUsuario);

                if (abogadoActual != null)
                {
                    // Regrilla 1: El SelectList de abogados solo tendrá una opción (él mismo)
                    ViewBag.Abogados = new SelectList(
                        new List<Abogado> { abogadoActual },
                        "idAbogado",
                        "nombreAbogado",
                        abogadoActual.idAbogado
                    );

                    // Regrilla 2: El SelectList de clientes mostrará SOLO los que él defiende
                    var clientesDelAbogado = await _context.cliente
                        .Where(c => c.idAbogado == abogadoActual.idAbogado && c.estadoCliente)
                        .ToListAsync();

                    ViewBag.Clientes = new SelectList(clientesDelAbogado, "idCliente", "nombreCliente");

                    // Bandera para que la vista oculte/estilice el select de abogados
                    ViewBag.EsAbogado = true;
                    return View();
                }
            }

            // 3. Si el rol es "Admin" (u otro), ve el comportamiento global original
            ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado");
            ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente");
            ViewBag.EsAbogado = false;

            return View();
        }

        // POST: Caso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Caso caso)
        {
            ModelState.Remove("abogado");
            ModelState.Remove("cliente");
            ModelState.Remove("expediente");
            ModelState.Remove("audiencia");
            ModelState.Remove("pago");

            if (ModelState.IsValid)
            {
                _context.caso.Add(caso);
                await _context.SaveChangesAsync();

                await Services.NotificationService.NotificarCasoEstadoAsync(_context, caso.id_Abogado, caso.tituloCaso, caso.estadoCaso);

                TempData["Success"] = "Caso registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // =========================================================================
            // SI EL MODELSTATE NO ES VÁLIDO: Volvemos a repoblar los combos según el rol
            // =========================================================================
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;

            if (User.IsInRole("Abogado") && idUsuarioClaim != null)
            {
                int idUsuario = int.Parse(idUsuarioClaim);
                var abogadoActual = await _context.abogados.FirstOrDefaultAsync(a => a.id_Usuario == idUsuario);

                if (abogadoActual != null)
                {
                    // Forzamos el ID correcto por si se manipuló el request en el cliente
                    caso.id_Abogado = abogadoActual.idAbogado;

                    ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", abogadoActual.idAbogado);
                    ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.idAbogado == abogadoActual.idAbogado && c.estadoCliente), "idCliente", "nombreCliente", caso.id_Cliente);
                    ViewBag.EsAbogado = true;
                    return View(caso);
                }
            }

            // Repoblación estándar para el Administrador en caso de error de validación
            ViewBag.Abogados = new SelectList(_context.abogados.Where(a => a.estadoAbogado), "idAbogado", "nombreAbogado", caso.id_Abogado);
            ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.estadoCliente), "idCliente", "nombreCliente", caso.id_Cliente);
            ViewBag.EsAbogado = false;

            return View(caso);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var caso = await _context.caso.FindAsync(id.Value);
            if (caso == null) return NotFound();
            if (!await PuedeVerCasoAsync(caso)) return Forbid();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", caso.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente.Where(c => c.idAbogado == abogadoActual.idAbogado), "idCliente", "nombreCliente", caso.id_Cliente);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", caso.id_Abogado);
                ViewBag.Clientes = new SelectList(_context.cliente, "idCliente", "nombreCliente", caso.id_Cliente);
            }
            return View(caso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Caso caso)
        {
            if (id != caso.idCaso) return NotFound();
            ModelState.Remove("abogado");
            ModelState.Remove("cliente");
            ModelState.Remove("expediente");
            ModelState.Remove("audiencia");
            ModelState.Remove("pago");
            if (ModelState.IsValid)
            {
                try
                {
                    var casoAnterior = await _context.caso.AsNoTracking().FirstOrDefaultAsync(c => c.idCaso == caso.idCaso);
                    _context.Update(caso);
                    await _context.SaveChangesAsync();

                    if (casoAnterior != null && casoAnterior.estadoCaso != caso.estadoCaso)
                    {
                        await Services.NotificationService.NotificarCasoEstadoAsync(_context, caso.id_Abogado, caso.tituloCaso, caso.estadoCaso);
                    }

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
            if (!await PuedeVerCasoAsync(caso)) return Forbid();
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
            if (!await PuedeVerCasoAsync(caso)) return Forbid();
            return View(caso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caso = await _context.caso.FindAsync(id);
            if (caso == null) return NotFound();
            if (!await PuedeVerCasoAsync(caso)) return Forbid();

            _context.caso.Remove(caso);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Caso eliminado.";
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

        private async Task<bool> PuedeVerCasoAsync(Caso caso)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            return abogadoActual == null || caso.id_Abogado == abogadoActual.idAbogado;
        }
    }
}