using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class InformationSchemaColumnRepository : BaseRepository, IInformationSchemaColumnRepository
  {
    private readonly string tableBase = "vw_ifin_columns";
    public Task<int> DeleteByID(IDbTransaction transaction, string ID)
    {
      throw new NotImplementedException();
    }

    public Task<InformationSchemaColumn> GetRowByID(IDbTransaction transaction, string ID)
    {
      throw new NotImplementedException();
    }

    public async Task<InformationSchemaColumn> GetRowByName(IDbTransaction transaction, string name)
    {
      var p = db.Symbol();

      string query = $@"
        select
          table_name    as TableName
          ,column_name  as Name
          ,data_type    as Type
          ,ordinal_position as OrderKey
          ,is_nullable  as IsNullable
        from
          {tableBase}
        where
          column_name = {p}Name
      ";

      var parameters = new
      {
        Name = name
      };

      return await _command.GetRow<InformationSchemaColumn>(transaction, query, parameters);
    }

    public Task<List<InformationSchemaColumn>> GetRows(IDbTransaction transaction, string tableName)
    {
      var p = db.Symbol();

      string query = $@"
        select
          table_name    as TableName
          ,column_name  as Name
          ,data_type    as Type
          ,ordinal_position as OrderKey
          ,is_nullable  as IsNullable
        from
          {tableBase}
        where
          table_name = {p}TableName
      ";

      var parameters = new
      {
        TableName = tableName
      };

      return _command.GetRows<InformationSchemaColumn>(transaction, query, parameters);
    }
    public Task<List<InformationSchemaColumn>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();

      string query = $@"
        select
          table_name    as TableName
          ,column_name  as Name
          ,data_type    as Type
          ,ordinal_position as OrderKey
          ,is_nullable  as IsNullable
        from
          {tableBase}
      ";

      var parameters = new { };

      return _command.GetRows<InformationSchemaColumn>(transaction, query, parameters);
    }

    public Task<int> Insert(IDbTransaction transaction, InformationSchemaColumn model)
    {
      throw new NotImplementedException();
    }

    public Task<int> UpdateByID(IDbTransaction transaction, InformationSchemaColumn model)
    {
      throw new NotImplementedException();
    }
  }
}