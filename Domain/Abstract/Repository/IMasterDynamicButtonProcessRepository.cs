using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IMasterDynamicButtonProcessRepository : IBaseRepository<MasterDynamicButtonProcess>
  {
    Task<List<MasterDynamicButtonProcess>> GetRowsForLookup(IDbTransaction transaction, string? keyword, int offset, int limit);
    Task<int?> CountByShortDesc(IDbTransaction transaction, string shortDescription, string ID);
    Task<int?> CountByDescription(IDbTransaction transaction, string description, string ID);
  }
}
