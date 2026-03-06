using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class DynamicReportColumnService(IDynamicReportColumnRepository _repo) : BaseService, IDynamicReportColumnService
  {
    #region GetRows
    public async Task<List<DynamicReportColumn>> GetRows(string? keyword, int offset, int limit)
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

    #region GetRowsByDynamicReport
    public async Task<List<DynamicReportColumn>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
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

    #region GetRowsForLookupExcludeByDynamicReport
    public async Task<List<DynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
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

    #region GetRowByID
    public async Task<DynamicReportColumn> GetRowByID(string id)
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
    public async Task<int> Insert(List<DynamicReportColumn> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;

        string dynamicReportID = models.FirstOrDefault()?.DynamicReportID ?? "";

        foreach (var model in models)
        {
          var topRow = (await _repo.GetTopOrderByDynamicReport(transaction, 1, dynamicReportID)).FirstOrDefault();
          int orderKey = topRow?.OrderKey ?? 0;
          model.OrderKey = ++orderKey;


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
    public async Task<int> Insert(DynamicReportColumn model)
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
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(List<DynamicReportColumn> models)
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
    public async Task<int> UpdateByID(DynamicReportColumn model)
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
    #endregion

    #region OrderUp
    public async Task<int> OrderUp(DynamicReportColumn model)
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
    public async Task<int> OrderDown(DynamicReportColumn model)
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
          var model = await _repo.GetRowByID(transaction, id);
          result += await _repo.DeleteByID(transaction, id);

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