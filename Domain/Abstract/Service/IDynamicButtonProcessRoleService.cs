using Domain.Models;

namespace Domain.Abstract.Service;

public interface IDynamicButtonProcessRoleService : IBaseService<DynamicButtonProcessRole>
{
    Task<int> SyncButtonProcessRole(DynamicButtonProcessRole model);
    Task<List<DynamicButtonProcessRole>> GetRows(string? keyword, int offset, int limit, string dynamicButtonProcessID);
    Task<DynamicButtonProcessRole> GetRowByRoleCode(string roleCode);
}