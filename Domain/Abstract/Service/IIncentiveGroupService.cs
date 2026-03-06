using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IIncentiveGroupService : IBaseService<IncentiveGroup>
  {
    Task<int> ChangeStatus(IncentiveGroup model);
    Task<List<ExtendModel>> GetRowForParent(string ParentID);
  }
}