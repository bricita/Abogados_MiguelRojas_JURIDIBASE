using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Expediente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idExpediente { get; set; }
        [Required, StringLength(70)]
        public string tituloExpediente { get; set; }
        [Required, StringLength(70)]
        public string tipoExpediente { get; set;  }
        [Required, StringLength(200)]
        public string resumenExpediente { get; set; }
        [Required]
        public bool estadoExpediente { get; set; }
        [Required, StringLength(100)]
        public string victima {  get; set; }
        [Required, StringLength(100)]
        public string victimario {  get; set; }
        [Required]
        public DateOnly fechaInicio { get; set; }
        [Required]
        public DateOnly fechaCierre { get; set; }

        public int id_Caso { get; set; }
        public Caso caso { get; set; }

        public ICollection<DocumentosLegales> documentosLegales { get; set; }

    }
}
