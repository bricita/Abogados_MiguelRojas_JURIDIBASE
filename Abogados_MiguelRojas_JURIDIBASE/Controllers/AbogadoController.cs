using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Controllers
{
    [Authorize]
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
            var listaUsuarios = await _context.usuario.ToListAsync();

            // Creamos el SelectList (Valor que se guarda, Texto que se muestra)
            ViewBag.Usuarios = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                listaUsuarios,
                "idUsuario",       // <-- Asegúrate de que coincida con la PK de tu clase Usuario
                "nombreUsuario"    // <-- El campo con el nombre que verá el administrador
            );
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(Abogado abogado)
        {
            ModelState.Remove("usuario");
            ModelState.Remove("abogadoArea");
            ModelState.Remove("abogadoServicio");
            ModelState.Remove("cita");
            ModelState.Remove("audiencia");
            ModelState.Remove("caso");
            ModelState.Remove("cliente");
            ModelState.Remove("pago");

            if (ModelState.IsValid)
            {
                await _context.abogados.AddAsync(abogado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Abogado registrado correctamente.";
                return RedirectToAction(nameof(Listar));
            }

            var listaUsuarios = await _context.usuario.ToListAsync();
            ViewBag.Usuarios = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                listaUsuarios,
                "idUsuario",
                "nombreUsuario",
                abogado.id_Usuario
            );
            return View(abogado);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Abogado abogado = await _context.abogados.FirstAsync(a => a.idAbogado == id);
            return View(abogado);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Abogado abogado)
        {
            ModelState.Remove("usuario");
            ModelState.Remove("abogadoArea");
            ModelState.Remove("abogadoServicio");
            ModelState.Remove("cita");
            ModelState.Remove("audiencia");
            ModelState.Remove("caso");
            ModelState.Remove("cliente");
            ModelState.Remove("pago");

            if (ModelState.IsValid)
            {
                _context.abogados.Update(abogado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Abogado actualizado correctamente.";
                return RedirectToAction(nameof(Listar));
            }

            return View(abogado);
        }
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            Abogado abogado = await _context.abogados.FirstAsync(a => a.idAbogado == id);
            _context.abogados.Remove(abogado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public async Task<IActionResult> Perfil(int id)
        {
            var abogado = await _context.abogados
                .Include(a => a.usuario)
                .Include(a => a.abogadoArea).ThenInclude(aa => aa.areaDerecho)
                .Include(a => a.abogadoServicio).ThenInclude(se => se.servicioLegal)
                .FirstOrDefaultAsync(a => a.idAbogado == id);
            if (abogado == null) return NotFound();

            var areasAsignadas = abogado.abogadoArea.Select(aa => aa.id_AreaDerecho).ToHashSet();
            ViewBag.AreasDisponibles = new SelectList(
                await _context.areasDerecho.Where(ad => !areasAsignadas.Contains(ad.idAreaDerecho) && ad.estadoAreaDerecho).ToListAsync(),
                "idAreaDerecho", "nombreAreaDerecho");

            var serviciosAsignados = abogado.abogadoServicio.Select(s => s.id_ServicioLegal).ToHashSet();
            ViewBag.ServiciosDisponibles = new SelectList(
                await _context.servicio.Where(s => !serviciosAsignados.Contains(s.idServicio)).ToListAsync(),
                "idServicio", "nombreServicio");

            return View(abogado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarArea(int idAbogado, int idArea)
        {
            var existe = await _context.abogadoArea.AnyAsync(aa => aa.id_Abogado == idAbogado && aa.id_AreaDerecho == idArea);
            if (!existe)
            {
                _context.abogadoArea.Add(new AbogadoArea { id_Abogado = idAbogado, id_AreaDerecho = idArea });
                await _context.SaveChangesAsync();
                TempData["Success"] = "Área asignada.";
            }
            return RedirectToAction(nameof(Perfil), new { id = idAbogado });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarArea(int idAbogado, int idArea)
        {
            var aa = await _context.abogadoArea.FirstOrDefaultAsync(x => x.id_Abogado == idAbogado && x.id_AreaDerecho == idArea);
            if (aa != null)
            {
                _context.abogadoArea.Remove(aa);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Área removida.";
            }
            return RedirectToAction(nameof(Perfil), new { id = idAbogado });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarServicio(int idAbogado, int idServicio)
        {
            var existe = await _context.AbogadoServicio.AnyAsync(s => s.id_Abogado == idAbogado && s.id_ServicioLegal == idServicio);
            if (!existe)
            {
                _context.AbogadoServicio.Add(new AbogadoServicio { id_Abogado = idAbogado, id_ServicioLegal = idServicio });
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio asignado.";
            }
            return RedirectToAction(nameof(Perfil), new { id = idAbogado });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarServicio(int idAbogado, int idServicio)
        {
            var s = await _context.AbogadoServicio.FirstOrDefaultAsync(x => x.id_Abogado == idAbogado && x.id_ServicioLegal == idServicio);
            if (s != null)
            {
                _context.AbogadoServicio.Remove(s);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio removido.";
            }
            return RedirectToAction(nameof(Perfil), new { id = idAbogado });
        }
    }
}
