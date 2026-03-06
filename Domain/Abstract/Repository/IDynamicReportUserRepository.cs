using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IDynamicReportUserRepository : IBaseRepository<DynamicReportUser>
  {
    Task<List<DynamicReportUser>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
  }
}