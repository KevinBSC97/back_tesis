namespace TesisAdvocorp.Modelos.Dtos
{
    public class UsuarioRegistroDTO
    {
        public string Identificacion {  get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }
        public int? EspecialidadId { get; set; }
        public string Estado { get; set; }
    }
}
