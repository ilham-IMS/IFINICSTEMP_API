namespace Domain.Models
{
  public class InterfaceAgreementFee : BaseModel
  {
    public string? AgreementIncentiveID { get; set; }
    public string? FeeID { get; set; }
    public string? FeeCode { get; set; }
    public string? FeeName { get; set; }
    public decimal? FeeAmount { get; set; }
    public decimal? FeeRate { get; set; }
    public string? FeePaymentType { get; set; }
    public decimal? FeePaidAmount { get; set; }
    public decimal? FeeReduceDisburseAmount { get; set; }
    public decimal? FeeCapitalizeAmount { get; set; }
    public string? InsuranceYear { get; set; }
    public string? Remarks { get; set; }
    public int? IsInternalIncome { get; set; }
    public string? JobStatus { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
  }
}