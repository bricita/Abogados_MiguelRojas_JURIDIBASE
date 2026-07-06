using System.ComponentModel.DataAnnotations;

namespace Abogados_MiguelRojas_JURIDIBASE.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100)]
        public string NombreUser { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
