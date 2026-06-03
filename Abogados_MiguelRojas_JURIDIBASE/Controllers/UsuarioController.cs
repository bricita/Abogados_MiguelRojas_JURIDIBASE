using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Abogados_MiguelRojas_JURIDIBASE.ViewModels;
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
        public IActionResult Nuevo()
        {
            
            return View(new Abogados_MiguelRojas_JURIDIBASE.ViewModels.UsuarioVM());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(UsuarioVM model)
        {
            
            if (model.password != model.RepPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";

            
                return View(model);
            }

            
            Usuario usuario = new Usuario()
            {
                nombreUsuario = model.nombreUsuario,
                passwordUsuario = model.password,
                rolUsuario = model.rolUsuario
            };

            
            await _context.usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Login");
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
