using Domain.Models;
using System.Data;
using System.Text.Json.Nodes;

namespace Domain.Abstract.Repository
{
  public interface IGeneralValidationRepository
  {
    Task<int> CountByDynamicColumn(IDbTransaction transaction, string tableName, Dictionary<string, string> columnsNValues, string ID);
    Task<int> CountData(IDbTransaction transaction, string tableName, List<JsonObject> colNVals);
    Task<T> GetTop<T>(IDbTransaction transaction, string prefix, string column = "code", string tableName = default!);
  }
}