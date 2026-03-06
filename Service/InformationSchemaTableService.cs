using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
	public class InformationSchemaTableService(IInformationSchemaTableRepository _repo) : BaseService, IInformationSchemaTableService
	{
		public Task<int> DeleteByID(string[] idList)
		{
			throw new NotImplementedException();
		}

		public Task<InformationSchemaTable> GetRowByID(string code)
		{
			throw new NotImplementedException();
		}

		public async Task<List<InformationSchemaTable>> GetRowsForLookup(string? keyword)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookup(transaction, keyword);

				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

		}
		public async Task<List<InformationSchemaTable>> GetRowsForLookupExcludeByMasterDynamicReport(string? keyword)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookupExcludeByMasterDynamicReport(transaction, keyword);

				transaction.Commit();
				return result;
			}
			catch (Exception)

			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<List<InformationSchemaTable>> GetRows(string? keyword, int offset, int limit)
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

		public Task<int> Insert(InformationSchemaTable model)
		{
			throw new NotImplementedException();
		}

		public Task<int> UpdateByID(InformationSchemaTable model)
		{
			throw new NotImplementedException();
		}
	}
}