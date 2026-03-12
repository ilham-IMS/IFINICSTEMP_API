using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveGroupRepository : IBaseRepository<IncentiveGroup>
  {
    Task<int> ChangeStatus(IDbTransaction transaction, IncentiveGroup model);
    Task<int> CountExistingGroup(IDbTransaction transaction, string incentiveType, string groupDescription);
  }
}