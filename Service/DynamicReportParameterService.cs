using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;

using iFinancing360.Service.Helper;

namespace Service
{
  public class DynamicReportParameterService : BaseService, IDynamicReportParameterService
  {
    private readonly IDynamicReportParameterRepository _repo;
    private readonly IDynamicReportParameterExtRepository _repoExt;

    public DynamicReportParameterService(IDynamicReportParameterRepository repo, IDynamicReportParameterExtRepository repoExt)
    {
      _repo = repo;
      _repoExt = repoExt;
    }

    #region GetRowsByDynamicReport
    public async Task<List<DynamicReportParameter>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.GetRowsByDynamicReport(transaction, keyword, offset, limit, dynamicReportID);

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

    #region GetRowsComponentByDynamicReport
    public async Task<List<DynamicReportParameter>> GetRowsComponentByDynamicReport(string dynamicReportID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.GetRowsComponentByDynamicReport(transaction, dynamicReportID);

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
    public async Task<List<DynamicReportParameter>> GetRows(string? keyword, int offset, int limit)
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

    #region GetRowByID
    public async Task<DynamicReportParameter> GetRowByID(string ID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        //await _repo.LockRow(transaction, "master_Form", ID);
        var result = await _repo.GetRowByID(transaction, ID);
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
    public async Task<int> Insert(DynamicReportParameter model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var topRow = (await _repo.GetTopOrderByDynamicReport(transaction, 1, model.DynamicReportID!)).FirstOrDefault();
        var topOrder = topRow?.OrderKey ?? 0;

        model.OrderKey = topOrder + 1;
        model.Name ??= model.Label?.Trim().Replace(" ", "_")+ "_"  + model.ID?.Substring(0, 5);


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
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(DynamicReportParameter model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        model.Name = model.Label?.Trim().Replace(" ", "_") + "_"  + model.ID?.Substring(0, 5);
        var result = await _repo.UpdateByID(transaction, model) ;

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

    #region DeleteByID
    public async Task<int> DeleteByID(string[] listID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int count = 0;
        foreach (var ID in listID)
        {
          var model = await _repo.GetRowByID(transaction, ID);
          count += await _repo.DeleteByID(transaction, ID);

          if (model is not null)
          {
            var columnList = await _repo.GetRowsOrderByDynamicReport(transaction, model.DynamicReportID!);
            int orderKey = 0;
            foreach (var column in columnList)
            {
              column.OrderKey = ++orderKey;
              await _repo.ChangeOrder(transaction, column);
            }
          }
        }
        transaction.Commit();
        return count;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region OrderUp
    public async Task<int> OrderUp(DynamicReportParameter model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;
        var columnList = await _repo.GetRowsOrderByDynamicReport(transaction, model.DynamicReportID!);

        var currentColumn = columnList.Find(x => x.ID == model.ID) ?? throw new Exception("Data not found");
        var currentOrderKey = currentColumn.OrderKey ?? throw new Exception("Order key has not been selected");

        if (currentOrderKey == 1) throw new Exception("Order key cannot be less than 1");

        var nextColumn = columnList.Find(x => x.OrderKey == currentOrderKey - 1);

        if (nextColumn is not null)
        {
          nextColumn.OrderKey = currentOrderKey;
          nextColumn.ModBy = model.ModBy;
          nextColumn.ModDate = model.ModDate;
          nextColumn.ModIPAddress = model.ModIPAddress;
          result += await _repo.ChangeOrder(transaction, nextColumn);
        }

        currentColumn.OrderKey = currentOrderKey - 1;
        currentColumn.ModBy = model.ModBy;
        currentColumn.ModDate = model.ModDate;
        currentColumn.ModIPAddress = model.ModIPAddress;
        result += await _repo.ChangeOrder(transaction, currentColumn);


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

    #region OrderDown
    public async Task<int> OrderDown(DynamicReportParameter model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;
        var columnList = await _repo.GetRowsOrderByDynamicReport(transaction, model.DynamicReportID!);

        var currentColumn = columnList.Find(x => x.ID == model.ID) ?? throw new Exception("Data not found");
        var currentOrderKey = currentColumn.OrderKey ?? throw new Exception("Order key has not been selected");

        if (currentOrderKey == columnList.Count) throw new Exception("Order key cannot be greater than " + columnList.Count.ToString());

        var nextColumn = columnList.Find(x => x.OrderKey == currentOrderKey + 1);

        if (nextColumn is not null)
        {
          nextColumn.OrderKey = currentOrderKey;
          nextColumn.ModBy = model.ModBy;
          nextColumn.ModDate = model.ModDate;
          nextColumn.ModIPAddress = model.ModIPAddress;
          result += await _repo.ChangeOrder(transaction, nextColumn);
        }

        currentColumn.OrderKey = currentOrderKey + 1;
        currentColumn.ModBy = model.ModBy;
        currentColumn.ModDate = model.ModDate;
        currentColumn.ModIPAddress = model.ModIPAddress;
        result += await _repo.ChangeOrder(transaction, currentColumn);


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
			DynamicReportParameter model = await _repo.GetRowByID(transaction, ParentID);
		
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
		private async Task<int> InsertExt(IDbTransaction transaction, dynamic tempProperties, DynamicReportParameter model)
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
