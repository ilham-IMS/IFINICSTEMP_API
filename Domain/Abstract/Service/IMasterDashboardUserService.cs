using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IMasterDashboardUserService : IBaseService<MasterDashboardUser>
  {
    Task<List<MasterDashboardUser>> GetRows(string? keyword, int offset, int limit, string userID);
    Task<int> Insert(List<MasterDashboardUser> models);
    Task<int> UpdateByID(List<MasterDashboardUser> models);
  }
}