using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;
using Dapper;
using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using System.Text.Json;

namespace DAL
{
  public class GeneralValidationRepository : BaseRepository, IGeneralValidationRepository
  {
    #region CountByDynamicColumn
    public async Task<int> CountByDynamicColumn(IDbTransaction transaction, string tableName, Dictionary<string, string> columnsNValues, string ID)
    {
      int i = 0;
      var p = db.Symbol();

      string query = $@"
          select
            count(id)
          from
            {tableName}
          where
            ID <> {p}ID and ";

      List<string> queries = [];
      Dictionary<string, object> parameters = new()
      {
        ["ID"] = ID
      };

      foreach (var dict in columnsNValues)
      {
        i++;
        var column = dict.Key;
        var value = dict.Value;

        var myStr = $"lower({column}) = lower({p}Value_{i})";

        queries.Add(myStr);
        parameters.Add($"Value_{i}", value);
      }

      query += string.Join(" and ", queries);

      return await _command.GetRow<int>(transaction, query, parameters);
    }
    #endregion

    #region CountData
    public async Task<int> CountData(IDbTransaction transaction, string tableName, List<JsonObject> colNVals)
    {
      var p = db.Symbol();
      DynamicParameters parameters = new();

      string query = $@"
        SELECT
          count(id)
        FROM
          {tableName}
        where ";

      for (int i = 0; i < colNVals.Count; i++)
      {
        var column = colNVals[i]["column"]?.ToString();
        var operat = colNVals[i]["operator"]?.ToString();

        object? value = null;

        if (colNVals[i]["value"]?.GetValueKind() == JsonValueKind.Number)
        {
          try
          {
            value = colNVals[i]["value"]?.GetValue<decimal>();
          }
          catch (Exception)
          {
            value = colNVals[i]["value"]?.GetValue<int>();
          }
        }
        else
        {
          value = colNVals[i]["value"]?.ToString();
        }

        if (i == 0)
        {
          query += $"{column} {operat} {p}Value{i}";
        }
        else
        {
          query += $" AND {column} {operat} {p}Value{i}";
        }

        parameters.Add($"Value{i}", value);
      }

      return await _command.GetRow<int>(transaction, query, parameters);
    }
    #endregion

    #region GetTop
    public async Task<T> GetTop<T>(IDbTransaction transaction, string prefix, string column = "code", string tableName = default!)
    {
      var convertedTableName = string.Concat(typeof(T).Name.Select((x, i) =>
        char.IsUpper(x) && i > 0 ? $"_{x}" : x.ToString()
      )).ToLower();

      string query = $@"
        SELECT
          {column}
        FROM
          {convertedTableName}
        WHERE
          {column} like @Prefix
        order by
          {column} desc ";

      query = QueryLimit(query);

      return await _command.GetRow<T>(transaction, query, new { Limit = 1, Prefix = $"%{prefix}%" });
    }
    #endregion
  }
}