using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportTableService : IBaseService<DynamicReportTable>
  {
    Task<List<DynamicReportTable>> GetRows(string? keyword, int offset, int limit, string dynamicReportID);
    Task<List<DynamicReportTable>> GetRowsExclude(string? keyword, int offset, int limit, string dynamicReportTableID);
    Task<List<ExtendModel>> GetRowForParent(string ParentID);
  }
}