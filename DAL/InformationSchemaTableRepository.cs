using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class InformationSchemaTableRepository : BaseRepository, IInformationSchemaTableRepository
  {
    private readonly string tableBase = "vw_ifin_tables";
    private readonly string tableMasterDynamicReportTable = "master_dynamic_report_table";
    public Task<int> DeleteByID(IDbTransaction transaction, string ID)
    {
      throw new NotImplementedException();
    }

    public Task<InformationSchemaTable> GetRowByID(IDbTransaction transaction, string ID)
    {
      throw new NotImplementedException();
    }

    public async Task<List<InformationSchemaTable>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();

      string query = $@"
        select
          table_name as Name
          ,table_type as Type
        from
          {tableBase}
      ";

      var parameters = new { };

      return await _command.GetRows<InformationSchemaTable>(transaction, query, parameters);
    }
    public async Task<List<InformationSchemaTable>> GetRowsForLookup(IDbTransaction transaction, string? keyword)
    {
      var p = db.Symbol();

      string query = $@"
        select
          table_name as Name
          ,table_type as Type
        from
          {tableBase}
        where
          (
            lower(table_name)    like ({p}Keyword)
            or lower(table_type) like lower({p}Keyword)
          )
      ";

      var parameters = new
      {
        Keyword = $"%{keyword}%"
      };

      return await _command.GetRows<InformationSchemaTable>(transaction, query, parameters);
    }
    public async Task<List<InformationSchemaTable>> GetRowsForLookupExcludeByMasterDynamicReport(IDbTransaction transaction, string? keyword)
    {
      var p = db.Symbol();

      string query = $@"
        select
          table_name as Name
          ,table_type as Type
        from
          {tableBase}
        where
          (
            lower(table_name)    like ({p}Keyword)
            or lower(table_type) like lower({p}Keyword)
          )
					and lower(table_name) not in (
						select 
							lower(name)
						from
							{tableMasterDynamicReportTable}
					)
      ";

      var parameters = new
      {
        Keyword = $"%{keyword}%"
      };

      return await _command.GetRows<InformationSchemaTable>(transaction, query, parameters);
    }

    public Task<int> Insert(IDbTransaction transaction, InformationSchemaTable model)
    {
      throw new NotImplementedException();
    }

    public Task<int> UpdateByID(IDbTransaction transaction, InformationSchemaTable model)
    {
      throw new NotImplementedException();
    }
  }
}