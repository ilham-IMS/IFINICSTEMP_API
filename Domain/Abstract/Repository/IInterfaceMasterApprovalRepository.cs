using Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Domain.Abstract.Repository
{
    public interface IInterfaceMasterApprovalRepository : IBaseRepository<InterfaceMasterApproval>
    {
        Task<List<InterfaceMasterApproval>> GetRowsForJobIn(IDbTransaction transaction, int limit);
        Task<int> UpdateAfterJobIn(IDbTransaction transaction, InterfaceMasterApproval model);
        Task<int> CountDataByCode(IDbTransaction transaction, string code);
    }
}
