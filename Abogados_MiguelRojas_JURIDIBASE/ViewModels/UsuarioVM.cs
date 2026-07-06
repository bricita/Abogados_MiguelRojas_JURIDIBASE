using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.ViewModels
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100)]
        public string nombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 50 caracteres.")]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string RepPassword { get; set; }

        [Required]
        public int RolId { get; set; }
    }
}
