namespace Domain.Models.Message
{
  public class MessagePaymentRequest : BaseModel
  {
    public string? Code { get; set; }
    public string? BranchID { get; set; }
    public string? BranchCode { get; set; }
    public string? BranchName { get; set; }
    public string? ClientName { get; set; }
    public string? PaymentSource { get; set; }
    public DateTime? PaymentRequestDate { get; set; }
    public string? PaymentSourceNo { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentCurrencyID { get; set; }
    public string? PaymentCurrencyCode { get; set; }
    public string? PaymentCurrencyDesc { get; set; }
    public decimal? PaymentAmount { get; set; }
    public string? PaymentRemarks { get; set; }
    public string? ToBankName { get; set; }
    public string? ToBankAccountNo { get; set; }
    public string? ToBankAccountName { get; set; }
    public string? PaymentTransactionCode { get; set; }
    public string? TaxType { get; set; }
    public string? TaxFileNo { get; set; }
    public string? TaxPayerReffCode { get; set; }
    public string? TaxFileName { get; set; }
    public string? ThirdPartyName { get; set; }
    public string? ModuleCode { get; set; }
    public List<MessagePaymentRequestDetail>? PaymentRequestDetail { get; set; }

  }
}