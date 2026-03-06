using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveMarketingRepository : IBaseRepository<IncentiveMarketing>
  {
    Task<List<IncentiveMarketing>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string PeriodeFrom, string PeriodeTo);
  }
}