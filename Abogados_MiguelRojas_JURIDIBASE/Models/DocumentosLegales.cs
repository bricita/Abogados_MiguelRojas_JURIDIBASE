using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class DocumentosLegales
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idDocumentoLegal {  get; set; }
        [Required, StringLength(70)]
        public string nombreDocumento { get; set; }
        [Required, StringLength(70)]
        public string rutaDocumento { get; set; }
        [Required]
        public DateOnly fechaCreacion { get; set; }
        
        //Conexion de muchos a uno con Expediente
        public int id_Expediente { get; set; }
        public Expediente expediente { get; set; }
    }
}
