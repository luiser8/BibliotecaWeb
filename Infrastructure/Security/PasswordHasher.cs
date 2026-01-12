using System.Security.Cryptography;
using System.Text;
using Domain.Ports;

namespace Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    
    public string HashSimplePasswordAsync(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(passwordBytes);
        return Convert.ToBase64String(hash);
    }

    public string HashPasswordAsync(string? password)
    {
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var passwordBytes = Encoding.UTF8.GetBytes(password ?? throw new ArgumentNullException(nameof(password)));
        var saltedPassword = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);
        var hash = SHA256.HashData(saltedPassword);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPasswordAsync(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hashStored = Convert.FromBase64String(parts[1]);

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPassword = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);
        var hashComputed = SHA256.HashData(saltedPassword);
        return CryptographicOperations.FixedTimeEquals(hashStored, hashComputed);
    }
}