namespace HelpDesk.AuthService.Application.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string entity, object id)
        : base($"{entity} ({id}) was not found.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }
}
