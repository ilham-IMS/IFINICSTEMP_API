using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IDynamicReportColumnRepository : IBaseRepository<DynamicReportColumn>
  {
    Task<List<DynamicReportColumn>> GetTopOrderByDynamicReport(IDbTransaction transaction, int limit, string dynamicReportID);
    Task<List<DynamicReportColumn>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
    Task<List<DynamicReportColumn>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<List<DynamicReportColumn>> GetRowsOrderByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<List<DynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
    Task<int> ChangeOrder(IDbTransaction transaction, DynamicReportColumn model);
    Task<int> GetCountByDynamicReport(IDbTransaction transaction, string dynamicReportID);

		Task<int> DeleteFormulaByTable(IDbTransaction transaction, string dynamicReportID, string tableAlias);

  }
}