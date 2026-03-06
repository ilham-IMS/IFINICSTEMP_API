using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportColumnOrderService : IBaseService<DynamicReportColumnOrder>
  {
    Task<List<DynamicReportColumnOrder>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);
    Task<int> UpdateByID(List<DynamicReportColumnOrder> models);
    Task<int> Insert(List<DynamicReportColumnOrder> models);
    Task<int> OrderUp(DynamicReportColumnOrder model);
    Task<int> OrderDown(DynamicReportColumnOrder model);

  }
}