using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Audiencia
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAudiencia { get; set; }

        [Required, StringLength(100)]
        public string direccionAudiencia { get; set; }
        [Required, StringLength(200)]
        public string tipoAudiencia { get; set; }
        [Required, StringLength(200)]
        public string linkAudiencia { get; set; }
        [Required]
        public DateOnly fechaAudiencia { get; set; }
        [Required]
        public DateTime horaAudiencia { get; set; }
    }
}
