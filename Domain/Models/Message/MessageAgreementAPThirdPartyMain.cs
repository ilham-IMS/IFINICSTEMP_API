namespace Domain.Models.Message
{
  public class MessageAgreementAPThirdPartyMain : BaseModel
  {
    public string? Code { get; set; }
    public string? AgreementID { get; set; }
    public string? AgreementNo { get; set; }
    public string? APType { get; set; }
    public string? APCurrencyID { get; set; }
    public string? APCurrencyCode { get; set; }
    public string? APCurrencyDesc { get; set; }
    public decimal? APAmount { get; set; }
    public List<MessageAgreementAPThirdPartyHistory>? AgreementAPThirdPartyHistory { get; set; }
  }
}