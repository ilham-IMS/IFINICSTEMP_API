using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using System.Data;

namespace Service
{
	public class DynamicReportTableService(IDynamicReportTableRepository _repo,IDynamicReportTableExtRepository _repoExt, IMasterDynamicReportColumnRepository _masterDynamicReportColumnRepo, IDynamicReportTableRelationRepository _dynamicReportTableRelationRepo, IDynamicReportColumnRepository _dynamicReportColumnRepo) : BaseService, IDynamicReportTableService
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
					var table = await _repo.GetRowByID(transaction, id);

					result += await _dynamicReportColumnRepo.DeleteFormulaByTable(transaction, table.DynamicReportID!, table.Alias!);
					// result += await _dynamicReportTableRelationRepo.DeleteByReferenceDynamicReportTableID(transaction, id);
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
		public async Task<DynamicReportTable> GetRowByID(string id)
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
		public async Task<List<DynamicReportTable>> GetRows(string? keyword, int offset, int limit, string dynamicReportID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.GetRows(transaction, keyword, offset, limit, dynamicReportID);

				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<List<DynamicReportTable>> GetRows(string? keyword, int offset, int limit)
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

		#region GetRowsExclude
		public async Task<List<DynamicReportTable>> GetRowsExclude(string? keyword, int offset, int limit, string dynamicReportTableID)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.GetRowsExclude(transaction, keyword, offset, limit, dynamicReportTableID);

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
		public async Task<int> Insert(DynamicReportTable model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				List<string> errors = new List<string>();
				var existTable = await _repo.GetCountByAliasName(transaction, model.DynamicReportID!, model.Alias!);
				if (existTable > 0)
				{
					errors.Add($"Alias {model.Alias} is already exist in this report.");
				}
				if (errors.Count > 0) throw new Exception(string.Join(";\n", errors));

				model.JoinClause = "INNER";
				var result = await _repo.Insert(transaction, model);

				// if (result > 0)
				// {
				// 	var foreignColumns = await _masterDynamicReportColumnRepo.GetRowsForeignByReportTable(transaction, model.DynamicReportID!, model.MasterDynamicReportTableID!);

				// 	foreach (var foreignColumn in foreignColumns)
				// 	{
				// 		DynamicReportTableRelation relation = new DynamicReportTableRelation
				// 		{
				// 			ID = GUID(),
				// 			CreDate = model.CreDate,
				// 			CreBy = model.CreBy,
				// 			CreIPAddress = model.CreIPAddress,
				// 			ModDate = model.ModDate,
				// 			ModBy = model.ModBy,
				// 			ModIPAddress = model.ModIPAddress,
				// 			DynamicReportTableID = model.ID,
				// 			// MasterDynamicReportColumnID = foreignColumn.ID,
				// 			// ReferenceDynamicReportTableID = foreignColumn.ReportTableReferenceID,
				// 			// ReferenceMasterDynamicReportColumnID = foreignColumn.ColumnReferenceID,
				// 		};
				// 		result += await _dynamicReportTableRelationRepo.Insert(transaction, relation);
				// 	}
				// }

				var tempProperties = model.Properties;
        model.Properties = null;
        await InsertExt(transaction, tempProperties, model); // Panggil InsertExt()

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
		public async Task<int> UpdateByID(DynamicReportTable model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				var result = await _repo.UpdateByID(transaction, model);

				if (model.IsTableReference == -1)
        {
            // Panggil method DeleteReferenceDetail dari DynamicReportTableRelation
            await _dynamicReportTableRelationRepo.DeleteReferenceDetail(transaction, model.ID!);
        }


				// Update Dynamic Form data
        var extProperties = await _repoExt.GetRowForParent(transaction, model.ID!);
        var tempProperties = model.Properties;
        model.Properties = null;
        await UpdateExt(transaction, tempProperties, extProperties, model); // Panggil UpdateExt()

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

		#region GetRowForParent
		public async Task<List<ExtendModel>> GetRowForParent(string ParentID)
		{
		  using var connection = _repo.GetDbConnection();
		  using var transaction = connection.BeginTransaction();
		
		  try
		  {
			DynamicReportTable model = await _repo.GetRowByID(transaction, ParentID);
		
			var result = await _repoExt.GetRowForParent(transaction, model.ID);
		
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

    #region InsertExt
		private async Task<int> InsertExt(IDbTransaction transaction, dynamic tempProperties, DynamicReportTable model)
		{
		  var properties = await DeserializeProperties(tempProperties);
		
		  if (properties == null) return 0;
		
		  int resultExt = 0;
		
		  foreach (var property in properties)
		  {
			var extendModel = new ExtendModel
			{
			  ID = Guid.NewGuid().ToString("N").ToLower(),
			  CreDate = model.CreDate,
			  CreBy = model.CreBy,
			  CreIPAddress = model.CreIPAddress,
			  ModDate = model.ModDate,
			  ModBy = model.ModBy,
			  ModIPAddress = model.ModIPAddress,
			  ParentID = model.ID,
			  Keyy = property.Key,
			  Value = property.Value,
			  Properties = null
			};
		
			resultExt += await _repoExt.Insert(transaction, extendModel);
		  }
		
		  return resultExt;
		}
		#endregion

    #region UpdateExt
		private async Task<int> UpdateExt<T>(IDbTransaction transaction, dynamic tempProperties, List<ExtendModel> extProperties, T model) where T : ExtendModel
		{
		  int result = 0;
		
		  var properties = await DeserializeProperties(tempProperties);
		
		  foreach (var property in properties)
		  {
			var keyy = property.Key;
			var value = property.Value;
		
			if (value is not string)
			{
			  value = value.ToString();
			}
		
			if (extProperties.Any(e => e.Keyy == keyy))
			{
			  var extendModel = new ExtendModel
			  {
				ParentID = model.ID,
				ModDate = model.ModDate,
				ModBy = model.ModBy,
				ModIPAddress = model.ModIPAddress,
				Keyy = keyy,
				Value = value,
				Properties = null
			  };
		
			  result += await _repoExt.UpdateByID(transaction, extendModel);
			}
			else
			{
			  var extendModel = new ExtendModel
			  {
				ID = Guid.NewGuid().ToString("N").ToLower(),
				CreDate = model.CreDate,
				CreBy = model.CreBy,
				CreIPAddress = model.CreIPAddress,
				ModDate = model.ModDate,
				ModBy = model.ModBy,
				ModIPAddress = model.ModIPAddress,
				ParentID = model.ID,
				Keyy = keyy,
				Value = value,
				Properties = null
			  };
		
			  result += await _repoExt.Insert(transaction, extendModel);
			}
		  }
		
		  return result;
		}
		#endregion
	}
}