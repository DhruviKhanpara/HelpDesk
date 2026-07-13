using HelpDesk.TicketService.Application.Common.Models;

namespace HelpDesk.TicketService.Application.Common.Interfaces;

public interface IAttachmentStorageService
{
    Task<IReadOnlyList<AttachmentUploadResult>> UploadAsync(IEnumerable<IUploadedFile> files, CancellationToken cancellationToken);
    void Delete(IEnumerable<string> relativePaths);
    Stream OpenRead(string storagePath);
}
