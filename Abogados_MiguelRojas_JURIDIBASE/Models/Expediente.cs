using System;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Expediente
    {
        public int IdExpediente { get; set; }
        public string numeroExpediente { get; set; }
        public string materiaExpediente { get; set; }
        public string descripcionExpediente { get; set; }
        public DateTime fechaInicioExpediente { get; set; }
        public string estadoExpediente { get; set; } 

        // Relación con el Abogado asignado
        public int IdAbogado { get; set; }
        public Abogado Abogado { get; set; }
    }
}