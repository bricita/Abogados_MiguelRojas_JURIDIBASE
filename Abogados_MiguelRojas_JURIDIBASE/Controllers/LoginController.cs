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
        //public IActionResult Registro()
        //{
        //    //ViewBag.Abogado = _dbconext.abogados.ToList();
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Registro(AbogadoVM model) 
        //{
        //    if(model.Password != model.RepPassword)
        //    {
        //        ViewData["Mensaje"] = "Las contraseñas son diferentes";
        //        return View();
        //    }
        //    // Crear usuario (contraseña en texto plano según petición)
        //    var usuario = new Usuario
        //    {
        //        nombreUsuario = model.nombreUsuario ?? model.Correo,
        //        passwordUsuario = model.Password
        //    };

        //    // Crear abogado y enlazar con usuario
        //    Abogado abogado = new Abogado()
        //    {
        //        nombreAbogado = model.Nombre,
        //        apellidoAbogado = model.Apellido,
        //        telefonoAbogado = model.Telefono,
        //        dniAbogado = model.DNI,
        //        correoAbogado = model.Correo,
        //        especialidadAbogado = model.Especialidad,
        //        estadoAbogado = true,
        //        usuario = usuario
        //    };

        //    // Agregar ambas entidades y guardar una vez
        //    await _dbconext.usuario.AddAsync(usuario);
        //    await _dbconext.abogados.AddAsync(abogado);
        //    await _dbconext.SaveChangesAsync();
        //    if (abogado.idAbogado != 0) return RedirectToAction("Login", "Login");
        //    ViewData["Mensaje"] = "El usuario no se puede crearse";
        //    return View();
        //}
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            Usuario? usuario_encontrado = await _dbconext.usuario.Where(u => u.nombreUsuario == model.NombreUser && u.passwordUsuario == model.Password).FirstOrDefaultAsync();
            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron usuarios";
                return View();
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.nombreUsuario),
                new Claim(ClaimTypes.Role, usuario_encontrado.rolUsuario),
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

            switch (usuario_encontrado.rolUsuario)
            {
                case "Admin":
                    return RedirectToAction("Admin", "Usuario");
                case "Abogado":
                    return RedirectToAction("Index", "Home");
                case "Asistente":
                    return RedirectToAction("Index", "Asistente");
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
