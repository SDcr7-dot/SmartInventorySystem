using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartInventorySystem.Configurations;
using SmartInventorySystem.Interfaces;

namespace SmartInventorySystem.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(
        IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmail(
        string to,
        string subject,
        string body)
    {
        var email = new MimeMessage();

        email.From.Add(
            MailboxAddress.Parse(_settings.Email));

        email.To.Add(
            MailboxAddress.Parse(to));

        email.Subject = subject;

        email.Body =
            new TextPart("plain")
            {
                Text = body
            };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            false);

        await smtp.AuthenticateAsync(
            _settings.Email,
            _settings.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}