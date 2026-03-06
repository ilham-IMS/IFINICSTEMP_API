using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IIncentiveSchemeService : IBaseService<IncentiveScheme>
  {
    Task<int> ChangeStatus(IncentiveScheme model);
    Task<List<ExtendModel>> GetRowForParent(string ParentID);
  }
}