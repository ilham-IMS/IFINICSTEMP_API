using System.Data;
using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IAgreementIncentiveMarketingService : IBaseService<AgreementIncentiveMarketing>
  {
    Task<List<AgreementIncentiveMarketing>> GetRowsByIncentiveID(string? keyword, int offset, int limit, string incentiveID);
    Task<FileDoc> GetPreview(AgreementIncentiveMarketing dataAgreementMarketing, string ID);
    Task<int> ProcessSync(IDbTransaction transaction, InterfaceAgreementIncentiveMarketing interfaceModel);
  }
}