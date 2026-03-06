using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IDynamicReportService : IBaseService<DynamicReport>
  {
    Task<List<DynamicReport>> GetRowsPublishedByUser(string? keyword, int offset, int limit, string userCode);
    Task<object> Print(DynamicReport model);
    Task<object> GetQuery(string ID);
    Task<int> ChangePublishStatus(DynamicReport model);
    Task<FileDoc> GetReportData();
    Task<List<ExtendModel>> GetRowForParent(string ParentID);

  }
}