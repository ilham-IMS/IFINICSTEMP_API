using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstract.Service
{
    public interface IInterfaceMasterApprovalService : IBaseService<InterfaceMasterApproval>
    {
        Task<List<InterfaceMasterApproval>> GetRowsForJobIn(int limit);
        Task<int> UpdateAfterJobIn(InterfaceMasterApproval model);
    }
}
