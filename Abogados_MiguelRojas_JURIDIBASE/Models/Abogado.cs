using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Abogado
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAbogado { get; set; }
        [Required, StringLength(150)]
        public string nombreAbogado { get; set; }
        [Required, StringLength(150)]
        public string apellidoAbogado { get; set; }
        [Required, StringLength(50)]
        public string telefonoAbogado { get; set; }
        [Required, StringLength(20)]
        public string dniAbogado { get; set; }
        [Required, StringLength(100)]
        public string correoAbogado { get; set; }
        [Required, StringLength(50)]
        public string especialidadAbogado { get; set; }
        [Required]
        public bool estadoAbogado { get; set; }

        //Conexion de uno a uno con usuario
        public int id_Usuario { get; set; }
        public Usuario usuario { get; set; }

        //Conexion de uno a muchos con AbogadoArea
        public ICollection<AbogadoArea> abogadoArea { get; set; }

        //Conexion de uno a muchos con AbogadoServicio
        public ICollection<AbogadoServicio> abogadoServicio { get; set; }

        //Conexion de uno a muchos con Citas
        public ICollection<Cita> cita { get; set; }

        //Conexion de uno a muchos con Audiencia
        public ICollection<Audiencia> audiencia { get; set; }

        //Conexion de uno a muchos con caso
        public ICollection<Caso> caso { get; set; }
    }
}
