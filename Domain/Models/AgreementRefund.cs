using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementRefund : BaseModel
  {
    public string? AgreementIncentiveID { get; set; }
    public string? RefundID { get; set; }
    public string? RefundCode { get; set; }
    public string? RefundDesc { get; set; }
    public decimal? RefundAmount { get; set; }
    public decimal? RefundRate { get; set; }
    public string? CalculateBy { get; set; }

    // Agreement Incentive Marketing
    public string? VendorName { get; set; }
    public string? AgentName { get; set; }
  }
}