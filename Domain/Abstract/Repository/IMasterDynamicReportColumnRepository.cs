using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
	public interface IMasterDynamicReportColumnRepository : IBaseRepository<MasterDynamicReportColumn>
	{
		Task<List<MasterDynamicReportColumn>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string masterDynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsByTables(IDbTransaction transaction, IEnumerable<string> masterDynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForeignByReportTable(IDbTransaction transaction, string dynamicReportID, string masterDynamicReportTableID);

		Task<List<MasterDynamicReportColumn>> GetRowsForeignReferenceToTable(IDbTransaction transaction, string dynamicReportID, string masterDynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookup(IDbTransaction transaction, string? keyword, int offset, int limit, string masterDynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTable(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID);
		Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTableForRelatedColumn(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID, string relatedMasterDynamicReportColumnID);

		Task<List<MasterDynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
		Task<int> ChangeAvailable(IDbTransaction transaction, MasterDynamicReportColumn model);
		Task<int> ChangeMaskingStatus(IDbTransaction transaction, MasterDynamicReportColumn model);
	}
}