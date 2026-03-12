using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
    public class InterfaceAgreementRefundService : BaseService, IInterfaceAgreementRefundService
    {
        private readonly IInterfaceAgreementRefundRepository _repo;

        public InterfaceAgreementRefundService(IInterfaceAgreementRefundRepository repo)
        {
            _repo = repo;
        }

        #region GetRows
        public async Task<List<InterfaceAgreementRefund>> GetRows(string? keyword, int offset, int limit)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                var result = await _repo.GetRows(transaction, keyword, offset, limit);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<InterfaceAgreementRefund>> GetRows(IDbTransaction transaction, string agreementIncentiveID)
        {
            try
            {
                var result = await _repo.GetRows(transaction, agreementIncentiveID);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetRowByID
        public async Task<InterfaceAgreementRefund> GetRowByID(string id)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                var result = await _repo.GetRowByID(transaction, id);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region Insert
        public async Task<int> Insert(InterfaceAgreementRefund model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                int result = await _repo.Insert(transaction, model);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region UpdateByID
        public async Task<int> UpdateByID(InterfaceAgreementRefund model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                int result = await _repo.UpdateByID(transaction, model);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region DeleteByID
        public async Task<int> DeleteByID(string[] idList)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                int countResult = 0;

                foreach (var id in idList)
                {
                    var result = await _repo.DeleteByID(transaction, id);
                    if (result > 0) countResult += result;
                }

                transaction.Commit();
                return countResult;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion
    }
}
