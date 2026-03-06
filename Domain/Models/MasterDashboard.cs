namespace Domain.Models
{
  public class MasterDashboard : BaseModel
  {
    public string? Code { get; set; }
    public string? DashboardName { get; set; }
    public string? DashboardType { get; set; }
    
    public string? SpName { get; set; }
    public string? DashboardGrid { get; set; }
    public int? IsActive { get; set; }
    public int? IsEditable { get; set; }
  }
}