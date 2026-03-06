using Domain.Models;

namespace Domain.Abstract.Service
{
   public interface IFormControlsService : IBaseService<FormControls>
   {
      Task<int> ChangeStatus(FormControls model);
      Task<List<FormControls>> GetRows(string MasterFormID);
      Task<List<FormControls>> GetRowsForDataTable(string MasterFormID);
   }

}
