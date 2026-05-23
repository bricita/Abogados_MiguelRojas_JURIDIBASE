using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Caso
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCaso { get; set; }
        [Required, StringLength(100)]
        public string tituloCaso { get; set; }
        [Required, StringLength(500)]
        public string descripcionCaso { get; set; }
        [Required]
        public bool estadoCaso { get; set; }

        //Conexion de uno a uno con Expediente
        public Expediente expediente { get; set; }

        //Conexion de muchos a uno con Abogado
        public int id_Abogado { get; set; }
        public Abogado abogado { get; set; }

        //Conexion de muchos a uno con Cliente
        public int id_Cliente { get; set; }
        public Cliente cliente { get; set; }

        //Conexion de uno a muchos con Audiencia
        public ICollection<Audiencia> audiencia { get; set; }

        //Conexion de uno a muchos con Pago
        public ICollection<Pago> pago { get; set; }
    }
}
