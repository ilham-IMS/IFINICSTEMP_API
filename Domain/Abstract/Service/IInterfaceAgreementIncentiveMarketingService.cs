using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstract.Service
{
    public interface IInterfaceAgreementIncentiveMarketingService : IBaseService<InterfaceAgreementIncentiveMarketing>
    {
        Task<List<InterfaceAgreementIncentiveMarketing>> GetRowsForJobIn(int limit);
        Task<int> UpdateAfterJobIn(InterfaceAgreementIncentiveMarketing model);
    }
}
