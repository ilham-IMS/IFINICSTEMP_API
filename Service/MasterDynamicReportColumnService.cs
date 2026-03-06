using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
	public class MasterDynamicReportColumnService(IMasterDynamicReportColumnRepository _repo, IInformationSchemaColumnRepository _informationSchemaColumnRepo) : BaseService, IMasterDynamicReportColumnService
	{
		#region DeleteByID
		public async Task<int> DeleteByID(string[] idList)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				int result = 0;

				foreach (var id in idList)
				{
					result += await _repo.DeleteByID(transaction, id);
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
		#endregion

		#region GetRowByID
		public async Task<MasterDynamicReportColumn> GetRowByID(string id)
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

		#region GetRows
		public async Task<List<MasterDynamicReportColumn>> GetRows(string? keyword, int offset, int limit, string masterDynamicReportTableID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRows(transaction, keyword, offset, limit, masterDynamicReportTableID);

				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}
		public async Task<List<MasterDynamicReportColumn>> GetRows(string? keyword, int offset, int limit)
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
		#endregion

		#region GetRowsForLookup
		public async Task<List<MasterDynamicReportColumn>> GetRowsForLookup(string? keyword, int offset, int limit, string masterDynamicReportTableID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookup(transaction, keyword, offset, limit, masterDynamicReportTableID);

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

		#region GetRowsForLookupByDynamicReport
		public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookupByDynamicReport(transaction, keyword, offset, limit, dynamicReportID);

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

		#region GetRowsForLookupByDynamicReportTable
		public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTable(string? keyword, int offset, int limit, string dynamicReportTableID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookupByDynamicReportTable(transaction, keyword, offset, limit, dynamicReportTableID);

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
		#region GetRowsForLookupByDynamicReportTableForRelatedColumn
		public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTableForRelatedColumn(string? keyword, int offset, int limit, string dynamicReportTableID, string relatedMasterDynamicReportColumnID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookupByDynamicReportTableForRelatedColumn(transaction, keyword, offset, limit, dynamicReportTableID, relatedMasterDynamicReportColumnID);

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

		#region GetRowsForLookupExcludeByDynamicReport
		public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookupExcludeByDynamicReport(transaction, keyword, offset, limit, dynamicReportID);

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

		#region GetRowsForeignReferenceToTable
		public async Task<List<MasterDynamicReportColumn>> GetRowsForeignReferenceToTable(string dynamicReportID, string masterDynamicReportTableID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForeignReferenceToTable(transaction, dynamicReportID, masterDynamicReportTableID);

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
		public async Task<int> Insert(List<MasterDynamicReportColumn> models)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = 0;
				foreach (var model in models)
				{
					var InformationSchemaColumn = await _informationSchemaColumnRepo.GetRowByName(transaction, model.Name!) ?? throw new Exception($"Information Schema Column {model.Name} Not Found");
					model.IsAvailable = 1;
					model.IsMasking = -1;
					model.OrderKey = InformationSchemaColumn.OrderKey ?? throw new Exception("Order key has not been selected");

					result += await _repo.Insert(transaction, model);
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
		public async Task<int> Insert(MasterDynamicReportColumn model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var InformationSchemaColumn = await _informationSchemaColumnRepo.GetRowByName(transaction, model.Name!) ?? throw new Exception($"Information Schema Column {model.Name} Not Found");
				model.IsAvailable = 1;
				model.IsMasking = -1;
				model.OrderKey = InformationSchemaColumn.OrderKey ?? throw new Exception("Order key has not been selected");

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
		#endregion

		#region UpdateByID
		public async Task<int> UpdateByID(MasterDynamicReportColumn model)
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
		public async Task<int> UpdateByID(IEnumerable<MasterDynamicReportColumn> models)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				int result = 0;
				foreach (var model in models)
				{
					result += await _repo.UpdateByID(transaction, model);
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
		#endregion

		#region ChangeAvailable
		public async Task<int> ChangeAvailable(MasterDynamicReportColumn model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.ChangeAvailable(transaction, model);

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

		#region ChangeMaskingStatus
		public async Task<int> ChangeMaskingStatus(MasterDynamicReportColumn model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.ChangeMaskingStatus(transaction, model);

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

	}
}