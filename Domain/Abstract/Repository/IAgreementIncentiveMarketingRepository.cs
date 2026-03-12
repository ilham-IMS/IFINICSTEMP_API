using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IAgreementIncentiveMarketingRepository : IBaseRepository<AgreementIncentiveMarketing>
  {
    Task<List<AgreementIncentiveMarketing>> GetRowsByIncentiveID(IDbTransaction transaction, string? keyword, int offset, int limit, string incentiveID);
  }
}