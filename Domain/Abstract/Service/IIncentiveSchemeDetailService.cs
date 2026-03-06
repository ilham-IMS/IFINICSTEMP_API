using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IIncentiveSchemeDetailService : IBaseService<IncentiveSchemeDetail>
  {
    Task<int> UpdateByID(List<IncentiveSchemeDetail> modelList);
    Task<List<IncentiveSchemeDetail>> GetRowsBySchemeID(string? keyword, int offset, int limit, string schemeID);
  }
}