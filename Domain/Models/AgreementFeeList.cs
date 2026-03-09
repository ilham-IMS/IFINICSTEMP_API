using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementFeeList : BaseModel
  {
    public int? FeeNo { get; set; }
    public string? FeeName { get; set; }
    public string? FeeAmount { get; set; }
    public string? FeeRate { get; set; }
  }
}