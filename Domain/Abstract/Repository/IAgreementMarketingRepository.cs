using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IAgreementMarketingRepository : IBaseRepository<AgreementMarketing>
  {
    Task<List<AgreementMarketing>> GetRowsByIncentiveID(IDbTransaction transaction, string? keyword, int offset, int limit, string incentiveMarketingID);
  }
}