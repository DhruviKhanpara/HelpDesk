using HelpDesk.TicketService.Application.Common.Exceptions;
using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Domain.Constants;
using HelpDesk.TicketService.Domain.Entities;
using HelpDesk.TicketService.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.TicketService.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IAttachmentStorageService _attachmentStorageService;

    public CreateTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IAttachmentStorageService attachmentStorageService)
    {
        _context = context;
        _currentUser = currentUser;
        _attachmentStorageService = attachmentStorageService;
    }

    public async Task<CreateTicketResponse> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;

        var categoryExists = await _context.Categories
            .AnyAsync(x => x.Id == model.CategoryId && x.IsActive, cancellationToken);

        if (!categoryExists)
            throw new NotFoundException("Ticket category", model.CategoryId);

        var ticket = new TicketEntity
        {
            Title = model.Title.Trim(),
            Description = model.Description.Trim(),

            CategoryId = model.CategoryId,
            StatusId = StatusIds.Open,
            PriorityId = PrioritiesIds.Medium,

            RequesterUserId = _currentUser.User.UserId,
            AssignedUserId = null
        };

        var uploadedFiles = new List<string>();

        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        try
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            ticket.AssignTicketNumber();

            if (model.Attachments?.Any() == true)
            {
                var uploads = await _attachmentStorageService.UploadAsync(model.Attachments, cancellationToken);

                foreach (var upload in uploads)
                {
                    uploadedFiles.Add(upload.RelativePath);

                    ticket.Attachments.Add(new TicketAttachmentEntity
                    {
                        OriginalFileName = upload.OriginalFileName,
                        StoredFileName = upload.StoredFileName,
                        StoragePath = upload.RelativePath,
                        ContentType = upload.ContentType,
                        FileSize = upload.FileSize
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            if (uploadedFiles.Any())
            {
                _attachmentStorageService.Delete(uploadedFiles);
            }

            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return new CreateTicketResponse
        {
            TicketId = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Status = (TicketStatus)ticket.StatusId
        };
    }
}
