using TesisAdvocorp.Modelos.Dtos;

namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface INotificacionRepositorio
    {
        Task EnviarNotificacionAsync(int usuarioId, string mensaje);
        Task<IEnumerable<NotificacionDTO>> GetNotificacionesPorUsuario(int usuarioId);
        Task<int> GetNumeroDeNotificacionesNoLeidas(int usuarioId); // Nuevo método
        Task MarcarComoLeida(int notificacionId);
    }
}
