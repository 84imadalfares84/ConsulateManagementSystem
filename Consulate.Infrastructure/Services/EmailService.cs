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

    public async Task SendLoginNotificationEmail(string email)
    {
        var body = @"
    <h3>Security Alert</h3>
    <p>Your account has been logged in successfully.</p>
    <p>If this wasn't you, please change your password immediately.</p>
    ";

        var message = new MailMessage();
        message.From = new MailAddress(_settings.Email);
        message.To.Add(email);
        message.Subject = "Login Alert";
        message.Body = body;
        message.IsBodyHtml = true;

        using var smtp = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Email, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        await smtp.SendMailAsync(message);
    }

    // ✅ الدالة الأساسية
    public async Task SendVerificationEmail(string email, string token)
    {
        var link = $"https://localhost:5001/api/auth/verify-email?token={token}";

        var htmlBody = GetHtmlTemplate(link); // 👈 استدعاء الدالة

        var message = new MailMessage();
        message.From = new MailAddress(_settings.Email);
        message.To.Add(email);
        message.Subject = "Verify Your Email";
        message.Body = htmlBody;
        message.IsBodyHtml = true;

        using var smtp = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Email, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        await smtp.SendMailAsync(message);
    }

    // 🔥 هون تحط الدالة (داخل نفس الكلاس)
    private string GetHtmlTemplate(string link)
    {
        return $@"
        <html>
        <body style='font-family:Arial; text-align:center;'>

            <h2>Welcome 👋</h2>
            <p>Please verify your email by clicking the button below:</p>

            <a href='{link}' 
               style='
                    display:inline-block;
                    padding:12px 25px;
                    background-color:#28a745;
                    color:white;
                    text-decoration:none;
                    border-radius:5px;
                    font-size:16px;
               '>
                Verify Email
            </a>

            <p style='margin-top:20px; color:gray;'>
                If you did not request this, ignore this email.
            </p>

        </body>
        </html>";
    }
}
