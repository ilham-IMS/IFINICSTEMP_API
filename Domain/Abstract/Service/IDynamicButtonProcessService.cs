using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IDynamicButtonProcessService : IBaseService<DynamicButtonProcess>
  {
    Task<List<DynamicButtonProcess>> GetRows(string? keyword, int offset, int limit, string ParentMenuID);
    Task<int> SyncButtonProcess(List<DynamicButtonProcess> model);
    Task<List<DynamicButtonProcess>> GetRowsForLookupParent(string? keyword, int offset, int limit, bool withAll = true);


  }
}
