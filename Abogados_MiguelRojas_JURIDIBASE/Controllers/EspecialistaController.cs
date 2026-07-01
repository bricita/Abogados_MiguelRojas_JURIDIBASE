using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class EspecialistaController : Controller
    {
        private readonly AppDbContext _context;

        public EspecialistaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            List<Especialista> lista = await _context.especialista.ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(Especialista especialista)
        {
            if (ModelState.IsValid)
            {
                await _context.especialista.AddAsync(especialista);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Especialista registrado correctamente.";
                return RedirectToAction(nameof(Listar));
            }

            return View(especialista);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Especialista especialista = await _context.especialista.FirstAsync(e => e.idEspecialista == id);
            return View(especialista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Especialista especialista)
        {
            if (ModelState.IsValid)
            {
                _context.especialista.Update(especialista);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Especialista actualizado correctamente.";
                return RedirectToAction(nameof(Listar));
            }

            return View(especialista);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Especialista especialista = await _context.especialista.FirstAsync(e => e.idEspecialista == id);
            _context.especialista.Remove(especialista);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
    }
}