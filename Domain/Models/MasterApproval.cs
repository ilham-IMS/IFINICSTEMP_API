namespace Domain.Models
{
  public class MasterApproval : BaseModel
  {
    public string? Code { get; set; }
    public string? ApprovalName { get; set; }
    public string? ReffApprovalCategoryID { get; set; }
    public string? ReffApprovalCategoryCode { get; set; }
    public string? ReffApprovalCategoryName { get; set; }
    public int? IsActive { get; set; }
    public int? CountCode { get; set; }
    public int? CountName { get; set; }
    public int? CountNameForUpdate { get; set; }
  }
}