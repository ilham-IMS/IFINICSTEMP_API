using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportColumnService : IBaseService<DynamicReportColumn>
  {
    Task<List<DynamicReportColumn>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);
    Task<List<DynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);

    Task<int> UpdateByID(List<DynamicReportColumn> models);
    Task<int> Insert(List<DynamicReportColumn> models);
    Task<int> OrderUp(DynamicReportColumn model);
    Task<int> OrderDown(DynamicReportColumn model);

  }
}