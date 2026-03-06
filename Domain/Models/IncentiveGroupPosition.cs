namespace Domain.Models
{
  public class IncentiveGroupPosition : BaseModel
  {  
    public string? IncentiveGroupID { get; set; }
    public string? PositionID { get; set; }
    public string? PositionCode { get; set; }
    public string? PositionDescription { get; set; }
    public decimal? PositionRatio { get; set; }
  }
}