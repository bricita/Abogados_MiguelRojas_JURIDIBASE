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
        [Required, Range(0.01, 9999999, ErrorMessage = "El monto debe ser mayor a 0.")]
        public float monto { get; set; }
        [Required]
        public DateOnly fechaPago { get; set; }
        public int idCliente  { get; set; }
        public Cliente cliente { get; set; }
        public int idAbogado { get; set; }
        public Abogado abogado { get; set; }

        public int id_Caso { get; set; }
        public Caso caso {  get; set; }

        
    }
}
