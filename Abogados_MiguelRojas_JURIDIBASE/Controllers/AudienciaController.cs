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
            var abogadoActual = await ObtenerAbogadoActualAsync();
            IQueryable<Audiencia> consulta = _context.audiencia
                .Include(a => a.abogado)
                .Include(a => a.caso);

            if (abogadoActual != null)
            {
                consulta = consulta.Where(a => a.id_Abogado == abogadoActual.idAbogado);
            }

            var audiencias = await consulta.ToListAsync();
            return View(audiencias);
        }

        public async Task<IActionResult> Create()
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado");
                ViewBag.Casos = new SelectList(_context.caso.Where(c => c.id_Abogado == abogadoActual.idAbogado), "idCaso", "tituloCaso");
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado");
                ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Audiencia audiencia)
        {
            ModelState.Remove("abogado");
            ModelState.Remove("caso");

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                audiencia.id_Abogado = abogadoActual.idAbogado;
            }

            // !! VALIDACIÓN DE CRUCE DE HORARIOS !!
            if (await ExisteCruceDeAudienciaAsync(audiencia))
            {
                ModelState.AddModelError("horaAudiencia", "El abogado ya tiene una audiencia programada para esta misma fecha y hora.");
            }

            if (ModelState.IsValid)
            {
                _context.audiencia.Add(audiencia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Audiencia registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Si falla, recargar los combos (Tu código original)
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
                ViewBag.Casos = new SelectList(_context.caso.Where(c => c.id_Abogado == abogadoActual.idAbogado), "idCaso", "tituloCaso", audiencia.id_Caso);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
                ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso", audiencia.id_Caso);
            }

            return View(audiencia);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var audiencia = await _context.audiencia.FindAsync(id.Value);
            if (audiencia == null) return NotFound();
            if (!await PuedeVerAudienciaAsync(audiencia)) return Forbid();

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
                ViewBag.Casos = new SelectList(_context.caso.Where(c => c.id_Abogado == abogadoActual.idAbogado), "idCaso", "tituloCaso", audiencia.id_Caso);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
                ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso", audiencia.id_Caso);
            }
            return View(audiencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Audiencia audiencia)
        {
            if (id != audiencia.idAudiencia) return NotFound();

            ModelState.Remove("abogado");
            ModelState.Remove("caso");

            var abogadoActual = await ObtenerAbogadoActualAsync();
            if (abogadoActual != null)
            {
                var audienciaDb = await _context.audiencia.AsNoTracking().FirstOrDefaultAsync(a => a.idAudiencia == id);
                if (audienciaDb == null) return NotFound();
                if (audienciaDb.id_Abogado != abogadoActual.idAbogado) return Forbid();
                audiencia.id_Abogado = abogadoActual.idAbogado;
            }

            // !! VALIDACIÓN DE CRUCE DE HORARIOS !!
            if (await ExisteCruceDeAudienciaAsync(audiencia))
            {
                ModelState.AddModelError("horaAudiencia", "El abogado ya tiene una audiencia programada para esta misma fecha y hora.");
            }

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

            // Si falla, recargar los combos (Tu código original)
            if (abogadoActual != null)
            {
                ViewBag.Abogados = new SelectList(new List<Abogado> { abogadoActual }, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
                ViewBag.Casos = new SelectList(_context.caso.Where(c => c.id_Abogado == abogadoActual.idAbogado), "idCaso", "tituloCaso", audiencia.id_Caso);
            }
            else
            {
                ViewBag.Abogados = new SelectList(_context.abogados, "idAbogado", "nombreAbogado", audiencia.id_Abogado);
                ViewBag.Casos = new SelectList(_context.caso, "idCaso", "tituloCaso", audiencia.id_Caso);
            }
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
            if (!await PuedeVerAudienciaAsync(audiencia)) return Forbid();
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
            if (!await PuedeVerAudienciaAsync(audiencia)) return Forbid();
            return View(audiencia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var audiencia = await _context.audiencia.FindAsync(id);
            if (audiencia == null) return NotFound();
            if (!await PuedeVerAudienciaAsync(audiencia)) return Forbid();

            _context.audiencia.Remove(audiencia);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Audiencia eliminada.";
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

        private async Task<bool> PuedeVerAudienciaAsync(Audiencia audiencia)
        {
            var abogadoActual = await ObtenerAbogadoActualAsync();
            return abogadoActual == null || audiencia.id_Abogado == abogadoActual.idAbogado;
        }
        private async Task<bool> ExisteCruceDeAudienciaAsync(Audiencia audiencia)
        {
            return await _context.audiencia
                .AnyAsync(a => a.id_Abogado == audiencia.id_Abogado
                            && a.fechaAudiencia == audiencia.fechaAudiencia
                            && a.horaAudiencia.Hour == audiencia.horaAudiencia.Hour
                            && a.horaAudiencia.Minute == audiencia.horaAudiencia.Minute
                            && a.idAudiencia != audiencia.idAudiencia); // Excluye la misma audiencia al editar
        }

    }
}