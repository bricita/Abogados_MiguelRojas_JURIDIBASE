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
        [Required, StringLength(8)]
        public string dniEspecialista { get; set; }
        [Required]
        public bool disponibilidadEspecialista { get; set; }
        [Required, StringLength(9)]
        public string telefonoEspecialista { get; set; }
        [Required, StringLength(20)]
        public string correoEspecialista { get; set; }
    }
}