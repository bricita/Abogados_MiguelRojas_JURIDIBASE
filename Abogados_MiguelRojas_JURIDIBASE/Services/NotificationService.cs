using Abogados_MiguelRojas_JURIDIBASE.Data;
using Abogados_MiguelRojas_JURIDIBASE.Models;

namespace Abogados_MiguelRojas_JURIDIBASE.Services
{
    public static class NotificationService
    {
        public static async Task CrearAsync(AppDbContext db, int idUsuario, string titulo, string mensaje)
        {
            db.notificacion.Add(new Notificacion
            {
                tituloNotificacion = titulo,
                mensajeNotificacion = mensaje,
                leido = false,
                fechaNotificacion = DateOnly.FromDateTime(DateTime.Today),
                id_Usuario = idUsuario
            });
            await db.SaveChangesAsync();
        }

        public static async Task NotificarAudienciaCreadaAsync(AppDbContext db, int idAbogado, string casoTitulo, DateOnly fecha)
        {
            var abogado = await db.abogados.FindAsync(idAbogado);
            if (abogado == null) return;
            await CrearAsync(db, abogado.id_Usuario,
                "Nueva audiencia programada",
                $"Se registró una audiencia para el caso \"{casoTitulo}\" el {fecha:dd/MM/yyyy}.");
        }

        public static async Task NotificarCasoEstadoAsync(AppDbContext db, int idAbogado, string casoTitulo, bool activo)
        {
            var abogado = await db.abogados.FindAsync(idAbogado);
            if (abogado == null) return;
            var estado = activo ? "activado" : "desactivado";
            await CrearAsync(db, abogado.id_Usuario,
                "Estado de caso actualizado",
                $"El caso \"{casoTitulo}\" fue {estado}.");
        }

        public static async Task NotificarPagoAsync(AppDbContext db, int idAbogado, string clienteNombre, decimal monto)
        {
            var abogado = await db.abogados.FindAsync(idAbogado);
            if (abogado == null) return;
            await CrearAsync(db, abogado.id_Usuario,
                "Nuevo pago registrado",
                $"Se registró un pago de S/ {monto:N2} del cliente {clienteNombre}.");
        }

        public static async Task NotificarCitaCreadaAsync(AppDbContext db, int idAbogado, string clienteNombre, DateOnly fecha)
        {
            var abogado = await db.abogados.FindAsync(idAbogado);
            if (abogado == null) return;
            await CrearAsync(db, abogado.id_Usuario,
                "Nueva cita agendada",
                $"Se agendó una cita con {clienteNombre} para el {fecha:dd/MM/yyyy}.");
        }
    }
}
