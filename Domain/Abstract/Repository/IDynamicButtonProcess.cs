using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
	public interface IDynamicButtonProcessRepository : IBaseRepository<DynamicButtonProcess>
	{
		Task<List<DynamicButtonProcess>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string ParentMenuID);
		Task<int> Insert(IDbTransaction transaction, List<DynamicButtonProcess> model);
		Task<int> BulkDelete(IDbTransaction transaction);
		Task<List<DynamicButtonProcess>> GetRowsForLookupParent(IDbTransaction transaction, string? keyword, int offset, int limit, bool withAll = true);


	}
}
