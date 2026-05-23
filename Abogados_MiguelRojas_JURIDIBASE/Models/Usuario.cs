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
        public string rolUsuario {  get; set; }
        [Required, StringLength(50)]
        public string passwordUsuario { get; set; }

        //Conexion de uno a muchos con Notificaciones
        public ICollection<Notificacion> notificacion { get; set; }
        
        //Conexion de uno a uno con abogado
        public Abogado abogado { get; set; }
    }
}
