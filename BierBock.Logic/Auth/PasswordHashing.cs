using System.Security.Cryptography;
using System.Text;

namespace BierBockBackend.Auth;

public class PasswordHashing
{
    public static HashResult HashPassword(string password, byte[] salt = null)
    {
        salt ??= GenerateSalt();
        using var hmac = new HMACSHA256(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = hmac.ComputeHash(passwordBytes);
        return new HashResult(Convert.ToBase64String(hashBytes), salt);
    }

    private static byte[] GenerateSalt()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return randomNumber;
    }

    public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
    {
        var hashedInputPassword = HashPassword(password, salt).Hash;
        return hashedPassword == hashedInputPassword;
    }

    public record HashResult(string Hash, byte[] Salt);
}