using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IBaseService<T> where T : BaseModel
  {
    Task<List<T>> GetRows(string? keyword, int offset, int limit);
    Task<T> GetRowByID(string id);
    Task<int> Insert(T model);
    Task<int> UpdateByID(T model);
    Task<int> DeleteByID(string[] idList);
  }
}
