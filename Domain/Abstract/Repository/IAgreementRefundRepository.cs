using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IAgreementRefundRepository : IBaseRepository<AgreementRefund>
  {
    Task<List<AgreementRefund>> GetRowsByAgreementID(IDbTransaction transaction, string? keyword, int offset, int limit, string agreementID);
  }
}