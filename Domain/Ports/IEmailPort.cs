
namespace Domain.Ports;

public interface IEmailPort
{
    Task SendEmailAsync(string to, string subject, string body);
}

