
using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository;

public interface IDynamicButtonProcessRoleRepository : IBaseRepository<DynamicButtonProcessRole>
{
    Task<List<DynamicButtonProcessRole>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicButtonProcessID);
    Task<DynamicButtonProcessRole> GetRowByRoleCode(IDbTransaction transaction, string roleCode);

}