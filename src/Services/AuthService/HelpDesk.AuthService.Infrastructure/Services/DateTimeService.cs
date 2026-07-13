using HelpDesk.AuthService.Application.Common.Interfaces;

namespace HelpDesk.AuthService.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

