using System.Data;
using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportParameterService : IBaseService<DynamicReportParameter>
  {
    Task<List<DynamicReportParameter>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);

    Task<List<DynamicReportParameter>> GetRowsComponentByDynamicReport(string dynamicReportID);
    Task<int> OrderUp(DynamicReportParameter model);
    Task<int> OrderDown(DynamicReportParameter model);
    Task<List<ExtendModel>> GetRowForParent(string ParentID);
  }
}