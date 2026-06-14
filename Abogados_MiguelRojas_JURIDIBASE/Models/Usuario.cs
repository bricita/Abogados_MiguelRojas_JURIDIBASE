using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario {  get; set; }
        [Required, StringLength(100)]
        public string nombreUsuario { get; set; }
        [Required, StringLength(50)]
        public string passwordUsuario { get; set; }
        public ICollection<Notificacion> notificacion { get; set; }
        public Abogado abogado { get; set; }
        public int RolId { get; set; }
        public Rol rol { get; set; }
    }
}
