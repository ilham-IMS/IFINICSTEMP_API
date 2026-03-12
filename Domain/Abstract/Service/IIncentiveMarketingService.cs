using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IIncentiveMarketingService : IBaseService<IncentiveMarketing>
  {
    Task<List<IncentiveMarketing>> GetRows(string? keyword, int offset, int limit, string PeriodeFrom, string PeriodeTo);
    Task<string> GetHTMLPreview(string id, IncentiveMarketing incentiveMarketingData, List<AgreementIncentiveMarketing> dataAgreement);
    Task<FileDoc> PrintDocument(string mimeType, string id, IncentiveMarketing incentiveMarketingData, List<AgreementIncentiveMarketing> dataAgreement);
  }
}