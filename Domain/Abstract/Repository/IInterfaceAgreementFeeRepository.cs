using Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Domain.Abstract.Repository
{
    public interface IInterfaceAgreementFeeRepository : IBaseRepository<InterfaceAgreementFee>
    {
      Task<List<InterfaceAgreementFee>> GetRows(IDbTransaction transaction, string agreementIncentiveID);
    }
}
