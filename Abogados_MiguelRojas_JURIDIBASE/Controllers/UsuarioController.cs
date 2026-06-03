using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Abogados_MiguelRojas_JURIDIBASE.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            List<Usuario> lista = await _context.usuario.ToListAsync(); 
            return View(lista);
        }
        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            // Cargamos los roles desde la base de datos para el select de la vista
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(Usuario usuario, string RepPassword)
        {
            // 1. Validación de contraseñas en el controlador
            if (usuario.passwordUsuario != RepPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";

                // Retornamos el objeto 'usuario' para no perder lo que ya escribió el usuario
                return View(usuario);
            }

            // 2. Tu código principal exacto de inserción
            await _context.usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Listar));
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Usuario usuario = await _context.usuario.FirstAsync(e => e.idUsuario == id);
            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Usuario user)
        {
            _context.usuario.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Usuario usuario = await _context.usuario.FirstAsync(e => e.idUsuario == id);
            _context.usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));

        }
        public IActionResult Eliminar()
        {
            return View();
        }
        
    }
}
