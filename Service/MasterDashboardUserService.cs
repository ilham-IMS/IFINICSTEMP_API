using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class MasterDashboardUserService : BaseService, IMasterDashboardUserService
  {
    private readonly IMasterDashboardUserRepository _repo;

    public MasterDashboardUserService(IMasterDashboardUserRepository repo)
    {
      _repo = repo;
    }

    #region GetRows
    public Task<List<MasterDashboardUser>> GetRows(string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region GetRows
    public async Task<List<MasterDashboardUser>> GetRows(string? keyword, int offset, int limit, string userID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        List<MasterDashboardUser> result = await _repo.GetRows(transaction, keyword, offset, limit, userID);
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
    public async Task<MasterDashboardUser> GetRowByID(string id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        MasterDashboardUser result = await _repo.GetRowByID(transaction, id);
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
    public async Task<int> Insert(List<MasterDashboardUser> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int result = 0;
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

    public async Task<int> Insert(MasterDashboardUser model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
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
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(List<MasterDashboardUser> models)
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

    public async Task<int> UpdateByID(MasterDashboardUser model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
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
    #endregion

    #region DeleteByID
    public async Task<int> DeleteByID(string[] ids)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int result = 0;
        foreach (string id in ids)
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
  }
}
