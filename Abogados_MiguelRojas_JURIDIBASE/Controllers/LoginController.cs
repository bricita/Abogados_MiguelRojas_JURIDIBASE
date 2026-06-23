using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Abogados_MiguelRojas_JURIDIBASE.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
// Nota: no se usa PasswordHasher si la contraseña se guarda en texto plano

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly AppDbContext _dbconext;
        public LoginController(AppDbContext dbcontext)
        {
            _dbconext = dbcontext;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            // CORREGIDO: Incluimos explícitamente la tabla relacionada 'rol'
            Usuario? usuario_encontrado = await _dbconext.usuario
                .Include(u => u.rol)
                .Where(u => u.nombreUsuario == model.NombreUser && u.passwordUsuario == model.Password)
                .FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "Usuario o Contraseña incorrectos";
                return View();
            }

            // OBTENER NOMBRE DEL ROL DE MANERA SEGURA
            // Si por alguna razón un usuario en la BD no tiene RolId válido, le asigna "SinRol" para evitar crasheos
            string nombreRol = usuario_encontrado.rol?.nombre ?? "SinRol";

            var claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Name, usuario_encontrado.nombreUsuario),
        new Claim(ClaimTypes.Role, nombreRol), // CORREGIDO
        new Claim("IdUsuario", usuario_encontrado.idUsuario.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var propiedades = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                propiedades
            );

            HttpContext.Session.SetString("Usuario", usuario_encontrado.nombreUsuario);
            HttpContext.Session.SetInt32("IdUsuario", usuario_encontrado.idUsuario);

            // CORREGIDO: Evaluamos con la variable segura 'nombreRol'
            switch (nombreRol)
            {
                case "Administrador":
                    return RedirectToAction("Admin", "Usuario"); // Asegúrate de que esta acción exista
                case "Abogado":
                    return RedirectToAction("Inicio", "Home");
                case "Asistente":
                    return RedirectToAction("Index", "Home"); // Asegúrate de que este controlador exista
                default:
                    return RedirectToAction("Login", "Login");
            }
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
        public IActionResult AccesoDenegado()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
    }
}
