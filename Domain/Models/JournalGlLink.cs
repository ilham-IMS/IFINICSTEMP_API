
namespace Domain.Models
{
  public class JournalGlLink : BaseModel
  {
    public string? Code { get; set; }
    public string? GlLinkName { get; set; }
    public int? IsActive { get; set; }
  }
}