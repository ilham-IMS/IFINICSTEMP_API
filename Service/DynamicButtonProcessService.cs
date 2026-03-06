using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
	public class DynamicButtonProcessService : BaseService, IDynamicButtonProcessService
	{
		private readonly IDynamicButtonProcessRepository _repo;

		public DynamicButtonProcessService(IDynamicButtonProcessRepository repo)
		{
			_repo = repo;
		}

        public Task<int> DeleteByID(string[] idList)
        {
            throw new NotImplementedException();
        }

        public async Task<DynamicButtonProcess> GetRowByID(string id)
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

        public Task<List<DynamicButtonProcess>> GetRows(string? keyword, int offset, int limit)
		{
			throw new NotImplementedException();

		}
		public async Task<List<DynamicButtonProcess>> GetRows(string? keyword, int offset, int limit, string ParentMenuID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{

				var result = await _repo.GetRows(transaction, keyword, offset, limit, ParentMenuID);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

		}

        public async Task<List<DynamicButtonProcess>> GetRowsForLookupParent(string? keyword, int offset, int limit, bool withAll = true)
        {
            using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.GetRowsForLookupParent(transaction, keyword, offset, limit, withAll);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

        }

        public async Task<int> Insert(DynamicButtonProcess model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.Insert(transaction ,model);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

        }

        public async Task<int> SyncButtonProcess(List<DynamicButtonProcess> models)
        {
            using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				List<string> Ids = models.Select(x => x.ID).ToList();

				int deletes = await _repo.BulkDelete(transaction);
				
				var result = await _repo.Insert(transaction ,models);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
        }

        public async Task<int> UpdateByID(DynamicButtonProcess model)
        {
            using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.UpdateByID(transaction ,model);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
        }
    }
}
