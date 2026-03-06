using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveGroupPositionRepository : IBaseRepository<IncentiveGroupPosition>
  {
    Task<List<IncentiveGroupPosition>> GetRowsByGroupID(IDbTransaction transaction, string? keyword, int offset, int limit, string groupID);
    Task<int> CheckDuplicatePosition(IDbTransaction transaction, string incentiveGroupID, string positionID, string exceptID);
  }
}