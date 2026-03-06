using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IAgreementMarketingService : IBaseService<AgreementMarketing>
  {
    Task<FileDoc> GetPreview(AgreementMarketing dataAgreementMarketing, string ID);
    Task<FileDoc> GenerateDocumentAllTypeDoc(string mimeType, string ID, AgreementMarketing dataAgreementMarketing);
  }
}