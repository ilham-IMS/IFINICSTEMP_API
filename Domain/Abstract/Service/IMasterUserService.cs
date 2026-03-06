using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IMasterUserService : IBaseService<MasterUser>
  {
    Task<FileDoc> GetReportData();
  }
}