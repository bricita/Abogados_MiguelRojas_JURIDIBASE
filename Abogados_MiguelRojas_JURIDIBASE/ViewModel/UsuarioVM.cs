using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.ViewModel
{
    public class UsuarioVM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }
        [Required, StringLength(100)]
        public string nombreUsuario { get; set; }
        [Required, StringLength(50)]
        public string rolUsuario { get; set; }
        [Required, StringLength(50)]
        public string password{ get; set; }
        public string RepPassword { get; set; }
    }
}
