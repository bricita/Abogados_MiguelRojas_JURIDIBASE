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
            // Traemos las notificaciones e incluimos al usuario asociado para ver a quién le pertenece
            List<Notificacion> lista = await _context.notificacion.Include(n => n.usuario).ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(Notificacion notificacion)
        {
            await _context.notificacion.AddAsync(notificacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Notificacion notificacion = await _context.notificacion.FirstAsync(n => n.idNotificacion == id);
            return View(notificacion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Notificacion notificacion)
        {
            _context.notificacion.Update(notificacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Notificacion notificacion = await _context.notificacion.FirstAsync(n => n.idNotificacion == id);
            _context.notificacion.Remove(notificacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
    }
}