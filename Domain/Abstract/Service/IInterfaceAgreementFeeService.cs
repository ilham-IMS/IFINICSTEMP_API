using System.Data;
using Domain.Models;

namespace Domain.Abstract.Service
{
    public interface IInterfaceAgreementFeeService : IBaseService<InterfaceAgreementFee>
    { 
      Task<List<InterfaceAgreementFee>> GetRows(IDbTransaction transaction, string agreementIncentiveID);
    }
}
