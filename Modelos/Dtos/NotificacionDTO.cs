namespace TesisAdvocorp.Modelos.Dtos
{
    public class NotificacionDTO
    {
        public int NotificacionId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Leida { get; set; }
    }
}
