using Domain.Models;
using System.Data;

namespace Domain.Abstract.Repository
{
    public interface IBaseExtRepository<T> where T : ExtendModel
    {
        IDbConnection GetDbConnection();
        Task<int> Insert(IDbTransaction transaction, T model);
        Task<int> UpdateByID(IDbTransaction transaction, T model);
        Task<List<T>> GetRowForParent(IDbTransaction transaction, string ParentID);
    }

}