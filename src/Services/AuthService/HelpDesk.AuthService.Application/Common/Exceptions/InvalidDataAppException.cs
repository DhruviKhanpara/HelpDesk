namespace HelpDesk.AuthService.Application.Common.Exceptions;

public sealed class InvalidDataAppException : AppException
{
    public InvalidDataAppException(string message) 
        : base(message)
    {
    }
}
