using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IIncentiveGroupCriteriaService : IBaseService<IncentiveGroupCriteria>
  {
    Task<List<IncentiveGroupCriteria>> GetRowsByGroupID(string? keyword, int offset, int limit, string groupID);
    Task<int> UpdateByID(List<IncentiveGroupCriteria> modelList);
  }
}