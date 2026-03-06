using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Message
{
    public class MessageMasterSurvey : BaseModel
    {
      public string? GroupOrEvidenceTypeID { get; set; }
      public string? GroupOrEvidenceTypeCode { get; set; }
      public string? GroupOrEvidenceTypeDescription { get; set; }
      public int? IsActive { get; set; }
      public string? Type { get; set; }
      public List<MessageMasterSurveyCriterias>? Criterias { get; set; }
    }
}