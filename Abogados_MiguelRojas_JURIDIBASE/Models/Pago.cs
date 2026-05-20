using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Pago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPago { get; set; }
        [Required, StringLength(50)]
        public string metodoPago {  get; set; }
        public float monto { get; set; }
        public DateOnly fechaPago { get; set; }
        public Caso caso {  get; set; }

        
    }
}
