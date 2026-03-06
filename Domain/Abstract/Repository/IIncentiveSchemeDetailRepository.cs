using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveSchemeDetailRepository : IBaseRepository<IncentiveSchemeDetail>
  {
    Task<List<IncentiveSchemeDetail>> GetRowsBySchemeID(IDbTransaction transaction, string? keyword, int offset, int limit, string schemeID);
  }
}