using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IAgreementFeeRepository : IBaseRepository<AgreementFee>
  {
    Task<List<AgreementFee>> GetRowsByAgreementID(IDbTransaction transaction, string? keyword, int offset, int limit, string agreementID, int isInternalIncome);
  }
}