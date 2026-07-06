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
        public DbSet<Rol> roles { get; set; }
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
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.rol)
                .WithMany(r => r.usuarios)
                .HasForeignKey(u => u.RolId);
            modelBuilder.Entity<AbogadoArea>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.abogadoArea)
                .HasForeignKey(u => u.id_Abogado);

            modelBuilder.Entity<AbogadoArea>()
                .HasOne(u => u.areaDerecho)
                .WithMany(r => r.abogadoArea)
                .HasForeignKey(u => u.id_AreaDerecho);

            modelBuilder.Entity<AbogadoServicio>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.abogadoServicio)
                .HasForeignKey(u => u.id_Abogado);

            modelBuilder.Entity<AbogadoServicio>()
                .HasOne(u => u.servicioLegal)
                .WithMany(r => r.abogadoServicio)
                .HasForeignKey(u => u.id_ServicioLegal);

            modelBuilder.Entity<Notificacion>()
                .HasOne(u => u.usuario)
                .WithMany(r => r.notificacion)
                .HasForeignKey(u => u.id_Usuario);

            modelBuilder.Entity<Abogado>()
                .HasOne(a => a.usuario)
                .WithOne(u => u.abogado)
                .HasForeignKey<Abogado>(a => a.id_Usuario);


            modelBuilder.Entity<Cita>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.cita)
                .HasForeignKey(u => u.id_Abogado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(u => u.cliente)
                .WithMany(r => r.cita)
                .HasForeignKey(u => u.id_Cliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Audiencia>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.audiencia)
                .HasForeignKey(u => u.id_Abogado);

            modelBuilder.Entity<Audiencia>()
                .HasOne(u => u.caso)
                .WithMany(r => r.audiencia)
                .HasForeignKey(u => u.id_Caso);

            modelBuilder.Entity<Caso>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.caso)
                .HasForeignKey(u => u.id_Abogado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caso>()
                .HasOne(u => u.cliente)
                .WithMany(r => r.caso)
                .HasForeignKey(u => u.id_Cliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.cliente)
                .HasForeignKey(u => u.idAbogado)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Pago>()
                .HasOne(u => u.caso)
                .WithMany(r => r.pago)
                .HasForeignKey(u => u.id_Caso);

            modelBuilder.Entity<Pago>()
                .HasOne(u => u.cliente)
                .WithMany(r => r.pago)
                .HasForeignKey(u => u.idCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(u => u.abogado)
                .WithMany(r => r.pago)
                .HasForeignKey(u => u.idAbogado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expediente>()
                .HasOne(u => u.caso)
                .WithOne(r => r.expediente)
                .HasForeignKey<Expediente>(u => u.id_Caso);

            modelBuilder.Entity<DocumentosLegales>()
                .HasOne(u => u.expediente)
                .WithMany(r => r.documentosLegales)
                .HasForeignKey(u => u.id_Expediente);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { idRol = 1, nombre = "Abogado" },
                new Rol { idRol = 2, nombre = "Administrador" },
                new Rol { idRol = 3, nombre = "Usuario" }
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { idUsuario = 1, nombreUsuario = "Miguel Rojas", passwordUsuario = "12345", RolId = 1 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

