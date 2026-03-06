using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportTableRelationRepository : IBaseRepository<DynamicReportTableRelation>
  {
    Task<List<DynamicReportTableRelation>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID);
    Task<List<DynamicReportTableRelation>> GetRows(IDbTransaction transaction, string dynamicReportTableID);
    Task<int> DeleteByReferenceDynamicReportTableID(IDbTransaction transaction, string ReferenceDynamicReportTableID);
    Task<List<DynamicReportTableRelation>> GetRowsByReference(IDbTransaction transaction, string? keyword, int offset, int limit, string referenceDynamicReportTableID);
    Task<int> UpdateReferenceColumn(IDbTransaction transaction, DynamicReportTableRelation model);
    Task<int> DeleteReferenceDetail(IDbTransaction transaction, string DynamicReportTableID);
  }
}