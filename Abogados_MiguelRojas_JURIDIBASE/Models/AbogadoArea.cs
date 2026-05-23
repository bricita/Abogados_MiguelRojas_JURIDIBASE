using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abogados_MiguelRojas_JURIDIBASE.Models
{
    public class AbogadoArea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAbogadoArea { get; set; }
        
        //Conexion de muchos a uno con Abogado
        public int id_Abogado {  get; set; }
        public Abogado abogado { get; set; }

        //Conexion de muchos a uno con AreaDerecho
        public int id_AreaDerecho { get; set; }
        public AreaDerecho areaDerecho { get; set; }
        
    }
}
