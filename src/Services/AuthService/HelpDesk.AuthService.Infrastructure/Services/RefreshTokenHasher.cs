using HelpDesk.AuthService.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HelpDesk.AuthService.Infrastructure.Services;

internal class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}
