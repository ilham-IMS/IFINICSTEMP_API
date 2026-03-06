namespace Domain.Models
{
  public class IncentiveGroupCriteria : ExtendModel
  {  
    public string? IncentiveGroupID { get; set; }
    public string? CriteriaID { get; set; }
    public string? CriteriaCode { get; set; }
    public string? CriteriaDescription { get; set; }
    public string? CriteriaOperator { get; set; }
    public string? ValueFrom { get; set; }
    public string? ValueTo { get; set; }
  }
}