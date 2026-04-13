using Consulate.Application.Interfaces;
using Consulate.Infrastructure.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendVerificationEmail(string email, string token)
    {
        var link = $"https://localhost:5001/api/auth/verify-email?token={token}";

        var body = $@"
        <h2>Verify Email</h2>
        <p>Click below:</p>
        <a href='{link}'>Verify</a>
        ";

        var message = new MailMessage();
        message.From = new MailAddress(_settings.Email);
        message.To.Add(email);
        message.Subject = "Verify Email";
        message.Body = body;
        message.IsBodyHtml = true;

        using var smtp = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Email, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        await smtp.SendMailAsync(message);
    }
}
