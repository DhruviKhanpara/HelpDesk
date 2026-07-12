namespace HelpDesk.TicketService.Infrastructure.Options;

public class AttachmentStorageOptions
{
    public const string SectionName = "AttachmentStorage";
    public string UploadPath { get; set; } = string.Empty;
    public List<string> AllowedExtensions { get; set; } = [];
    public int MaxFileSizeInMb { get; set; }
    public int MaxFiles { get; set; }
}
