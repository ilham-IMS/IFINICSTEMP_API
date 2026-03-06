namespace Domain.Models
{
  public class SysJobTasklist : BaseModel
  {
    public string? Code { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? SpName { get; set; }
    public string? OrderNo { get; set; }
    public int? IsActive { get; set; }
    public int? RowToProcess { get; set; }
    public int? LastID { get; set; }
    public string? WhenError { get; set; }
    public string? EodStatus { get; set; }
    public string? EodRemark { get; set; }
  }
}