using Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Domain.Abstract.Repository
{
    public interface IInterfaceAgreementRefundRepository : IBaseRepository<InterfaceAgreementRefund>
    {
      Task<List<InterfaceAgreementRefund>> GetRows(IDbTransaction transaction, string agreementIncentiveID);
    }
}
