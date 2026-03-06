
using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IMasterDashboardUserRepository : IBaseRepository<MasterDashboardUser>
  {
    Task<List<MasterDashboardUser>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string userID);
  }
}