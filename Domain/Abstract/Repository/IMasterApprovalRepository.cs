using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Abstract.Repository
{
	public interface IMasterApprovalRepository : IBaseRepository<MasterApproval>
	{
		Task<int> ChangeIsActive(IDbTransaction transaction, MasterApproval model);
		Task<MasterApproval> CheckCode(IDbTransaction transaction, string Code);
		Task<MasterApproval> CheckName(IDbTransaction transaction, string ApprovalName);
		Task<MasterApproval> CheckNameForUpdate(IDbTransaction transaction, string ApprovalName, string ID);
		Task<MasterApproval> GetRowByCode(IDbTransaction transaction, string code);
    Task<MasterApproval> CountData(IDbTransaction transaction, string reffApprovalCategoryID);
	}
}