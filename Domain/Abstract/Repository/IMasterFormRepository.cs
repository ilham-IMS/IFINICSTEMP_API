using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
  public interface IMasterFormRepository : IBaseRepository<MasterForm>
  {
    Task<int> ChangeStatus(IDbTransaction transaction, MasterForm model);
    Task<MasterForm> GetRowByCode(IDbTransaction transaction, string Code);
    Task<int?> CountByName(IDbTransaction transaction, string name, string ID);
    Task<int?> CountByCode(IDbTransaction transaction, string code);
    Task<int?> CountByLabel(IDbTransaction transaction, string label, string ID);
  }

}