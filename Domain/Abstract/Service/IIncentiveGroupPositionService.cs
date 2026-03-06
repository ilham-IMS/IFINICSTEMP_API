using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IIncentiveGroupPositionService : IBaseService<IncentiveGroupPosition>
  {
    Task<List<IncentiveGroupPosition>> GetRowsByGroupID(string? keyword, int offset, int limit, string groupID);
    Task<int> UpdateByID(List<IncentiveGroupPosition> modelList);
  }
}