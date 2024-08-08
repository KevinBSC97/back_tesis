using Microsoft.AspNetCore.Mvc;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Controller
{
    [Route("api/notificaciones")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionRepositorio _notificacionRepositorio;

        public NotificacionesController(INotificacionRepositorio notificacionRepositorio)
        {
            _notificacionRepositorio = notificacionRepositorio;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetNotificacionesPorUsuario(int usuarioId)
        {
            var notificaciones = await _notificacionRepositorio.GetNotificacionesPorUsuario(usuarioId);
            if (notificaciones == null) return NotFound();
            return Ok(notificaciones);
        }

        [HttpGet("usuario/{usuarioId}/numero")]
        public async Task<IActionResult> GetNumeroDeNotificacionesNoLeidas(int usuarioId)
        {
            var numeroDeNotificaciones = await _notificacionRepositorio.GetNumeroDeNotificacionesNoLeidas(usuarioId);
            return Ok(new { numeroDeNotificaciones });
        }

        [HttpPost("marcar-leida/{notificacionId}")]
        public async Task<IActionResult> MarcarComoLeida(int notificacionId)
        {
            await _notificacionRepositorio.MarcarComoLeida(notificacionId);
            return NoContent();
        }
    }
}
