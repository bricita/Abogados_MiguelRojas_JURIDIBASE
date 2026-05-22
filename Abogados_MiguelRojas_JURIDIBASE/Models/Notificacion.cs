using System;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        public string asuntoNotificacion { get; set; }
        public string mensajeNotificacion { get; set; }
        public DateTime fechaEnvioNotificacion { get; set; }
        public bool leidaNotificacion { get; set; } 

        // Relación con el Expediente al que pertenece la notificación
        public int IdExpediente { get; set; }
        public Expediente Expediente { get; set; }
    }
}