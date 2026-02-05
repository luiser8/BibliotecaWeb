
namespace Application.UseCases.Email;

public interface IEmailCommandUseCase
{
    Task SendEmailAsync(string to, string subject, string body);
}