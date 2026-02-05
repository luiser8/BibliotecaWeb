
namespace Application.UseCases;

public interface IEmailCommandUseCase
{
    Task SendEmailAsync(string to, string subject, string body);
}