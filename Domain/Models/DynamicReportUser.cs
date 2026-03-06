using System.Text.Json;

namespace Domain.Models
{
  public class DynamicReportUser : BaseModel
  {
    public string? DynamicReportID { get; set; }
    public string? UserID { get; set; }
    public string? UserCode { get; set; }
    public string? UserName { get; set; }
	}
}


