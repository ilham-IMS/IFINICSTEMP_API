using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface ISysJobTaskListRepository : IBaseRepository<SysJobTasklist>
  {
    Task<SysJobTasklist> GetCodeGenerate(IDbTransaction transaction, int offset, int limit);
  }
}