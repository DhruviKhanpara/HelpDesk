using HelpDesk.AuthService.Application.Common.Exceptions;
using HelpDesk.AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.Id == request.UserId && !u.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException("User", request.UserId);

        return new GetUserByIdResponse
        {
            UserId = user.Id,
            EmployeeCode = user.EmployeeCode,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            LastLoginDate = user.LastLoginDate,
            CreatedDate = user.CreatedDate,
            Roles = user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList()
        };
    }
}
