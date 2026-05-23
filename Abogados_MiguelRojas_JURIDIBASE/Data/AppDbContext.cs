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
        public DbSet<AbogadoServicio> AbogadoServicio { get; set; }
        public DbSet<AreaDerecho> areasDerecho { get; set; }
        public DbSet<Audiencia> audiencia { get; set; }
        public DbSet<Caso> caso { get; set; }
        public DbSet<Cita> cita { get; set; }
        public DbSet<Cliente> cliente { get; set; }
        public DbSet<DocumentosLegales> documento { get; set; }
        public DbSet<Especialista> especialista { get; set; }
        public DbSet<Expediente> expediente { get; set; }
        public DbSet<Notificacion> notificacion { get; set; }
        public DbSet<Pago> pago {  get; set; }
        public DbSet<ServicioLegal> servicio { get; set; }
        public DbSet<Usuario> usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Abogado (1) -> (M) AbogadoArea
            modelBuilder.Entity<AbogadoArea>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.abogadoArea)
                .HasForeignKey(u => u.id_Abogado);

            //AreaDerecho (1) -> (M) AbogadoArea
            modelBuilder.Entity<AbogadoArea>()
                .HasOne(u => u.areaDerecho)
                .WithMany(r => r.abogadoArea)
                .HasForeignKey(u => u.id_AreaDerecho);

            //Abogado (1) -> (M) AbogadoServicio
            modelBuilder.Entity<AbogadoServicio>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.abogadoServicio)
                .HasForeignKey(u => u.id_Abogado);

            //ServicioLegal (1) -> (M) AbogadoServicio
            modelBuilder.Entity<AbogadoServicio>()
                .HasOne(u => u.servicioLegal)
                .WithMany(r => r.abogadoServicio)
                .HasForeignKey(u => u.id_ServicioLegal);

            //Usuario (1) -> (M) Notificacion
            modelBuilder.Entity<Notificacion>()
                .HasOne(u => u.usuario)
                .WithMany(r => r.notificacion)
                .HasForeignKey(u => u.id_Usuario);

            //Usuario (1) -> (1) Abogado (FK en Abogado: id_Usuario)
            modelBuilder.Entity<Abogado>()
                .HasOne(a => a.usuario)
                .WithOne(u => u.abogado)
                .HasForeignKey<Abogado>(a => a.id_Usuario);

            //Abogado (1) -> (M) Cita
            modelBuilder.Entity<Cita>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.cita)
                .HasForeignKey(u => u.id_Abogado);

            //Cliente (1) -> (M) Cita
            modelBuilder.Entity<Cita>()
                .HasOne(u => u.cliente)
                .WithMany(r => r.cita)
                .HasForeignKey(u => u.id_Cliente);

            //Abogado (1) -> (M) Audiencia
            modelBuilder.Entity<Audiencia>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.audiencia)
                .HasForeignKey(u => u.id_Abogado);

            //Caso (1) -> (M) Audiencia
            modelBuilder.Entity<Audiencia>()
                .HasOne(u => u.caso)
                .WithMany(r => r.audiencia)
                .HasForeignKey(u => u.id_Caso);

            //Abogado (1) -> (M) Caso
            modelBuilder.Entity<Caso>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.caso)
                .HasForeignKey(u => u.id_Abogado);

            //Cliente (1) -> (M) Caso
            modelBuilder.Entity<Caso>()
                .HasOne(u => u.cliente)
                .WithMany(r => r.caso)
                .HasForeignKey(u => u.id_Cliente);

            //Caso (1) -> (M) Pago
            modelBuilder.Entity<Pago>()
                .HasOne(u => u.caso)
                .WithMany(r => r.pago)
                .HasForeignKey(u => u.id_Caso);

            //Caso (1) -> (1) Expediente
            modelBuilder.Entity<Expediente>()
                .HasOne(u => u.caso)
                .WithOne(r => r.expediente)
                .HasForeignKey<Expediente>(u => u.id_Caso);

            //Expediente (1) -> (M) DocumentosLegales
            modelBuilder.Entity<DocumentosLegales>()
                .HasOne(u => u.expediente)
                .WithMany(r => r.documentosLegales)
                .HasForeignKey(u => u.id_Expediente);

            base.OnModelCreating(modelBuilder);
        }
    }
}
