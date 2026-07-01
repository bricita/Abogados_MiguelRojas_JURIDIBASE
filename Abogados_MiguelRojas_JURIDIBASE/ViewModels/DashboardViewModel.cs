using Abogados_MiguelRojas_JURIDIBASE.Models;

namespace Abogados_MiguelRojas_JURIDIBASE.ViewModels
{
    public class DashboardViewModel
    {
        // Stats generales
        public int TotalCasosActivos { get; set; }
        public int TotalClientesActivos { get; set; }
        public int TotalAudienciasHoy { get; set; }
        public int NotificacionesNoLeidas { get; set; }
        public int TotalCitasPendientes { get; set; }

        // Listas recientes
        public List<Caso> UltimosCasos { get; set; } = new();
        public List<Audiencia> ProximasAudiencias { get; set; } = new();
        public List<Cita> ProximasCitas { get; set; } = new();
        public List<Pago> UltimosPagos { get; set; } = new();
        public List<Notificacion> UltimasNotificaciones { get; set; } = new();

        // Admin-only
        public int TotalAbogadosActivos { get; set; }
        public int TotalUsuarios { get; set; }
        public decimal IngresosTotales { get; set; }
        public int TotalExpedientesActivos { get; set; }
        public List<Abogado> AbogadosRecientes { get; set; } = new();
    }
}
