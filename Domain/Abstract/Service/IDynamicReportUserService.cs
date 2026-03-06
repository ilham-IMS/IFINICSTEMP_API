using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportUserService : IBaseService<DynamicReportUser>
  {
    Task<List<DynamicReportUser>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);
    Task<int> Insert(List<DynamicReportUser> models);
  }
}