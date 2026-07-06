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
        [Required, StringLength(50), RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos numéricos.")]
        public string telefonoAbogado { get; set; }
        [Required, StringLength(20), RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string dniAbogado { get; set; }
        [Required, StringLength(100), EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string correoAbogado { get; set; }
        [Required, StringLength(50)]
        public string especialidadAbogado { get; set; }
        [Required]
        public bool estadoAbogado { get; set; }

        public int id_Usuario { get; set; }
        public Usuario usuario { get; set; }

        public ICollection<AbogadoArea> abogadoArea { get; set; }

        public ICollection<AbogadoServicio> abogadoServicio { get; set; }

        public ICollection<Cita> cita { get; set; }

        public ICollection<Audiencia> audiencia { get; set; }

        public ICollection<Caso> caso { get; set; }

        public ICollection<Cliente> cliente { get; set; }

        public ICollection<Pago> pago { get; set; }
    }
}
