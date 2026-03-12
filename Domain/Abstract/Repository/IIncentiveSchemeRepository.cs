using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IIncentiveSchemeRepository : IBaseRepository<IncentiveScheme>
  {
    Task<int> ChangeStatus(IDbTransaction transaction, IncentiveScheme model);
    Task<int> CountExistingScheme(IDbTransaction transaction, string incentiveType, DateTime? effDate, string? excludeID = null);
    Task<DateTime> GetExistingSchemeEffDate(IDbTransaction transaction, string incentiveType, string? excludeID = null);
  }
}