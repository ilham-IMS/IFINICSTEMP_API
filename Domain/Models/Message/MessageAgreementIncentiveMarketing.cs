namespace Domain.Models.Message
{
    public class MessageAgreementIncentiveMarketing : BaseModel
    {
      public string? ApplicationMainID { get; set; }
      public string? AgreementNo { get; set; }
      public string? ClientID { get; set; }
      public string? ClientNo { get; set; }
      public string? ClientName { get; set; }
      public DateTime? ApprovedDate { get; set; }
      public DateTime? DisbursementDate { get; set; }
      public int? IncentivePeriod { get; set; }
      public string? PaymentMethod { get; set; }
      public string? CurrencyID { get; set; }
      public string? CurrencyCode { get; set; }
      public string? CurrencyDesc { get; set; }
      public decimal? NetFinance { get; set; }
      public decimal? InterestRate { get; set; }
      public decimal? CostRate { get; set; }
      public decimal? InterestMargin { get; set; }
      public decimal? InsuranceRate { get; set; }
      public decimal? InterestAmount { get; set; }
      public decimal? CostAmount { get; set; }
      public decimal? InterestMarginAmount { get; set; }
      public decimal? TotalInsurancePremiAmount { get; set; }
      public decimal? CCYRate { get; set; }
      public decimal? CommissionRate { get; set; }
      public string? VendorID { get; set; }
      public string? VendorCode { get; set; }
      public string? VendorName { get; set; }
      public string? AgentID { get; set; }
      public string? AgentCode { get; set; }
      public string? AgentName { get; set; }

      public List<MessageAgreementFee>? AgreementFees { get; set; }
      public List<MessageAgreementRefund>? AgreementRefunds { get; set; }

    }
}