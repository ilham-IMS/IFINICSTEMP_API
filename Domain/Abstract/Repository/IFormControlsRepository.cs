using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
   public interface IFormControlsRepository : IBaseRepository<FormControls>
   {
      Task<int> ChangeStatus(IDbTransaction transaction, FormControls model);
      Task<List<FormControls>> GetRows(IDbTransaction transaction, string MasterFormID);
      Task<List<FormControls>> GetRowsForDataTable(IDbTransaction transaction, string MasterFormID);
      Task<int> ChangeStatusNonActive(IDbTransaction transaction, FormControls model);
   }

}