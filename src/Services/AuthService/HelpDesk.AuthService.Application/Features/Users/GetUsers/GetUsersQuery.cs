using HelpDesk.AuthService.Application.Common.Models;
using MediatR;

namespace HelpDesk.AuthService.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery(
    string? Search,
    long? RoleId,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<UserListItemResponse>>;
