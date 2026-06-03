using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCliente { get; set;  }
        [Required, StringLength(50)]
        public string nombreCliente { get; set; }
        [Required, StringLength(200)]
        public string descripcionCliente { get; set; }
        [Required, StringLength(8)]
        public string dniCliente { get; set; }
        [Required, StringLength(15)]
        public string rucCliente { get; set; }
        [Required, StringLength(9)]
        public string telefonoCliente { get; set; }
        [Required, StringLength(15)]
        public string direccionCliente { get; set; }
        [Required, StringLength(15)]
        public string correoCliente { get; set; }
        [Required]
        public bool estadoCliente { get; set; }
        [Required]
        public string tipoCliente { get; set; }

        //Conexion de uno a muchos con Cita
        public ICollection<Cita> cita { get; set; }

        //Conexion de uno a muchos con Caso
        public ICollection<Caso> caso { get; set; }
    }
}
