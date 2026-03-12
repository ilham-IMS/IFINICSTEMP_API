using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class InterfaceAgreementRefund : BaseModel
  {
    public string? AgreementIncentiveID { get; set; }
    public string? RefundID { get; set; }
    public string? RefundCode { get; set; }
    public string? RefundDesc { get; set; }
    public decimal? RefundAmount { get; set; }
    public decimal? RefundRate { get; set; }
    public string? CalculateBy { get; set; }
    public string? JobStatus { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
  }
}