using System.Data;
using Domain.Models;

namespace Domain.Abstract.Service
{
    public interface IInterfaceAgreementRefundService : IBaseService<InterfaceAgreementRefund>
    { 
      Task<List<InterfaceAgreementRefund>> GetRows(IDbTransaction transaction, string agreementIncentiveID);
    }
}
