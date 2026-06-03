using System.ComponentModel.DataAnnotations;

namespace Abogados_MiguelRojas_JURIDIBASE.ViewModels
{
    public class UsuarioVM
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string RepPassword { get; set; }
        public int idAbogado { get; set; }
    }
}
