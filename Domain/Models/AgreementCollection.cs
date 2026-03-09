using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class AgreementCollection : BaseModel
  { 
    public string? IncentiveCollectionID { get; set; }
    public string? AgreementID { get; set; }
    public string? AgreementNo { get; set; }
    public string? ClientID { get; set; }
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public string? CollectorID { get; set; }
    public string? CollectorCode { get; set; }
    public string? CollectorName { get; set; }
    public string? MarketingID { get; set; }
    public string? MarketingCode { get; set; }
    public string? MarketingName { get; set; }
    public int? IncentivePeriod { get; set; }
    public int? InstallmentNo { get; set; }
    public decimal? UnpaidAmount { get; set; }
    public DateTime? UnpaidDate { get; set; }
    public DateTime? StartingDateHandling { get; set; }
    public DateTime? DeadlineDateHandling { get; set; }
    public string? IncentiveResult { get; set; }
    public decimal? CollectedAmount { get; set; }
    public decimal? PaidCase { get; set; }
    public decimal? CollectedPct { get; set; }
    public decimal? IncentivePct { get; set; }
    public decimal? IncentiveAmount { get; set; }
    public string? Remarks { get; set; }    
  }
}