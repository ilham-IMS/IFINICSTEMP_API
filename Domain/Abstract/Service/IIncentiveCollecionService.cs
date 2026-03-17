using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IIncentiveCollectionService : IBaseService<IncentiveCollection>
  {
    Task<List<IncentiveCollection>> GetRows(string? keyword, int offset, int limit, string PeriodeFrom, string PeriodeTo);
    Task<string> GetHTMLPreview(string[] ids, IncentiveCollection incentiveCollectionData);
    Task<FileDoc> PrintDocument(string mimeType, string[] ids, IncentiveCollection dataAgreementCollection);
  }
}