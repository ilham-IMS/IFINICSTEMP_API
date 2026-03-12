using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
    public class InterfaceAgreementIncentiveMarketingService : BaseService, IInterfaceAgreementIncentiveMarketingService
    {
        private readonly IInterfaceAgreementIncentiveMarketingRepository _repo;

        public InterfaceAgreementIncentiveMarketingService(IInterfaceAgreementIncentiveMarketingRepository repo)
        {
            _repo = repo;
        }

        #region GetRowsForJobIn
        public async Task<List<InterfaceAgreementIncentiveMarketing>> GetRowsForJobIn(int limit)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.GetRowsForJobIn(transaction, limit);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region UpdateAfterJobIn
        public async Task<int> UpdateAfterJobIn(InterfaceAgreementIncentiveMarketing model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.UpdateAfterJobIn(transaction, model);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region IBaseService Overrides
        public async Task<int> Insert(InterfaceAgreementIncentiveMarketing model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.Insert(transaction, model);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> UpdateByID(InterfaceAgreementIncentiveMarketing model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.UpdateByID(transaction, model);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> DeleteByID(string id)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.DeleteByID(transaction, id);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<InterfaceAgreementIncentiveMarketing> GetRowByID(string id)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.GetRowByID(transaction, id);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<InterfaceAgreementIncentiveMarketing>> GetRows(string? keyword, int offset, int limit)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.GetRows(transaction, keyword, offset, limit);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<int> DeleteByID(string[] idList)
        {
          throw new NotImplementedException();
        }
        #endregion
  }
}
