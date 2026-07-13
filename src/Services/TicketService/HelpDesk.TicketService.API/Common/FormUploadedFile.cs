using HelpDesk.TicketService.Application.Common.Interfaces;

namespace HelpDesk.TicketService.API.Common;

public sealed class FormUploadedFile : IUploadedFile
{
    private readonly IFormFile _file;

    public FormUploadedFile(IFormFile file)
    {
        _file = file;
    }

    public string FileName => _file.FileName;

    public string ContentType => _file.ContentType;

    public long Length => _file.Length;

    public async Task CopyToAsync(Stream target, CancellationToken cancellationToken)
        => await _file.CopyToAsync(target, cancellationToken);
}