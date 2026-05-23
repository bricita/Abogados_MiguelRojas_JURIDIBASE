using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class AbogadoServicio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAbogadoServicio { get; set; }

        //Conexion de muchos a uno con servicioLegal
        public int id_ServicioLegal { get; set; }
        public ServicioLegal servicioLegal { get; set; }

        //Conexion de muchos a uno con Abogado
        public int id_Abogado { get; set; }
        public Abogado abogado { get; set; }
    }
}
