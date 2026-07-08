using HelpDesk.AuthService.Application.Common.Interfaces;
using System.Security.Cryptography;

namespace HelpDesk.AuthService.Infrastructure.Services;

/// <summary>
/// PBKDF2 password hashing using only built-in .NET crypto (System.Security.Cryptography).
/// No external package required. Stored format: "{iterations}.{saltBase64}.{hashBase64}"
/// so the iteration count and salt travel with the hash - if you ever raise the
/// iteration count, old hashes still verify correctly.
/// </summary>
internal class PasswordHasher : IPasswordHasher
{
    private const int SaltSizeBytes = 16;
    private const int HashSizeBytes = 32;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSizeBytes);

        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hash)
    {
        var parts = hash.Split('.', 3);
        if (parts.Length != 3) return false;

        if (!int.TryParse(parts[0], out var iterations)) return false;

        var salt = Convert.FromBase64String(parts[1]);
        var expectedHash = Convert.FromBase64String(parts[2]);

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, expectedHash.Length);

        // Constant-time comparison to avoid timing attacks
        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
