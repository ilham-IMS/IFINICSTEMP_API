namespace Domain.Models
{
  public class IncentiveScheme : ExtendModel
  {  
    public string? IncentiveType { get; set; }
    public DateTime? EffDate { get; set; }
    public int? IsActive { get; set; }
    public decimal? IncentiveRatio { get; set; }
    public decimal? RatePenalty { get; set; }
    public int? OverdueDaysFrom { get; set; }
    public int? OverdueDaysTo { get; set; }
    public int? MinimumAmount { get; set; }
    public int? MaximumAmount { get; set; }
  }
}