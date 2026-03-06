using System.Data;
using System.Reflection;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using DotNetEnv;
using iFinancing360.Helper;
using iFinancing360.Service.Helper;

namespace Service
{
	public class MasterDynamicReportTableService(IMasterDynamicReportTableRepository _repo,IMasterDynamicReportTableExtRepository _repoExt, IInformationSchemaColumnRepository _informationSchemaColumnRepo, IMasterDynamicReportColumnRepository _masterDynamicReportColumnRepo, IDynamicReportTableRepository _repoDynamicReportTable) : BaseService, IMasterDynamicReportTableService
	{
		public async Task<int> DeleteByID(string[] idList)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				int result = 0;
				List<string> errors = [];

				foreach (var id in idList)
				{
					if (await _repoDynamicReportTable.CheckTableExists(transaction, id) > 0)
					{
						string title = await _repoDynamicReportTable.GetTitle(transaction, id);
						errors.Add($"Table is already used in {title}");
					}
					if (errors.Count <= 0)
					{
						result += await _repo.DeleteByID(transaction, id);
					}
				}
				if (errors.Count > 0) throw new Exception(string.Join(";\n", errors));

				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<MasterDynamicReportTable> GetRowByID(string id)
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

		public async Task<List<MasterDynamicReportTable>> GetRows(string? keyword, int offset, int limit)
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

		public async Task<List<MasterDynamicReportTable>> GetRowsForLookup(string? keyword, int offset, int limit)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.GetRowsForLookup(transaction, keyword, offset, limit);

				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<int> Insert(List<MasterDynamicReportTable> models)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				int result = 0;
				foreach (var model in models)
				{
					model.Alias ??= model.Name?.Replace("_", " ").ToUpper();
					var insertResult = await _repo.Insert(transaction, model);


					if (insertResult > 0)
					{
						var columns = await _informationSchemaColumnRepo.GetRows(transaction, model.Name ?? "");

						if (columns.Count <= 0) throw new Exception($"There is no columns for table {model.Alias}");

						PropertyInfo[] properties = typeof(BaseModel).GetProperties();
						foreach (var column in columns)
						{
							string? alias = column.Name?.Replace("_", " ").ToUpper();


							var dynamicReportColumn = new MasterDynamicReportColumn
							{
								ID = GUID(),
								CreDate = model.CreDate,
								ModDate = model.ModDate,
								CreBy = model.CreBy,
								ModBy = model.ModBy,
								CreIPAddress = model.CreIPAddress,
								ModIPAddress = model.ModIPAddress,
								MasterDynamicReportTableID = model.ID,
								Name = column.Name,
								Alias = alias,
								Type = column.Type,
								OrderKey = column.OrderKey,
								IsAvailable = properties.Any(x => x.Name.Equals(alias?.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase)) ? -1 : 1,
								IsMasking = -1
							};

							insertResult += await _masterDynamicReportColumnRepo.Insert(transaction, dynamicReportColumn);
						}
					}

					result += insertResult;
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

		public async Task<int> Insert(MasterDynamicReportTable model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.Insert(transaction, model);

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



		public async Task<int> UpdateByID(MasterDynamicReportTable model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();
			try
			{
				var result = await _repo.UpdateByID(transaction, model);

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

		public async Task<FileDoc> GetReportData()
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				string title = "Master Dynamic Report Table List";
				List<string> headers = new() { "Name", "Alias" }; // Header disesuaikan dengan jumalh kolom yang ditampilkan dan menggunakan PascalCase

				var reportData = await _repo.GetReportData(transaction);
				// Jika data pada database ingin dirubah misalnya 1 menjadi Yes dapat menggunakan cara mapping seperti dibawah ini
				var formattedData = reportData.Select(tempData => new
				{
					Name = tempData.Name,
					Alias = tempData.Alias
				}).ToList();

				MemoryStream ms = new MemoryStream();
				ms = await GenerateExcelTemplateWithData(title, headers, formattedData);

				transaction.Commit();
				return new FileDoc
				{
					Content = ms.ToArray(),
					Name = $"master_dynamic_report_table_{Static.FileTimeStamp}.xlsx",
					MimeType = FileMimeType.Xlsx
				};

			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		#region GetRowForParent
		public async Task<List<ExtendModel>> GetRowForParent(string ParentID)
		{
		  using var connection = _repo.GetDbConnection();
		  using var transaction = connection.BeginTransaction();
		
		  try
		  {
			MasterDynamicReportTable model = await _repo.GetRowByID(transaction, ParentID);
		
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
		private async Task<int> InsertExt(IDbTransaction transaction, dynamic tempProperties, MasterDynamicReportTable model)
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