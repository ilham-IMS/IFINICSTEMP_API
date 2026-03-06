using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IJournalGlLinkService : IBaseService<JournalGlLink>
  {
    Task<int> ChangeIsActive(JournalGlLink model);
    Task<FileDoc> GetReportData();
  }
}
