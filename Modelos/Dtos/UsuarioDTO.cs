namespace TesisAdvocorp.Modelos.Dtos
{
    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public string RolDescripcion { get; set; }
        public int? EspecialidadId { get; set; }
        public string Estado { get; set; }
        public string EspecialidadDescripcion { get; set; }
    }
}
