namespace TesisAdvocorp.Modelos.Dtos
{
    public class CitaDTO
    {
        public int CitaId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public int AbogadoId { get; set; }
        public string NombreAbogado { get; set; }
        public string Especialidad { get; set; }
        public string Estado { get; set; }
        public int Duracion { get; set; }
  }
}
