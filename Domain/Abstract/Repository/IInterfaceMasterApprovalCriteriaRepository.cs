using Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Domain.Abstract.Repository
{
    public interface IInterfaceMasterApprovalCriteriaRepository : IBaseRepository<InterfaceMasterApprovalCriteria>
    {
      Task<List<InterfaceMasterApprovalCriteria>> GetRows(IDbTransaction transaction, string iApprovalID);
    }
}
