using Domain.Models;

namespace Domain.Abstract.Service
{
   public interface IMasterFormService : IBaseService<MasterForm>
   {
      Task<int> ChangeStatus(MasterForm model);
      Task<MasterForm> GetRowByCode(string Code);
   }

}
