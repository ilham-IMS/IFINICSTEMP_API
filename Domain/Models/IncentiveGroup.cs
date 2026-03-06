namespace Domain.Models
{
  public class IncentiveGroup : ExtendModel
  {  
    public string? IncentiveType { get; set; }
    public string? GroupDescription { get; set; }
    public int? IsActive { get; set; }
  }
}