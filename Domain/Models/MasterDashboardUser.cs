namespace Domain.Models
{
  public class MasterDashboardUser : BaseModel
  {
    public string? UserID { get; set; }
    public string? MasterDashboardID { get; set; }
    public int? OrderKey { get; set; }

    //from master_user
    public string? EmployeeCode { get; set; }
    //  from master_dashboard
    public string? DashboardName { get; set; }
    public string? DashboardGrid { get; set; }
  }
}