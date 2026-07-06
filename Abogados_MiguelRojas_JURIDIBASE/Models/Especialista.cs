using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Especialista
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idEspecialista { get; set; }
        [Required, StringLength(100)]
        public string nombreEspecialista { get; set; }
        [Required, StringLength(500)]
        public string descripcionEspecialista { get; set; }
        [Required, StringLength(30)]
        public string estadoEspecialista { get; set; }
        [Required, StringLength(8), RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
        public string dniEspecialista { get; set; }
        [Required]
        public bool disponibilidadEspecialista { get; set; }
        [Required, StringLength(9), RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos numéricos.")]
        public string telefonoEspecialista { get; set; }
        [Required, StringLength(50), EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string correoEspecialista { get; set; }
    }
}