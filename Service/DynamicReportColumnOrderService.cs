using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class DynamicReportColumnOrderService(IDynamicReportColumnOrderRepository _repo, IDynamicReportColumnRepository _dynamicReportColumnRepo) : BaseService, IDynamicReportColumnOrderService
  {
    #region GetRows
    public async Task<List<DynamicReportColumnOrder>> GetRows(string? keyword, int offset, int limit)
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
    public async Task<List<DynamicReportColumnOrder>> GetRowsByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
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

    #region GetRowByID
    public async Task<DynamicReportColumnOrder> GetRowByID(string id)
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
    public async Task<int> Insert(List<DynamicReportColumnOrder> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;

        string dynamicReportColumnID = models.FirstOrDefault()?.DynamicReportColumnID ?? "";

        DynamicReportColumn dynamicReportColumn = await _dynamicReportColumnRepo.GetRowByID(transaction, dynamicReportColumnID) ?? throw new Exception("DynamicReportColumn not found");

        foreach (var model in models)
        {
          var topRow = (await _repo.GetTopOrderByDynamicReport(transaction, 1, dynamicReportColumn.DynamicReportID!)).FirstOrDefault();
          int orderKey = topRow?.OrderKey ?? 0;
          model.OrderKey = orderKey + 1;
          model.OrderBy = "ASC";

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
    public async Task<int> Insert(DynamicReportColumnOrder model)
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
    public async Task<int> UpdateByID(List<DynamicReportColumnOrder> models)
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
    public async Task<int> UpdateByID(DynamicReportColumnOrder model)
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
    public async Task<int> OrderUp(DynamicReportColumnOrder model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;

        DynamicReportColumn dynamicReportColumn = await _dynamicReportColumnRepo.GetRowByID(transaction, model.DynamicReportColumnID!) ?? throw new Exception("DynamicReportColumn not found");

        var columnList = await _repo.GetRowsOrderByDynamicReport(transaction, dynamicReportColumn.DynamicReportID!);

        var currentColumnOrder = columnList.Find(x => x.ID == model.ID) ?? throw new Exception("Data not found");
        var currentOrderKey = currentColumnOrder.OrderKey ?? throw new Exception("Order key has not been selected");

        if (currentOrderKey == 1) throw new Exception("Order key cannot be less than 1");

        var nextColumnOrder = columnList.Find(x => x.OrderKey == currentOrderKey - 1);

        if (nextColumnOrder is not null)
        {
          nextColumnOrder.OrderKey = currentOrderKey;
          nextColumnOrder.ModBy = model.ModBy;
          nextColumnOrder.ModDate = model.ModDate;
          nextColumnOrder.ModIPAddress = model.ModIPAddress;
          result += await _repo.ChangeOrder(transaction, nextColumnOrder);
        }

        currentColumnOrder.OrderKey = currentOrderKey - 1;
        currentColumnOrder.ModBy = model.ModBy;
        currentColumnOrder.ModDate = model.ModDate;
        currentColumnOrder.ModIPAddress = model.ModIPAddress;
        result += await _repo.ChangeOrder(transaction, currentColumnOrder);


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
    public async Task<int> OrderDown(DynamicReportColumnOrder model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;
        DynamicReportColumn dynamicReportColumn = await _dynamicReportColumnRepo.GetRowByID(transaction, model.DynamicReportColumnID!) ?? throw new Exception("DynamicReportColumn not found");

        var columnList = await _repo.GetRowsOrderByDynamicReport(transaction, dynamicReportColumn.DynamicReportID!);

        var currentColumnOrder = columnList.Find(x => x.ID == model.ID) ?? throw new Exception("Data not found");
        var currentOrderKey = currentColumnOrder.OrderKey ?? throw new Exception("Order key has not been selected");

        if (currentOrderKey == columnList.Count) throw new Exception("Order key cannot be greater than " + columnList.Count.ToString());

        var nextColumnOrder = columnList.Find(x => x.OrderKey == currentOrderKey + 1);

        if (nextColumnOrder is not null)
        {
          nextColumnOrder.OrderKey = currentOrderKey;
          nextColumnOrder.ModBy = model.ModBy;
          nextColumnOrder.ModDate = model.ModDate;
          nextColumnOrder.ModIPAddress = model.ModIPAddress;
          result += await _repo.ChangeOrder(transaction, nextColumnOrder);
        }

        currentColumnOrder.OrderKey = currentOrderKey + 1;
        currentColumnOrder.ModBy = model.ModBy;
        currentColumnOrder.ModDate = model.ModDate;
        currentColumnOrder.ModIPAddress = model.ModIPAddress;
        result += await _repo.ChangeOrder(transaction, currentColumnOrder);


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
            DynamicReportColumn dynamicReportColumn = await _dynamicReportColumnRepo.GetRowByID(transaction, model.DynamicReportColumnID!) ?? throw new Exception("DynamicReportColumn not found");

            var columnList = await _repo.GetRowsOrderByDynamicReport(transaction, dynamicReportColumn.DynamicReportID!);

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