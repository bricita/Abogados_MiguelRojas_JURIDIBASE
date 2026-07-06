using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class AbogadoArea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAbogadoArea { get; set; }
        
        public int id_Abogado {  get; set; }
        public Abogado abogado { get; set; }

        public int id_AreaDerecho { get; set; }
        public AreaDerecho areaDerecho { get; set; }
        
    }
}
