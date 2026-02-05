using Domain.Ports;

namespace Application.UseCases.Email;

public class EmailCommandUseCase : IEmailCommandUseCase
{
    private readonly IEmailPort _emailPort;

    public EmailCommandUseCase(IEmailPort emailPort)
    {
        _emailPort = emailPort;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await _emailPort.SendEmailAsync(to, subject, body);
    }
}