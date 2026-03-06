using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportTableRepository : IBaseRepository<DynamicReportTable>
  {
    Task<List<DynamicReportTable>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID);
    Task<List<DynamicReportTable>> GetRowsExclude(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID);

    Task<List<DynamicReportTable>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID);
    Task<int> UpdateJoinClauseByID(IDbTransaction transaction, DynamicReportTable model);
    Task<int> CheckTableExists(IDbTransaction transaction, string MasterDynamicReportTableID);
    Task<string> GetTitle(IDbTransaction transaction, string MasterDynamicReportTableID);
    Task<int> GetCountByAliasName(IDbTransaction transaction, string dynamicReportID, string aliasName);
    Task<List<DynamicReportTable>> GetRowsForValidateTableRelation(IDbTransaction transaction, string dynamicReportID);
  }
}