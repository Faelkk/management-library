namespace LibraryManagement.Services;


using System.Net;
using System.Net.Mail;
using LibraryManagement.Dto;

public class EmailService : IEmailService
{
    private readonly string _emailHost;
    private readonly string _emailFrom;
    private readonly string _emailPassword;

    public EmailService(IConfiguration configuration)
    {
        _emailHost = configuration["EmailSettings:SmtpServer"]
            ?? throw new InvalidOperationException("SmtpServer não configurado.");

        _emailFrom = configuration["EmailSettings:From"]
            ?? throw new InvalidOperationException("From não configurado.");

        _emailPassword = configuration["EmailSettings:Password"]
            ?? throw new InvalidOperationException("Password não configurado.");
    }


    public void Send(Message message)
    {
        try
        {
            using (var msgEmail = new MailMessage())
            {
                msgEmail.From = new MailAddress(_emailFrom);
                msgEmail.To.Add(message.MailTo);
                msgEmail.Subject = message.Title;
                msgEmail.Body = message.Text;
                msgEmail.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(_emailHost, 587))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailFrom, _emailPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(msgEmail);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar email: {ex.Message}");
            throw;
        }
    }
}
