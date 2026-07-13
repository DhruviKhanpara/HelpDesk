using HelpDesk.TicketService.Application.Common.Interfaces;
using HelpDesk.TicketService.Application.Common.Models;
using HelpDesk.TicketService.Infrastructure.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace HelpDesk.TicketService.Infrastructure.Storage;

internal sealed class AttachmentStorageService : IAttachmentStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly AttachmentStorageOptions _options;

    public AttachmentStorageService(IWebHostEnvironment environment, IOptions<AttachmentStorageOptions> options)
    {
        _environment = environment;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<AttachmentUploadResult>> UploadAsync(IEnumerable<IUploadedFile> files, CancellationToken cancellationToken)
    {
        var uploadedFiles = new List<AttachmentUploadResult>();

        var fileList = files?.ToList() ?? [];

        if (!fileList.Any())
            return uploadedFiles;

        if (fileList.Count > _options.MaxFiles)
            throw new InvalidOperationException($"Maximum {_options.MaxFiles} attachments are allowed.");

        var uploadDirectory = Path.Combine(_environment.WebRootPath, _options.UploadPath.Replace('/', Path.DirectorySeparatorChar));

        Directory.CreateDirectory(uploadDirectory);

        foreach (var file in fileList)
        {
            ValidateFile(file);

            var extension = Path.GetExtension(file.FileName);

            var storedFileName = $"{Guid.NewGuid():N}{extension}";

            var fullPath = Path.Combine(uploadDirectory, storedFileName);

            await using var fileStream = new FileStream(
                fullPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);

            await file.CopyToAsync(fileStream, cancellationToken);

            uploadedFiles.Add(new AttachmentUploadResult
            {
                OriginalFileName = file.FileName,
                StoredFileName = storedFileName,
                RelativePath = Path.Combine(_options.UploadPath, storedFileName)
                    .Replace(Path.DirectorySeparatorChar, '/'),
                ContentType = file.ContentType,
                FileSize = file.Length
            });
        }

        return uploadedFiles;
    }

    public void Delete(IEnumerable<string> relativePaths)
    {
        foreach (var relativePath in relativePaths)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }

    private void ValidateFile(IUploadedFile file)
    {
        if (file.Length <= 0)
            throw new InvalidOperationException("Attachment cannot be empty.");

        if (file.Length > _options.MaxFileSizeInMb * 1024 * 1024)
            throw new InvalidOperationException($"Attachment exceeds the maximum allowed size of {_options.MaxFileSizeInMb} MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_options.AllowedExtensions.Contains(extension))
            throw new InvalidOperationException($"Files with extension '{extension}' are not allowed.");
    }
}