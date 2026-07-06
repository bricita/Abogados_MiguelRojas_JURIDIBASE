using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class AreaDerecho
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAreaDerecho { get; set; }
        [Required, StringLength(100)]
        public string nombreAreaDerecho { get; set; }
        [Required, StringLength(200)]
        public string descripcionAreaDerecho { get; set; }
        [Required]
        public bool estadoAreaDerecho { get; set; }

        public ICollection<AbogadoArea> abogadoArea { get; set; }

    }
}
