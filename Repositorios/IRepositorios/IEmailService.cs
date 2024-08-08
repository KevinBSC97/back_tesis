namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
