namespace TesisAdvocorp.Modelos.Dtos
{
    public class CasoDTO
    {
        public int CasoId { get; set; }
        public string Descripcion { get; set; }
        public string Asunto { get; set; }
        public string Estado { get; set; }
        public int AbogadoId { get; set; }
        public int ClienteId { get; set; }
        public int CitaId { get; set; }
        public string EspecialidadDescripcion { get; set; }
        public string NombreCliente { get; set; }  // Agregar este campo para mostrar el nombre del cliente en el frontend
        public DateTime FechaCita { get; set; }  // Asegúrate de que la fecha de la cita también se envíe
        public string NombreAbogado { get; set; }
    }
}
