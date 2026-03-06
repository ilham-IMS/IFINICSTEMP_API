namespace Domain.Models
{
  public class SysJobTaskListLog : BaseModel
  {
    public string? JobTaskListCode { get; set; }
    public string? Status { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? LogDescription { get; set; }
    public string? RunBy { get; set; }
    public int? FromID { get; set; }
    public int? ToID { get; set; }
    public int? NumberOfRows { get; set; }
  }
}