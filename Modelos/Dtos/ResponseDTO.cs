namespace TesisAdvocorp.Modelos.Dtos
{
    public class ResponseDTO
    {
        public Usuario Usuario {  get; set; }
        public string Token { get; set; }  // Si estás utilizando tokens de autenticación, como JWT
    }
}
