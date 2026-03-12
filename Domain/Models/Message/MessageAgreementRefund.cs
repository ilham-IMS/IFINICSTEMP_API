namespace Domain.Models
{
  public class MessageAgreementRefund : BaseModel
  {
    public string? AgreementIncentiveID { get; set; }
    public string? RefundID { get; set; }
    public string? RefundCode { get; set; }
    public string? RefundName { get; set; }
    public decimal? RefundAmount { get; set; }
    public decimal? RefundRate { get; set; }
    public string? CalculateBy { get; set; }
  }
}