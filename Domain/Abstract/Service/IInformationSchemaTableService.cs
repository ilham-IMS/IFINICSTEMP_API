using Domain.Models;

namespace Domain.Abstract.Service
{
	public interface IInformationSchemaTableService : IBaseService<InformationSchemaTable>
	{
		Task<List<InformationSchemaTable>> GetRowsForLookup(string? keyword);
		Task<List<InformationSchemaTable>> GetRowsForLookupExcludeByMasterDynamicReport(string? keyword);

	}
}