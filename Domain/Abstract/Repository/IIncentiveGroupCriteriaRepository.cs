using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveGroupCriteriaRepository : IBaseRepository<IncentiveGroupCriteria>
  {
    Task<List<IncentiveGroupCriteria>> GetRowsByGroupID(IDbTransaction transaction, string? keyword, int offset, int limit, string groupID);
    Task<int> CheckDuplicateCriteria(IDbTransaction transaction, string incentiveGroupID, string criteriaID, string exceptID);
  }
}