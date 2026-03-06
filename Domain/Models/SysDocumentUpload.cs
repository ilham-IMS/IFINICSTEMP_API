namespace Domain.Models
{
  public class SysDocumentUpload : BaseModel
  {
    public string? ReffNo { get; set; }
    public string? ReffName { get; set; }
    public string? ReffTrxCode { get; set; }
    public string? FileName { get; set; }
    public string? MimeType { get; set; }
    public byte[]? DocFile { get; set; }
  }
}