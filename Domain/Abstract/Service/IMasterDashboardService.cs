using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IMasterDashboardService : IBaseService<MasterDashboard>
  {
    // Task<int> ChangeStatus(MasterDashboard model);
    Task<int> ChangeEditableStatus(MasterDashboard model);
    Task<List<MasterDashboard>> GetRowsForLookupExcludeByID(string? keyword, int offset, int limit, string[] ID);
    Task<FileDoc> GetReportData();
  }
}