using Abogados_MiguelRojas_JURIDIBASE.Models;
using Microsoft.EntityFrameworkCore;

namespace Abogados_MiguelRojas_JURIDIBASE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Abogado> abogados { get; set; }
        public DbSet<AbogadoArea> abogadoArea { get; set; }
        public DbSet<AbogadoServicio> AbogadoServicios { get; set; }
        public DbSet<AreaDerecho> areasDerecho { get; set; }
        public DbSet<Audiencia> audiencias { get; set; }
        public DbSet<Caso> casos { get; set; }
        public DbSet<Cita> citas { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<DocumentosLegales> documentos { get; set; }
        public DbSet<Especialista> especialistas { get; set; }
        public DbSet<Expediente> expedientes{ get; set; }
        public DbSet<Notificacion> notificaciones { get; set; }
        public DbSet<Pago> pagos{  get; set; }
        public DbSet<ServicioLegal> servicios { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        

    }
}
