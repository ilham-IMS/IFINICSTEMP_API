using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
   public interface IFormDropdownRepository : IBaseRepository<FormDropdown>
   {
      Task<List<FormDropdown>> GetRows(IDbTransaction transaction, string FormControlsID);
   }

}