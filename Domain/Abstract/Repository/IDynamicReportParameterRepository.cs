using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IDynamicReportParameterRepository : IBaseRepository<DynamicReportParameter>
  {

    Task<List<DynamicReportParameter>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
    Task<List<DynamicReportParameter>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<List<DynamicReportParameter>> GetTopOrderByDynamicReport(IDbTransaction transaction, int limit, string dynamicReportID);
    Task<List<DynamicReportParameter>> GetRowsOrderByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<List<DynamicReportParameter>> GetRowsComponentByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<int> ChangeOrder(IDbTransaction transaction, DynamicReportParameter model);

  }
}