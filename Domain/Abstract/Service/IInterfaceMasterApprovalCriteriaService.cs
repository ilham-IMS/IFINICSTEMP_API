using System.Data;
using Domain.Models;

namespace Domain.Abstract.Service
{
    public interface IInterfaceMasterApprovalCriteriaService : IBaseService<InterfaceMasterApprovalCriteria>
    { 
      Task<List<InterfaceMasterApprovalCriteria>> GetRows(IDbTransaction transaction, string iApprovalID);
    }
}
