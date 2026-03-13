using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementCommissionList : BaseModel
  {
    public int? CommNo { get; set; }
    public string? CommName { get; set; }
    public string? CommAmount { get; set; }
    public string? CommRate { get; set; }

    public List<AgreementReferralList>? ReffList { get; set; }
  }
}