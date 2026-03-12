using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IAgreementRefundService : IBaseService<AgreementRefund>
  {
    Task<List<AgreementRefund>> GetRowsByAgreementID(string? keyword, int offset, int limit, string agreementID);
  }
}