using Domain.Ports;

namespace Infrastructure.Email;

public class EmailAdapter : IEmailPort
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {

    }
}

