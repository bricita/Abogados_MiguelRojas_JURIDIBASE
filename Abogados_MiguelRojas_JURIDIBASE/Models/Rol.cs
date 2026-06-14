using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Rol
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idRol {  get; set; }
        [Required, StringLength(50)]
        public string nombre { get; set; }
        public ICollection<Usuario> usuarios{ get; set; }
       
    }
}
