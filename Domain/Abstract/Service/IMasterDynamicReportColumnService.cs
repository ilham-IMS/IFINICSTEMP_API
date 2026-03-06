using Domain.Models;

namespace Domain.Abstract.Service
{
	public interface IMasterDynamicReportColumnService : IBaseService<MasterDynamicReportColumn>
	{
		Task<List<MasterDynamicReportColumn>> GetRows(string? keyword, int offset, int limit, string masterDynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookup(string? keyword, int offset, int limit, string masterDynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTable(string? keyword, int offset, int limit, string dynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTableForRelatedColumn(string? keyword, int offset, int limit, string dynamicReportTableID, string relatedMasterDynamicReportColumnID);

		Task<List<MasterDynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID);
		Task<List<MasterDynamicReportColumn>> GetRowsForeignReferenceToTable(string dynamicReportID, string masterDynamicReportTableID);
		Task<int> UpdateByID(IEnumerable<MasterDynamicReportColumn> models);
		Task<int> ChangeAvailable(MasterDynamicReportColumn model);
		Task<int> ChangeMaskingStatus(MasterDynamicReportColumn model);
	}
}