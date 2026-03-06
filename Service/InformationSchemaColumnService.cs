using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class InformationSchemaColumnService(IInformationSchemaColumnRepository _repo) : BaseService, IInformationSchemaColumnService
  {
    public Task<int> DeleteByID(string[] idList)
    {
      throw new NotImplementedException();
    }

    public Task<InformationSchemaColumn> GetRowByID(string code)
    {
      throw new NotImplementedException();
    }
    public async Task<List<InformationSchemaColumn>> GetRows(string tableName)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.GetRows(transaction, tableName);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<List<InformationSchemaColumn>> GetRows(string? keyword, int offset, int limit)
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
    public Task<int> Insert(InformationSchemaColumn model)
    {
      throw new NotImplementedException();
    }

    public Task<int> UpdateByID(InformationSchemaColumn model)
    {
      throw new NotImplementedException();
    }
  }
}