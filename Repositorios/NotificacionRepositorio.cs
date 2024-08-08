using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Repositorios
{
    public class NotificacionRepositorio : INotificacionRepositorio
    {
        private readonly AppDbContext _db;

        public NotificacionRepositorio(AppDbContext db)
        {
            _db = db;
        }

        public async Task EnviarNotificacionAsync(int usuarioId, string mensaje)
        {
            var usuario = await _db.Usuarios.FindAsync(usuarioId);
            if (usuario != null)
            {
                var notificacion = new Notificacion
                {
                    UsuarioId = usuarioId,
                    Mensaje = mensaje,
                    Fecha = DateTime.Now,
                    Leida = false
                };
                _db.Notificaciones.Add(notificacion);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NotificacionDTO>> GetNotificacionesPorUsuario(int usuarioId)
        {
            return await _db.Notificaciones
                            .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                            .Select(n => new NotificacionDTO
                            {
                                NotificacionId = n.NotificacionId,
                                Mensaje = n.Mensaje,
                                Fecha = n.Fecha,
                                Leida = n.Leida
                            })
                            .ToListAsync();
        }

        public async Task<int> GetNumeroDeNotificacionesNoLeidas(int usuarioId)
        {
            return await _db.Notificaciones
                            .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                            .CountAsync();
        }

        public async Task MarcarComoLeida(int notificacionId)
        {
            var notificacion = await _db.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                await _db.SaveChangesAsync();
            }
        }
    }
}
