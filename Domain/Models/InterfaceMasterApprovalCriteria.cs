using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
  public class InterfaceMasterApprovalCriteria : BaseModel
  {
    public string? InterfaceApprovalID { get; set; }
    public string? CriteriaID { get; set; }
    public string? CriteriaCode { get; set; }
    public string? CriteriaDescription { get; set; }
    public string? JobStatus { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
    
  }
}