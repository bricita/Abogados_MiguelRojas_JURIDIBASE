using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class ExpedienteController : Controller
    {
        private readonly AppDbContext _context;

        public ExpedienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            IQueryable<Expediente> consulta = _context.expediente.Include(e => e.caso);

            if (abogadoActual != null)
            {
                consulta = consulta.Where(e => e.caso.id_Abogado == abogadoActual.idAbogado);
            }

            List<Expediente> lista = await consulta.ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            await CargarCasosAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(Expediente expediente)
        {
            ModelState.Remove("caso");
            ModelState.Remove("documentosLegales");

            if (ModelState.IsValid)
            {
                if (!await CasoPerteneceAlAbogadoAsync(expediente.id_Caso))
                {
                    ModelState.AddModelError("id_Caso", "El caso seleccionado no pertenece al abogado actual.");
                }
                else if (await CasoYaTieneExpedienteAsync(expediente.id_Caso, null))
                {
                    ModelState.AddModelError("id_Caso", "El caso seleccionado ya tiene un expediente asignado.");
                }
                else
                {
                    _context.expediente.Add(expediente);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Expediente registrado correctamente.";
                    return RedirectToAction(nameof(Listar));
                }
            }

            await CargarCasosAsync(expediente.id_Caso);
            return View(expediente);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Expediente expediente = await _context.expediente.Include(e => e.caso).FirstAsync(e => e.idExpediente == id);
            if (!await PuedeVerExpedienteAsync(expediente)) return Forbid();
            await CargarCasosAsync(expediente.id_Caso);
            return View(expediente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Expediente expediente)
        {
            if (!await PuedeVerExpedienteAsync(expediente)) return Forbid();
            ModelState.Remove("caso");
            ModelState.Remove("documentosLegales");

            if (ModelState.IsValid)
            {
                if (!await CasoPerteneceAlAbogadoAsync(expediente.id_Caso))
                {
                    ModelState.AddModelError("id_Caso", "El caso seleccionado no pertenece al abogado actual.");
                }
                else if (await CasoYaTieneExpedienteAsync(expediente.id_Caso, expediente.idExpediente))
                {
                    ModelState.AddModelError("id_Caso", "El caso seleccionado ya tiene un expediente asignado.");
                }
                else
                {
                    _context.expediente.Update(expediente);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Expediente actualizado correctamente.";
                    return RedirectToAction(nameof(Listar));
                }
            }

            await CargarCasosAsync(expediente.id_Caso);
            return View(expediente);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Expediente expediente = await _context.expediente.Include(e => e.caso).FirstAsync(e => e.idExpediente == id);
            if (!await PuedeVerExpedienteAsync(expediente)) return Forbid();
            _context.expediente.Remove(expediente);
            await _context.SaveChangesAsync();
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

        private async Task<bool> PuedeVerExpedienteAsync(Expediente expediente)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual == null) return true;

            var caso = await _context.caso.AsNoTracking().FirstOrDefaultAsync(c => c.idCaso == expediente.id_Caso);
            return caso != null && caso.id_Abogado == abogadoActual.idAbogado;
        }

        private async Task CargarCasosAsync(int? idCasoSeleccionado = null)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();

            var casosConExpediente = _context.expediente.Select(e => e.id_Caso);

            var casosQuery = _context.caso.Where(c => c.estadoCaso && !casosConExpediente.Contains(c.idCaso));
            if (abogadoActual != null)
            {
                casosQuery = casosQuery.Where(c => c.id_Abogado == abogadoActual.idAbogado);
            }

            var casos = await casosQuery.OrderBy(c => c.tituloCaso).ToListAsync();

            if (idCasoSeleccionado.HasValue && !casos.Any(c => c.idCaso == idCasoSeleccionado.Value))
            {
                var casoActual = await _context.caso.FindAsync(idCasoSeleccionado.Value);
                if (casoActual != null)
                    casos.Add(casoActual);
            }

            ViewBag.Casos = new SelectList(casos, "idCaso", "tituloCaso", idCasoSeleccionado);
        }

        private async Task<bool> CasoPerteneceAlAbogadoAsync(int idCaso)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual == null) return true;

            var caso = await _context.caso.AsNoTracking().FirstOrDefaultAsync(c => c.idCaso == idCaso);
            return caso != null && caso.id_Abogado == abogadoActual.idAbogado;
        }

        private async Task<bool> CasoYaTieneExpedienteAsync(int idCaso, int? idExpedienteIgnorar)
        {
            var query = _context.expediente.Where(e => e.id_Caso == idCaso);
            if (idExpedienteIgnorar.HasValue)
                query = query.Where(e => e.idExpediente != idExpedienteIgnorar.Value);
            return await query.AnyAsync();
        }
    }
}
