
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;

namespace Service
{
  public class IncentiveGroupPositionService : BaseService, IIncentiveGroupPositionService
  {

    private readonly IIncentiveGroupPositionRepository _repo;
    public IncentiveGroupPositionService(IIncentiveGroupPositionRepository repo)
    {
      _repo = repo;
    }


    public async Task<IncentiveGroupPosition> GetRowByID(string id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
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
    }

    public async Task<List<IncentiveGroupPosition>> GetRows(string? keyword, int offset, int limit)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
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
    }

    public async Task<List<IncentiveGroupPosition>> GetRowsByGroupID(string? keyword, int offset, int limit, string groupID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = await _repo.GetRowsByGroupID(transaction, keyword, offset, limit, groupID);
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

    public async Task<int> Insert(IncentiveGroupPosition model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          
          int result = await _repo.Insert(transaction, model);
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

    public async Task<int> UpdateByID(IncentiveGroupPosition model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
          try
          {
          int result = await _repo.UpdateByID(transaction, model);
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

    public async Task<int> UpdateByID(List<IncentiveGroupPosition> modelList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
          try
          {
              int result = 0;
              foreach (var model in modelList)
              {
                  // Cek duplikasi hanya jika PositionID berubah
                  var existingRecord = await _repo.GetRowByID(transaction, model.ID);
                  
                  if (existingRecord.PositionID != model.PositionID)
                  {
                      var countPosition = await _repo.CheckDuplicatePosition(
                          transaction, 
                          model.IncentiveGroupID, 
                          model.PositionID, 
                          model.ID);

                      if (countPosition > 0) 
                          throw new Exception($"Position {model.PositionDescription} already exists in this group.");
                  }
                  
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
    }
    
    public async Task<int> DeleteByID(string[] idList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = 0;
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
    }
  }
}