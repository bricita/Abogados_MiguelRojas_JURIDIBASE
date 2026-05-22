using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Abogado
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAbogado { get; set; }
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


    }
}
