using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
  public interface IBaseRepository<T> where T : BaseModel
  {
    IDbConnection GetDbConnection();
    Task<List<T>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit);
    Task<T> GetRowByID(IDbTransaction transaction, string id);
    Task<int> Insert(IDbTransaction transaction, T model);
    Task<int> UpdateByID(IDbTransaction transaction, T model);
    Task<int> DeleteByID(IDbTransaction transaction, string id);
  }

}