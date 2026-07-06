using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class AbogadoServicio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAbogadoServicio { get; set; }

        public int id_ServicioLegal { get; set; }
        public ServicioLegal servicioLegal { get; set; }

        public int id_Abogado { get; set; }
        public Abogado abogado { get; set; }
    }
}
