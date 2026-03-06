using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IDynamicReportRepository : IBaseRepository<DynamicReport>
  {
		Task<List<DynamicReport>> GetRowsPublishedByUser(IDbTransaction transaction, string? keyword, int offset, int limit, string userCode);
    Task<List<IDictionary<string, object?>>> GetDataFromQuery(IDbTransaction transaction, string query, IDictionary<string, object> parameters);
    Task<string> GetQuery(IDbTransaction transaction, string ID);
    Task<int> GetPublishedStatus(IDbTransaction transaction, string id);
    Task<int> ChangePublishedStatus(IDbTransaction transaction, DynamicReport model);
    Task<int> UpdateQuery(IDbTransaction transaction, DynamicReport model);
    Task<List<DynamicReport>> GetReportData(IDbTransaction transaction);
  }
}