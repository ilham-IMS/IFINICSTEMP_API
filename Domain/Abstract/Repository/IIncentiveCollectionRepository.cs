using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveCollectionRepository : IBaseRepository<IncentiveCollection>
  {
    Task<List<IncentiveCollection>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string PeriodeFrom, string PeriodeTo);
  }
}