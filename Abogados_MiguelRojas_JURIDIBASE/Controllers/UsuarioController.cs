using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Abogados_MiguelRojas_JURIDIBASE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            // LÍNEA CLAVE: Trae los roles de la BD y los empaqueta para el HTML
            ViewBag.Roles = new SelectList(await _context.roles.ToListAsync(), "idRol", "nombre");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(UsuarioVM model)
        {
            if (model.password != model.RepPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";

                // LÍNEA CLAVE: Si las contraseñas fallan, debes volver a cargar los roles 
                // antes de hacer el "return View(model)", de lo contrario se vaciará el combo.
                ViewBag.Roles = new SelectList(await _context.roles.ToListAsync(), "idRol", "nombre", model.RolId);

                return View(model);
            }

            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario()
                {
                    nombreUsuario = model.nombreUsuario,
                    passwordUsuario = model.password,
                    RolId = model.RolId // Tu nueva FK numérica
                };

                await _context.usuario.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Login");
            }

            // LÍNEA CLAVE: Si hay algún otro error de validación (ej: nombre vacío), rellenamos el combo
            ViewBag.Roles = new SelectList(await _context.roles.ToListAsync(), "idRol", "nombre", model.RolId);
            return View(model);
        }
        // GET: Usuario/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            // Buscamos al usuario incluyendo su Rol de forma preventiva
            var usuario = await _context.usuario.Include(u => u.rol).FirstOrDefaultAsync(u => u.idUsuario == id.Value);
            if (usuario == null) return NotFound();

            // Traemos los roles para el combo select
            var listaRoles = await _context.roles.ToListAsync();
            ViewBag.Roles = new SelectList(listaRoles, "idRol", "nombre", usuario.RolId);

            return View(usuario);
        }

        // 2. POST: Usuario/Editar/5
        // Se ejecuta cuando el usuario presiona el botón "Guardar"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Usuario usuario)
        {
            if (id != usuario.idUsuario) return NotFound();

            ModelState.Remove("rol");
            ModelState.Remove("notificacion");
            ModelState.Remove("abogado");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Usuario actualizado correctamente.";
                    return RedirectToAction(nameof(Listar)); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.usuario.Any(e => e.idUsuario == usuario.idUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var listaRolesAlterna = await _context.roles.ToListAsync();
            ViewBag.Roles = new SelectList(listaRolesAlterna, "idRol", "nombre", usuario.RolId);

            return View(usuario);
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
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Usuario()
        {
            return View();
        }
        public IActionResult Asistente()
        {
            return View();
        }
        
    }
}
