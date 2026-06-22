namespace Abogados_MiguelRojas_JURIDIBASE.ViewModels
{
    public class AbogadoVM
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string DNI { get; set; }
        public string Especialidad { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string RepPassword { get; set; }
        // Id del usuario relacionado (si aplica)
        public int idUsuario { get; set; }

        // Id del abogado (si ya existe en BD)
        public int idAbogado { get; set; }

        // Estado del abogado (activo/inactivo)
        public bool estadoAbogado { get; set; }

        // Campos del usuario relacionados al abogado
        public string nombreUsuario { get; set; }
    }
}
