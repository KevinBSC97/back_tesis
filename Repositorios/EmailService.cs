using System.Net.Mail;
using System.Net;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Repositorios
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MailMessage("youremail@example.com", to, subject, body);
            using (var client = new SmtpClient("smtp.example.com", 587))
            {
                client.Credentials = new NetworkCredential("username", "password");
                client.EnableSsl = true;
                await client.SendMailAsync(email);
            }
        }
    }
}
