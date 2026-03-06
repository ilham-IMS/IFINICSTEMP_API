using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
	public class DynamicButtonProcessRoleService : BaseService, IDynamicButtonProcessRoleService
	{
		private readonly IDynamicButtonProcessRoleRepository _repo;

		public DynamicButtonProcessRoleService(IDynamicButtonProcessRoleRepository repo)
		{
			_repo = repo;
		}





		public async Task<DynamicButtonProcessRole> GetRowByID(string id)
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
		public async Task<DynamicButtonProcessRole> GetRowByRoleCode(string roleCode)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.GetRowByRoleCode(transaction, roleCode);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}



		public async Task<List<DynamicButtonProcessRole>> GetRows(string? keyword, int offset, int limit, string dynamicButtonProcessID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.GetRows(transaction, keyword, offset, limit, dynamicButtonProcessID);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

		}

		public async Task<List<DynamicButtonProcessRole>> GetRows(string? keyword, int offset, int limit)
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

		public async Task<int> Insert(DynamicButtonProcessRole model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.Insert(transaction, model);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<int> SyncButtonProcessRole(DynamicButtonProcessRole model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				int result = 0;
				DynamicButtonProcessRole menuRole = await _repo.GetRowByID(transaction, model.ID);

				if (menuRole != null)
				{
					result += await Update(transaction, model);
				}
				else
				{
					result += await Insert(transaction, model);
				}
				transaction.Commit();
				return result;

			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<int> UpdateByID(DynamicButtonProcessRole model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.UpdateByID(transaction, model);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		private async Task<int> Insert(IDbTransaction transaction, DynamicButtonProcessRole model)
		{
			var result = await _repo.Insert(transaction, model);
			return result;
		}
		private async Task<int> Update(IDbTransaction transaction, DynamicButtonProcessRole model)
		{
			var result = await _repo.UpdateByID(transaction, model);
			return result;
		}
		
		public async Task<int> DeleteByID(string[] idList)
		{
			var connection = _repo.GetDbConnection();
			var transaction = connection.BeginTransaction();

			try
			{
				int result = 0;
				foreach (string id in idList)
				{
					result = await _repo.DeleteByID(transaction, id);
				}
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
