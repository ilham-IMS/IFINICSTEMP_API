using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
	public interface IInformationSchemaTableRepository : IBaseRepository<InformationSchemaTable>
	{

		Task<List<InformationSchemaTable>> GetRowsForLookup(IDbTransaction transaction, string? keyword);
		Task<List<InformationSchemaTable>> GetRowsForLookupExcludeByMasterDynamicReport(IDbTransaction transaction, string? keyword);

	}
}