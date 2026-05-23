using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Cita
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCita { get; set; }
        [Required, StringLength(150)]
        public string asuntoLegalCita { get; set; }
        [Required, StringLength(500)]
        public string detallesAdicionalesCita { get; set; }
        [Required]
        public DateOnly fechaHoraCita { get; set; }
        [Required]
        public bool estadoCita { get; set; }

        //Conexion de muchos a uno con Abogados
        public int id_Abogado { get; set; }
        public Abogado abogado {  get; set; }

        //Conexion de muchos a uno con Cliente
        public int id_Cliente { get; set; }
        public Cliente cliente { get; set; }
    }
}
