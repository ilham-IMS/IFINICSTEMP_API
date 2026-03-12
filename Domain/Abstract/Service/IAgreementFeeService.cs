using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IAgreementFeeService : IBaseService<AgreementFee>
  {
    Task<List<AgreementFee>> GetRowsByAgreementID(string? keyword, int offset, int limit, string agreementID, int isInternalIncome);
  }
}