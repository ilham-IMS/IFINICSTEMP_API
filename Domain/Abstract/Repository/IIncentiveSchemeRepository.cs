using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveSchemeRepository : IBaseRepository<IncentiveScheme>
  {
    Task<int> ChangeStatus(IDbTransaction transaction, IncentiveScheme model);
  }
}