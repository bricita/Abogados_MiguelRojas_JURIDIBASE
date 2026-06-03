using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class AbogadoController : Controller
    {
        private readonly AppDbContext _context;
        public AbogadoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            List<Abogado> lista = await _context.abogados.ToListAsync();
            return View(lista);
        }
        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo(Abogado abogado)
        {
            await _context.abogados.AddAsync(abogado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Abogado abogado = await _context.abogados.FirstAsync(a => a.idAbogado == id);
            return View(abogado);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Abogado abogado)
        {
            _context.abogados.Update(abogado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Abogado abogado = await _context.abogados.FirstAsync(a => a.idAbogado == id);
            _context.abogados.Remove(abogado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
    }
}
