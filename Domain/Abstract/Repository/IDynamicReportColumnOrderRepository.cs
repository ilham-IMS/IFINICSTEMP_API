using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IDynamicReportColumnOrderRepository : IBaseRepository<DynamicReportColumnOrder>
  {
    Task<List<DynamicReportColumnOrder>> GetTopOrderByDynamicReport(IDbTransaction transaction, int limit, string dynamicReportID);
    Task<List<DynamicReportColumnOrder>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
    Task<List<DynamicReportColumnOrder>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<List<DynamicReportColumnOrder>> GetRowsOrderByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<int> ChangeOrder(IDbTransaction transaction, DynamicReportColumnOrder model);
    Task<int> GetCountByDynamicReport(IDbTransaction transaction, string dynamicReportID);

  }
}