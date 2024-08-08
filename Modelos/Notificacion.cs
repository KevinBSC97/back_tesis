namespace TesisAdvocorp.Modelos
{
    public class Notificacion
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Leida { get; set; }

        public Usuario Usuario { get; set; }
    }
}
