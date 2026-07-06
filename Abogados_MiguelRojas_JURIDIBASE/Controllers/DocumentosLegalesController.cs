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
        private readonly IWebHostEnvironment _env;

        public DocumentosLegalesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<IActionResult> Create(DocumentosLegales doc, IFormFile archivo)
        {
            ModelState.Remove("expediente");
            ModelState.Remove("rutaDocumento");

            if (archivo != null && archivo.Length > 0)
            {
                if (Path.GetExtension(archivo.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("archivo", "Solo se permiten archivos PDF.");
                }
                else
                {
                    string uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "pdfs");
                    Directory.CreateDirectory(uploadsDir);

                    string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(archivo.FileName)}";
                    string filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    doc.rutaDocumento = $"/uploads/pdfs/{fileName}";
                }
            }
            else
            {
                ModelState.AddModelError("archivo", "Debe seleccionar un archivo PDF.");
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

        public IActionResult VerPdf(int? id)
        {
            if (id == null) return NotFound();
            var doc = _context.documento.Find(id.Value);
            if (doc == null) return NotFound();

            string filePath = Path.Combine(_env.WebRootPath, doc.rutaDocumento.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
                return NotFound("El archivo PDF no se encuentra en el servidor.");

            return PhysicalFile(filePath, "application/pdf");
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
                if (!string.IsNullOrEmpty(doc.rutaDocumento))
                {
                    string filePath = Path.Combine(_env.WebRootPath, doc.rutaDocumento.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.documento.Remove(doc);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Documento eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
