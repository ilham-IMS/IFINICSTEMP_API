using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Message
{
    public class MessageMasterApproval : BaseModel
    {
      public string? ApprovalCategoryID { get; set; }
      public string? ApprovalCategoryCode { get; set; }
      public string? ApprovalCategoryName { get; set; }
      public string? ReffModuleCode { get; set; }
      public List<MessageMasterApprovalCriteria>? Criterias { get; set; }
    }
}