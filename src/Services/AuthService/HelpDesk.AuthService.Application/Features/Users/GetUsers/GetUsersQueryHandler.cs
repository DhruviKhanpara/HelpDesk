using HelpDesk.AuthService.Application.Common.Interfaces;
using HelpDesk.AuthService.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.AuthService.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserListItemResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<UserListItemResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted)
            .AsNoTracking();

        if (request.IsActive.HasValue)
            query = query.Where(u => u.IsActive == request.IsActive.Value);

        if (request.RoleId.HasValue)
            query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == request.RoleId.Value));

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(u =>
                u.FirstName.Contains(search) ||
                u.LastName.Contains(search) ||
                u.Email.Contains(search) ||
                u.EmployeeCode.Contains(search));
        }

        var projected = query
            .OrderByDescending(u => u.CreatedDate)
            .Select(u => new UserListItemResponse
            {
                UserId = u.Id,
                EmployeeCode = u.EmployeeCode,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                LastLoginDate = u.LastLoginDate,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            });

        return await PaginatedList<UserListItemResponse>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}
