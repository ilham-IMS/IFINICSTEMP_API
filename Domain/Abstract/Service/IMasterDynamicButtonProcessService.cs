using Domain.Models;

namespace Domain.Abstract.Service
{
	public interface IMasterDynamicButtonProcessService : IBaseService<MasterDynamicButtonProcess>
	{
		Task<List<MasterDynamicButtonProcess>> GetRowsForLookup(string? keyword, int offset, int limit);
		Task<bool> IsDLLExist(string assemblyName);
		Task<List<ExtendModel>> GetRowForParent(string ParentID);
	}
}