using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Domain.Ports;

namespace Infrastructure.Email;

public class EmailAdapter : IEmailPort
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender Name", "sender@example.com"));
        message.To.Add(new MailboxAddress("Recipient Name", to));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();
        // Connect to the SMTP server
        await client.ConnectAsync("smtp.example.com", 587, SecureSocketOptions.StartTls);

        // Authenticate with credentials (use App Passwords for Gmail/Outlook if 2FA is on)
        await client.AuthenticateAsync("your_username", "your_app_password");

        // Send the message
        await client.SendAsync(message);

        // Disconnect
        await client.DisconnectAsync(true);
    }
}

