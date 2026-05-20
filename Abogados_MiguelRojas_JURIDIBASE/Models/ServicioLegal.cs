using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class ServicioLegal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idServicio {  get; set; }
        [Required, StringLength(100)]
        public string nombre { get; set; }
        [Required, StringLength(200)]
        public string descripcion {  get; set; }
        [Required, StringLength(100)]
        public string estado { get; set; }  
        public float costoBase { get; set; }
    }
}
