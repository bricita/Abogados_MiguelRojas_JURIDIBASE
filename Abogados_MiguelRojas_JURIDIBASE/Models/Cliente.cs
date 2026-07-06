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
        
        public string? descripcionCliente { get; set; }
        [Required, StringLength(8), RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string dniCliente { get; set; }

        public string? rucCliente { get; set; }
        [Required, StringLength(9), RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos numéricos.")]
        public string telefonoCliente { get; set; }
        [Required, StringLength(100)]
        public string direccionCliente { get; set; }
        [Required, StringLength(100), EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string correoCliente { get; set; }
        [Required]
        public bool estadoCliente { get; set; }
        [Required, StringLength(50)]
        public string tipoCliente { get; set; }

        public int idAbogado { get; set; }
        public Abogado abogado { get; set; }

        public ICollection<Cita> cita { get; set; }

        public ICollection<Caso> caso { get; set; }

        public ICollection<Pago> pago { get; set; }
    }
}
