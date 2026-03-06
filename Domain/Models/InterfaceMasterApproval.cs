using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
  public class InterfaceMasterApproval : BaseModel
  {
    public string? ApprovalCategoryID { get; set; }
    public string? ApprovalCategoryCode { get; set; }
    public string? ApprovalCategoryName { get; set; }
    public string? ReffModuleCode { get; set; }
    public List<InterfaceMasterApprovalCriteria>? Criterias { get; set; }
    public string? JobStatus { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
    
  }
}