using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IMasterDashboardRepository : IBaseRepository<MasterDashboard>
  {
    // Task<int> ChangeStatus(IDbTransaction transaction, MasterDashboard model);
    Task<int> ChangeEditableStatus(IDbTransaction transaction, MasterDashboard model);
    Task<List<MasterDashboard>> GetRowsForLookupExcludeByID(IDbTransaction transaction, string? keyword, int offset, int limit, string[] ID);
    Task<List<MasterDashboard>> GetReportData(IDbTransaction transaction);
  }
}
