using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.ViewModels
{
    public class UsuarioVM
    {
        public string nombreUsuario { get; set; }
        public string password { get; set; }
        public string RepPassword { get; set; }

        
        public int RolId { get; set; }
    }
}
