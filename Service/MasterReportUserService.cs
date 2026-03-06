using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class MasterReportUserService : BaseService, IMasterReportUserService
  {

    private readonly IMasterReportUserRepository _repo;
    public MasterReportUserService(IMasterReportUserRepository repo)
    {
      _repo = repo;
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

    public Task<MasterReportUser> GetRowByID(string id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<MasterReportUser>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<int> Insert(List<MasterReportUser> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          int results = 0;
          foreach (var model in models)
          {
            int result = await _repo.Insert(transaction, model);
            results = result++;
          }
          transaction.Commit();
          return results;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }

    public Task<int> Insert(MasterReportUser model)
    {
      throw new NotImplementedException();
    }

    public Task<int> UpdateByID(MasterReportUser model)
    {
      throw new NotImplementedException();
    }
  }
}