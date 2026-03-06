
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;

namespace Service
{
  public class IncentiveSchemeDetailService : BaseService, IIncentiveSchemeDetailService
  {

    private readonly IIncentiveSchemeDetailRepository _repo;
    public IncentiveSchemeDetailService(IIncentiveSchemeDetailRepository repo)
    {
      _repo = repo;
    }


    public async Task<IncentiveSchemeDetail> GetRowByID(string id)
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

    public async Task<List<IncentiveSchemeDetail>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<List<IncentiveSchemeDetail>> GetRowsBySchemeID(string? keyword, int offset, int limit, string schemeID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = await _repo.GetRowsBySchemeID(transaction, keyword, offset, limit, schemeID);
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

    public async Task<int> Insert(IncentiveSchemeDetail model)
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

    public async Task<int> UpdateByID(IncentiveSchemeDetail model)
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

    public async Task<int> UpdateByID(List<IncentiveSchemeDetail> modelList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
          try
          {
          int result = 0;
          foreach (var model in modelList)
          {
            List<string> ErrorList = new List<string>();
            if (model.FromRate < 0 ) ErrorList.Add("From Rate must be greater than or equal to 0");
            if (model.ToRate < 0 ) ErrorList.Add("To Rate must be greater than or equal to 0");
            if (model.FromRate > model.ToRate) ErrorList.Add("From Rate must be less than or equal to To Rate");
            if (model.IncentiveRate < 0 ) ErrorList.Add("Incentive Rate must be greater than or equal to 0");

            if (ErrorList.Count > 0) throw new Exception(string.Join(";\n ", ErrorList));

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