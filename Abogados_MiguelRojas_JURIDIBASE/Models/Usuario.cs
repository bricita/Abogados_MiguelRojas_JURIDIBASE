using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario {  get; set; }
        [Required, StringLength(100)]
        public string nombre { get; set; }
        [Required, StringLength(50)]
        public string rol {  get; set; }
        [Required, StringLength(50)]
        public string password { get; set; }

    }
}
