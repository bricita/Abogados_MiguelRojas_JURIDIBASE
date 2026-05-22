using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class AbogadoArea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAbogadoArea {  get; set; }

    }
}
