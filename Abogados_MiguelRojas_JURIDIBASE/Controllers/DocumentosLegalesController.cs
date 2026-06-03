using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    public class DocumentosLegalesController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentosLegalesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var documentos = await _context.documento.Include(d => d.expediente).ToListAsync();
            return View(documentos);
        }

        public IActionResult Create()
        {
            ViewBag.Expedientes = new SelectList(_context.expediente.Where(e => e.estadoExpediente), "idExpediente", "tituloExpediente");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentosLegales doc)
        {
            if (ModelState.IsValid)
            {
                _context.documento.Add(doc);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Documento agregado.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Expedientes = new SelectList(_context.expediente.Where(e => e.estadoExpediente), "idExpediente", "tituloExpediente", doc.id_Expediente);
            return View(doc);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var doc = await _context.documento.Include(d => d.expediente).FirstOrDefaultAsync(d => d.idDocumentoLegal == id.Value);
            if (doc == null) return NotFound();
            return View(doc);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var doc = await _context.documento.Include(d => d.expediente).FirstOrDefaultAsync(d => d.idDocumentoLegal == id.Value);
            if (doc == null) return NotFound();
            return View(doc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doc = await _context.documento.FindAsync(id);
            if (doc != null)
            {
                _context.documento.Remove(doc);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Documento eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
