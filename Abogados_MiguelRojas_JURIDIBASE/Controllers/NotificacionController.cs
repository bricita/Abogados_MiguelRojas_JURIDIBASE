using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly AppDbContext _context;

        public NotificacionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            IQueryable<Notificacion> consulta = _context.notificacion.Include(n => n.usuario);
            var idUsuario = ObtenerIdUsuarioActual();

            if (!User.IsInRole("Administrador") && idUsuario != null)
            {
                consulta = consulta.Where(n => n.id_Usuario == idUsuario.Value);
            }

            List<Notificacion> lista = await consulta.ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(Notificacion notificacion)
        {
            ModelState.Remove("usuario");

            if (ModelState.IsValid)
            {
                await _context.notificacion.AddAsync(notificacion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Notificación creada correctamente.";
                return RedirectToAction(nameof(Listar));
            }

            return View(notificacion);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Notificacion notificacion = await _context.notificacion.FirstAsync(n => n.idNotificacion == id);
            if (!await PuedeVerNotificacionAsync(notificacion)) return Forbid();
            return View(notificacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Notificacion notificacion)
        {
            if (!await PuedeVerNotificacionAsync(notificacion)) return Forbid();
            ModelState.Remove("usuario");

            if (ModelState.IsValid)
            {
                _context.notificacion.Update(notificacion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Notificación actualizada correctamente.";
                return RedirectToAction(nameof(Listar));
            }

            return View(notificacion);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Notificacion notificacion = await _context.notificacion.FirstAsync(n => n.idNotificacion == id);
            if (!await PuedeVerNotificacionAsync(notificacion)) return Forbid();
            _context.notificacion.Remove(notificacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }

        private int? ObtenerIdUsuarioActual()
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            var idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");
            return int.TryParse(idUsuarioClaim, out var claimId) ? claimId : idUsuarioSession;
        }

        private async Task<bool> PuedeVerNotificacionAsync(Notificacion notificacion)
        {
            if (User.IsInRole("Administrador")) return true;
            var idUsuario = ObtenerIdUsuarioActual();
            return idUsuario != null && notificacion.id_Usuario == idUsuario.Value;
        }
    }
}
