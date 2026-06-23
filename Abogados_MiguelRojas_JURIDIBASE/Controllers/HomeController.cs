using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbconext;
        public HomeController(AppDbContext dbcontext)
        {
            _dbconext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Inicio()
        {
            string displayName = "ABOGADO";
            var idClaim = User?.FindFirst("IdUsuario")?.Value;
            if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var idUsuario))
            {
                var abogado = await _dbconext.abogados.FirstOrDefaultAsync(a => a.id_Usuario == idUsuario);
                if (abogado != null)
                {
                    displayName = ($"{abogado.nombreAbogado} {abogado.apellidoAbogado}").ToUpperInvariant();
                }
            }
            ViewBag.DisplayName = displayName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
