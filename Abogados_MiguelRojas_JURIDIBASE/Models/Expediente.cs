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
        [Required, StringLength(20)]
        public string victima {  get; set; }
        [Required, StringLength(20)]
        public string victimario {  get; set; }
        [Required, StringLength(10)]
        public string fechaInicio { get; set; }
        [Required, StringLength(10)]
        public string fechaCierre { get; set; }

    }
}