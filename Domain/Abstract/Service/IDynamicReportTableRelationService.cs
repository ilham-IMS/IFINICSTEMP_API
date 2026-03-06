using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportTableRelationService : IBaseService<DynamicReportTableRelation>
  {
    Task<List<DynamicReportTableRelation>> GetRows(string? keyword, int offset, int limit, string dynamicReportTableID);
    Task<int> Insert(DynamicReportTableRelation models);
    Task<int> UpdateByID(List<DynamicReportTableRelation> models);
    Task<List<DynamicReportTableRelation>> GetRowsByReference(string? keyword, int offset, int limit, string referenceDynamicReportTableID);

  }
}