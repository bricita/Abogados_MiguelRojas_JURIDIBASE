using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class ExpedienteController : Controller
    {
        private readonly AppDbContext _context;

        public ExpedienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            // Include(e => e.caso) para traer también la información de la relación si la necesitas en la vista
            List<Expediente> lista = await _context.expediente.Include(e => e.caso).ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            // Aquí podrías enviar ViewBag con la lista de Casos para llenar un ComboBox en tu vista
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(Expediente expediente)
        {
            await _context.expediente.AddAsync(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Expediente expediente = await _context.expediente.FirstAsync(e => e.idExpediente == id);
            return View(expediente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Expediente expediente)
        {
            _context.expediente.Update(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Expediente expediente = await _context.expediente.FirstAsync(e => e.idExpediente == id);
            _context.expediente.Remove(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }
    }
}