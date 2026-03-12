using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementReferralList : BaseModel
  {
    public int? ReferralNo { get; set; }
    public string? ReferralName { get; set; }
    public string? ReferralAmount { get; set; }
    public string? ReferralRate { get; set; }
  }
}