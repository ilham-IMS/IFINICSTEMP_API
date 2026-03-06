namespace Domain.Models.Message
{
  public class MessageAgreementAPThirdPartyHistory : BaseModel
  {
    public string? APThirdPartyID { get; set; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? InputDate { get; set; }
    public decimal? OrigAmount { get; set; }
    public string? OrigCurrencyID { get; set; }
    public string? OrigCurrencyCode { get; set; }
    public string? OrigCurrencyDesc { get; set; }
    public decimal? ExchRate { get; set; }
    public decimal? BaseAmount { get; set; }
    public string? SourceReffModule { get; set; }
    public string? SourceReffCode { get; set; }
    public string? SourceReffName { get; set; }
    public string? Remarks { get; set; }
    public string? ThirdPartyName { get; set; }
  }
}