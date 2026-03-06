using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class DynamicReportTableRelationService(IDynamicReportTableRelationRepository _repo, IDynamicReportTableRepository _dynamicReportTableRepo) : BaseService, IDynamicReportTableRelationService
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
    public async Task<DynamicReportTableRelation> GetRowByID(string id)
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
    public async Task<List<DynamicReportTableRelation>> GetRows(string? keyword, int offset, int limit, string dynamicReportID)
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

    public async Task<List<DynamicReportTableRelation>> GetRows(string? keyword, int offset, int limit)
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

    #region GetRowsByReference
    public async Task<List<DynamicReportTableRelation>> GetRowsByReference(string? keyword, int offset, int limit, string referenceDynamicReportTableID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var result = await _repo.GetRowsByReference(transaction, keyword, offset, limit, referenceDynamicReportTableID);

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
    public async Task<int> Insert(List<DynamicReportTableRelation> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var result = 0;

        foreach (var model in models)
        {
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
    public async Task<int> Insert(DynamicReportTableRelation model)
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
    public async Task<int> UpdateByID(List<DynamicReportTableRelation> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {

        var result = 0;

        foreach (var model in models)
        {
          // DynamicReportTable reportTable = new()
          // {
          //   ID = model.DynamicReportTableID,
          //   JoinClause = model.TableJoinClause,
          //   ModDate = model.ModDate,
          //   ModBy = model.ModBy,
          //   ModIPAddress = model.ModIPAddress
          // };

          // result += await _dynamicReportTableRepo.UpdateJoinClauseByID(transaction, reportTable);
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
    public async Task<int> UpdateByID(DynamicReportTableRelation model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var result = await _repo.UpdateReferenceColumn(transaction, model);

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