using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class MasterApprovalCriteria : BaseModel
	{
		public string? ApprovalID { get; set; }
		public string? ReffCriteriaID { get; set; }
		public string? ReffCriteriaCode { get; set; }
		public string? ReffCriteriaName { get; set; }
		public string? CriteriaID { get; set; }
		public string? CriteriaCode { get; set; }
		public string? CriteriaDescription { get; set; }

		// Dari Hasil Get ke IFINSYS
		public string? CriteriaValue { get; set; }
	}
}