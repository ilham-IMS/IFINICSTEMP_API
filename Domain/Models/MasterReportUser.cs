namespace Domain.Models
{
  public class MasterReportUser : BaseModel
  {
    public string? EmployeeID { get; set; }
    public string? ReportID { get; set; }
    //table sys report
    public string? ReportName { get; set; }
    public string? ReportType { get; set; }
  }
}