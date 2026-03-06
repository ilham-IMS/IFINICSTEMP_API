using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
    public interface ISysJobTaskRepository : IBaseRepository<SysJobTask>
    {
        Task<SysJobTask> GetRowByCode(IDbTransaction transaction, string code);
        Task<int> UpdateJobStatus(IDbTransaction transaction, SysJobTask model);
    }
}