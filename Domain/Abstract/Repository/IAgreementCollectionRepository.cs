using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IAgreementCollectionRepository : IBaseRepository<AgreementCollection>
  {
    Task<List<AgreementCollection>> GetRowsByIncentiveID(IDbTransaction transaction, string? keyword, int offset, int limit, string incentiveMarketingID);
  }
}