using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IMasterReportUserService : IBaseService<MasterReportUser>
  {
    Task<int> Insert(List<MasterReportUser> models);
  }
}