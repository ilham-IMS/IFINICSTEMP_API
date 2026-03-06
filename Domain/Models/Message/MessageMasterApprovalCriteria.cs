using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Message
{
    public class MessageMasterApprovalCriteria : BaseModel
    {
      public string? CriteriaID { get; set; }
      public string? CriteriaCode { get; set; }
      public string? CriteriaDescription { get; set; }
    }
}