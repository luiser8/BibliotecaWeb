namespace Domain.Ports;

public interface IPasswordHasher
{
    string HashSimplePasswordAsync(string password);
    string HashPasswordAsync(string? password);
    bool VerifyPasswordAsync(string password, string hashedPassword);
}