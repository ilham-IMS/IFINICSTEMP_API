using Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Domain.Abstract.Repository
{
    public interface IInterfaceAgreementIncentiveMarketingRepository : IBaseRepository<InterfaceAgreementIncentiveMarketing>
    {
        Task<List<InterfaceAgreementIncentiveMarketing>> GetRowsForJobIn(IDbTransaction transaction, int limit);
        Task<int> UpdateAfterJobIn(IDbTransaction transaction, InterfaceAgreementIncentiveMarketing model);
        Task<int> CountDataByCode(IDbTransaction transaction, string code);
    }
}
