using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Abstract.Repository
{
	public interface IMasterApprovalCriteriaRepository : IBaseRepository<MasterApprovalCriteria>
	{
		Task<List<MasterApprovalCriteria>> GetRows(IDbTransaction transaction, string keyword, int offset, int limit, string MasterApprovalID);
		Task<int> UpdateReferenceByID(IDbTransaction transaction, MasterApprovalCriteria model);
		Task<int> DeleteByNotInReffCriteria(IDbTransaction transaction, string[] reffCriteriaID, string MasterApprovalId);
		Task<int> DeleteAllByApprovalID(IDbTransaction transaction, string ApprovalID);
		Task<List<MasterApprovalCriteria>> GetRowsByReffCriteria(IDbTransaction transaction, string[] reffCriteriaID, string MasterApprovalId);
		Task<List<MasterApprovalCriteria>> GetRowsByApprovalCode(IDbTransaction transaction, string approvalCode);
	}
}