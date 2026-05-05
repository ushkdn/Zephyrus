using MailKit.Net.Smtp;
using MimeKit;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Application.Models;
using Zephyrus.Identity.Infrastructure.Settings;

namespace Zephyrus.Identity.Infrastructure.Services;

public class EmailService(EmailSettings emailSettings) : IEmailService
{
    public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailSettings.From));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;
        email.Body = new TextPart("html") { Text = message.Body };

        using var client = new SmtpClient();
        await client.ConnectAsync(emailSettings.Host, emailSettings.Port, true, cancellationToken);
        await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password, cancellationToken);
        await client.SendAsync(email, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}