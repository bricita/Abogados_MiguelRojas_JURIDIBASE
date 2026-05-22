using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Notificacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idNotificacion { get; set; }
        [Required, StringLength(70)]
        public string tituloNotificacion { get; set; }
        [Required, StringLength(500)]
        public string mensajeNotificacion { get; set; }
        [Required]
        public bool leido { get;set; }
        [Required]
        public DateOnly fechaNotificacion { get; set; }
        
    }
}
