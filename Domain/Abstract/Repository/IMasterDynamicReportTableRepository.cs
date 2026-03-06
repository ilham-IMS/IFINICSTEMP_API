using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IMasterDynamicReportTableRepository : IBaseRepository<MasterDynamicReportTable>
  {
    Task<List<MasterDynamicReportTable>> GetRowsForLookup(IDbTransaction transaction, string? keyword, int offset, int limit);
    Task<List<MasterDynamicReportTable>> GetReportData(IDbTransaction transaction);
  }
}