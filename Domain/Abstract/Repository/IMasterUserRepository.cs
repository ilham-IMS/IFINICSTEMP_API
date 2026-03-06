
using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
  public interface IMasterUserRepository : IBaseRepository<MasterUser>
  {
    Task<List<MasterUser>> GetReportData(IDbTransaction transaction);
  }
}