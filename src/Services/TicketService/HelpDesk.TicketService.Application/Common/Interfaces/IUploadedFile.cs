namespace HelpDesk.TicketService.Application.Common.Interfaces;

public interface IUploadedFile
{
    string FileName { get; }
    string ContentType { get; }
    long Length { get; }
    Task CopyToAsync(Stream target, CancellationToken cancellationToken);
}
