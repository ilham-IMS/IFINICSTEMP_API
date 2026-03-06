using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class SysJobTaskListLogService : BaseService, ISysJobTaskListLogService
  {
    private readonly ISysJobTaskListLogRepository _repo;

    public SysJobTaskListLogService(ISysJobTaskListLogRepository repo)
    {
      _repo = repo;
    }
    public Task<int> DeleteByID(string[] idList)
    {
      throw new NotImplementedException();
    }

    public Task<SysJobTaskListLog> GetRowByID(string id)
    {
      throw new NotImplementedException();
    }

    public Task<List<SysJobTaskListLog>> GetRows(string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }

    public async Task<int> Insert(SysJobTaskListLog model)
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

    public Task<int> UpdateByID(SysJobTaskListLog model)
    {
      throw new NotImplementedException();
    }
  }
}