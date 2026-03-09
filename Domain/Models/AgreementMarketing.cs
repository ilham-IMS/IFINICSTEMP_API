using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementMarketing : BaseModel
  {
    public string? IncentiveMarketingID { get; set; }
    public string? AgreementID { get; set; }
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
    public decimal? BPERatio { get; set; }
    public decimal? InsurancePremiumUsageRatio { get; set; }
    public decimal? BPEEffect { get; set; }
    public decimal? BPEIncome { get; set; }
    public decimal? IncentiveExpense { get; set; }
    public decimal? NonInterestEffect { get; set; }
    public decimal? ProfitBeforeMarketingIncentive { get; set; }
    public decimal? IncentiveAmount { get; set; }
    public decimal? MarketingIncentiveRatio { get; set; }
    public decimal? FinanceAmount { get; set; }
    public decimal? InsuranceRate { get; set; }
    public decimal? InterestAmount { get; set; }
    public decimal? CostAmount { get; set; }
    public decimal? InterestMarginAmount { get; set; }
    public decimal? CCYRate { get; set; }
    public decimal? BPETotal { get; set; }
    public decimal? BPETotalAmount { get; set; }
    public string? NonInterestName { get; set; }
    public decimal? NonInterestExpense { get; set; }
    public decimal? NonInterestEffectAmount { get; set; }

    // Calculated Fields
    public decimal? InterestMarginProfitBeforeMarketingIncentive { get; set; }
    public decimal? MarketingIncentiveRatioIncentiveAmount { get; set; }
    public decimal? MarketingIncentiveRatioFinanceAmount { get; set; }
    public decimal? BPEIncomeIncentiveExpense { get; set; }
    public decimal? NetInterestMarginAfterCost { get; set; }

    //Print
      public string? MimeType { get; set; }
      public IFormFile? File { get; set; }
      public string? CompanyFileName { get; set; }
      public string? CompanyName { get; set; }
  }
}