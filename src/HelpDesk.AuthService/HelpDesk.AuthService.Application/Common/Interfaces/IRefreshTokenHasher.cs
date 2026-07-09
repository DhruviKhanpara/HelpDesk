namespace HelpDesk.AuthService.Application.Common.Interfaces;

public interface IRefreshTokenHasher
{
    string Hash(string token);
}
