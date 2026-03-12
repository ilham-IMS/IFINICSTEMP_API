using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementIncentiveMarketing : BaseModel
  {
    public string? IncentiveMarketingID { get; set; }
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
    public decimal? BPETotalAmount { get; set; }
    public decimal? BPETotal { get; set; }
    public decimal? BPERatio { get; set; }
    public decimal? BPEIncomeIncentiveExpense { get; set; }
    public decimal? BPEEffect { get; set; }
    public decimal? NonInterestExpense { get; set; }
    public decimal? NonInterestIncome { get; set; }
    public decimal? NonInterestEffectAmount { get; set; }
    public decimal? NonInterestEffect { get; set; }
    public decimal? MarketingIncentiveRatio { get; set; }
    public decimal? MarketingIncentiveRatioInterest { get; set; }
    public decimal? MarketingIncentiveRatioFinance { get; set; }
    public decimal? NetInterestMarginAfterCost { get; set; }
    public decimal? NetInterestMargin { get; set; }
    public decimal? InsurancePremiumUsageRatio { get; set; }
    public decimal? ProfitBeforeMarketingIncentive { get; set; }
    public decimal? TotalInterestMargin { get; set; }

    //Calculation
    public decimal? TotalRefundAmount { get; set; }

    //Company Info
    public string? CompanyFileName { get; set; }
    public string? CompanyName { get; set; }
  }
}