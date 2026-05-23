using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Audiencia
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAudiencia { get; set; }

        [Required, StringLength(100)]
        public string direccionAudiencia { get; set; }
        [Required, StringLength(200)]
        public string tipoAudiencia { get; set; }
        [Required, StringLength(200)]
        public string linkAudiencia { get; set; }
        [Required]
        public DateOnly fechaAudiencia { get; set; }
        [Required]
        public DateTime horaAudiencia { get; set; }

        //Conexion de muchos a uno con Abogado
        public int id_Abogado { get; set; }
        public Abogado abogado { get; set; }

        //Conexion de muchos a uno con Caso
        public int id_Caso { get; set; }
        public Caso caso { get; set; }
    }
}
