using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
  public interface IJournalGlLinkRepository : IBaseRepository<JournalGlLink>
  {
    Task<int> ChangeIsActive(IDbTransaction transaction, JournalGlLink model);
    Task<List<JournalGlLink>> GetReportData(IDbTransaction transaction);
  }
}