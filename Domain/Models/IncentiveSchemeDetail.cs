namespace Domain.Models
{
  public class IncentiveSchemeDetail : BaseModel
  {  
    public string? IncentiveSchemeID { get; set; }
    public decimal? FromRate { get; set; }
    public decimal? ToRate { get; set; }
    public decimal? IncentiveRate { get; set; }
  }
}