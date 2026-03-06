using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IMasterDynamicReportTableService : IBaseService<MasterDynamicReportTable>
  {
    Task<int> Insert(List<MasterDynamicReportTable> model);
    Task<List<MasterDynamicReportTable>> GetRowsForLookup(string? keyword, int offset, int limit);
    Task<FileDoc> GetReportData();
    Task<List<ExtendModel>> GetRowForParent(string ParentID);
  }
}