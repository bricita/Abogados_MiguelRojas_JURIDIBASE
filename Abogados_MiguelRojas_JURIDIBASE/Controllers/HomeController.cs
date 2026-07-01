using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;
using Abogados_MiguelRojas_JURIDIBASE.ViewModels;
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
            var idClaim = User?.FindFirst("IdUsuario")?.Value;
            string displayName = User?.Identity?.Name ?? "USUARIO";

            var vm = new DashboardViewModel();
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var idUsuario))
            {
                var usuario = await _dbconext.usuario
                    .Include(u => u.rol)
                    .FirstOrDefaultAsync(u => u.idUsuario == idUsuario);

                var abogado = usuario != null
                    ? await _dbconext.abogados.FirstOrDefaultAsync(a => a.id_Usuario == idUsuario)
                    : null;

                if (abogado != null)
                {
                    displayName = ($"{abogado.nombreAbogado} {abogado.apellidoAbogado}").ToUpperInvariant();
                }

                ViewBag.UserRole = usuario?.rol?.nombre;

                var esAdmin = User.IsInRole("Administrador");
                var esAbogado = User.IsInRole("Abogado");

                if (esAdmin)
                {
                    vm.TotalAbogadosActivos = await _dbconext.abogados.CountAsync(a => a.estadoAbogado);
                    vm.TotalUsuarios = await _dbconext.usuario.CountAsync();
                    vm.TotalCasosActivos = await _dbconext.caso.CountAsync(c => c.estadoCaso);
                    vm.TotalClientesActivos = await _dbconext.cliente.CountAsync(c => c.estadoCliente);
                    vm.TotalExpedientesActivos = await _dbconext.expediente.CountAsync(e => e.estadoExpediente);
                    vm.IngresosTotales = await _dbconext.pago.SumAsync(p => (decimal?)p.monto) ?? 0m;
                    vm.NotificacionesNoLeidas = await _dbconext.notificacion.CountAsync(n => !n.leido);

                    vm.AbogadosRecientes = await _dbconext.abogados
                        .OrderByDescending(a => a.idAbogado)
                        .Take(5)
                        .ToListAsync();
                    vm.UltimosCasos = await _dbconext.caso
                        .Include(c => c.cliente)
                        .Include(c => c.abogado)
                        .OrderByDescending(c => c.idCaso)
                        .Take(5)
                        .ToListAsync();
                    vm.ProximasAudiencias = await _dbconext.audiencia
                        .Include(a => a.caso)
                        .Where(a => a.fechaAudiencia >= today)
                        .OrderBy(a => a.fechaAudiencia)
                        .ThenBy(a => a.horaAudiencia)
                        .Take(5)
                        .ToListAsync();
                    vm.UltimosPagos = await _dbconext.pago
                        .Include(p => p.cliente)
                        .Include(p => p.caso)
                        .OrderByDescending(p => p.fechaPago)
                        .Take(5)
                        .ToListAsync();
                }
                else if (esAbogado && abogado != null)
                {
                    var idAbog = abogado.idAbogado;

                    vm.TotalCasosActivos = await _dbconext.caso.CountAsync(c => c.id_Abogado == idAbog && c.estadoCaso);
                    vm.TotalClientesActivos = await _dbconext.cliente.CountAsync(c => c.idAbogado == idAbog && c.estadoCliente);
                    vm.TotalAudienciasHoy = await _dbconext.audiencia.CountAsync(a => a.id_Abogado == idAbog && a.fechaAudiencia == today);
                    vm.NotificacionesNoLeidas = await _dbconext.notificacion.CountAsync(n => n.id_Usuario == idUsuario && !n.leido);
                    vm.TotalCitasPendientes = await _dbconext.cita.CountAsync(c => c.id_Abogado == idAbog && c.fechaHoraCita >= today && !c.estadoCita);

                    vm.ProximasAudiencias = await _dbconext.audiencia
                        .Include(a => a.caso)
                        .Where(a => a.id_Abogado == idAbog && a.fechaAudiencia >= today)
                        .OrderBy(a => a.fechaAudiencia)
                        .ThenBy(a => a.horaAudiencia)
                        .Take(5)
                        .ToListAsync();
                    vm.ProximasCitas = await _dbconext.cita
                        .Include(c => c.cliente)
                        .Where(c => c.id_Abogado == idAbog && c.fechaHoraCita >= today)
                        .OrderBy(c => c.fechaHoraCita)
                        .Take(5)
                        .ToListAsync();
                    vm.UltimosCasos = await _dbconext.caso
                        .Include(c => c.cliente)
                        .Where(c => c.id_Abogado == idAbog)
                        .OrderByDescending(c => c.idCaso)
                        .Take(5)
                        .ToListAsync();
                    vm.UltimosPagos = await _dbconext.pago
                        .Include(p => p.cliente)
                        .Include(p => p.caso)
                        .Where(p => p.idAbogado == idAbog)
                        .OrderByDescending(p => p.fechaPago)
                        .Take(5)
                        .ToListAsync();
                    vm.UltimasNotificaciones = await _dbconext.notificacion
                        .Where(n => n.id_Usuario == idUsuario)
                        .OrderByDescending(n => n.fechaNotificacion)
                        .Take(5)
                        .ToListAsync();
                }
            }

            ViewBag.DisplayName = displayName;
            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> Buscar(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return View(new List<object>());

            var idClaim = User?.FindFirst("IdUsuario")?.Value;
            int? idAbogado = null;
            if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var idUsuario))
            {
                var abogado = await _dbconext.abogados.FirstOrDefaultAsync(a => a.id_Usuario == idUsuario);
                idAbogado = abogado?.idAbogado;
            }

            var esAdmin = User.IsInRole("Administrador");

            q = q.ToLower();

            var casos = await (esAdmin
                ? _dbconext.caso.Include(c => c.cliente).Where(c => c.tituloCaso.ToLower().Contains(q) || c.descripcionCaso.ToLower().Contains(q))
                : _dbconext.caso.Include(c => c.cliente).Where(c => c.id_Abogado == idAbogado && (c.tituloCaso.ToLower().Contains(q) || c.descripcionCaso.ToLower().Contains(q))))
                .Take(5).ToListAsync();

            var clientes = await (esAdmin
                ? _dbconext.cliente.Where(c => c.nombreCliente.ToLower().Contains(q) || c.dniCliente.Contains(q) || c.correoCliente.ToLower().Contains(q))
                : _dbconext.cliente.Where(c => c.idAbogado == idAbogado && (c.nombreCliente.ToLower().Contains(q) || c.dniCliente.Contains(q) || c.correoCliente.ToLower().Contains(q))))
                .Take(5).ToListAsync();

            var expedientes = await (esAdmin
                ? _dbconext.expediente.Include(e => e.caso).Where(e => e.tituloExpediente.ToLower().Contains(q) || e.resumenExpediente.ToLower().Contains(q))
                : _dbconext.expediente.Include(e => e.caso).Where(e => e.caso.id_Abogado == idAbogado && (e.tituloExpediente.ToLower().Contains(q) || e.resumenExpediente.ToLower().Contains(q))))
                .Take(5).ToListAsync();

            ViewBag.Casos = casos;
            ViewBag.Clientes = clientes;
            ViewBag.Expedientes = expedientes;
            ViewBag.Query = q;
            return View();
        }

        [Authorize]
        public IActionResult Calendario()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ObtenerEventos()
        {
            var idClaim = User?.FindFirst("IdUsuario")?.Value;
            int? idAbogado = null;
            if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var idUsuario))
            {
                var abogado = await _dbconext.abogados.FirstOrDefaultAsync(a => a.id_Usuario == idUsuario);
                idAbogado = abogado?.idAbogado;
            }

            var esAdmin = User.IsInRole("Administrador");

            var eventos = new List<object>();

            IQueryable<Audiencia> audienciasQ = _dbconext.audiencia.Include(a => a.caso);
            if (!esAdmin && idAbogado.HasValue)
                audienciasQ = audienciasQ.Where(a => a.id_Abogado == idAbogado.Value);

            foreach (var a in await audienciasQ.ToListAsync())
            {
                eventos.Add(new
                {
                    title = "Audiencia: " + (a.caso?.tituloCaso ?? "—"),
                    start = a.fechaAudiencia.ToString("yyyy-MM-dd") + "T" + a.horaAudiencia.ToString("HH:mm:ss"),
                    color = "#1B263B",
                    url = Url.Action("Edit", "Audiencia", new { id = a.idAudiencia })
                });
            }

            IQueryable<Cita> citasQ = _dbconext.cita.Include(c => c.cliente);
            if (!esAdmin && idAbogado.HasValue)
                citasQ = citasQ.Where(c => c.id_Abogado == idAbogado.Value);

            foreach (var c in await citasQ.ToListAsync())
            {
                eventos.Add(new
                {
                    title = "Cita: " + (c.cliente?.nombreCliente ?? "—"),
                    start = c.fechaHoraCita.ToString("yyyy-MM-dd"),
                    color = "#415A77",
                    url = Url.Action("Edit", "Citas", new { id = c.idCita })
                });
            }

            return Json(eventos);
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
