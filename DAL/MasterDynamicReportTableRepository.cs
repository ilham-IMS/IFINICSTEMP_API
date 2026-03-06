
using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class MasterDynamicReportTableRepository : BaseRepository, IMasterDynamicReportTableRepository
  {
    #region Tables
    private readonly string tableBase = "master_dynamic_report_table";
    #endregion

    #region DeleteByID (transaction, ID)
    public async Task<int> DeleteByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();

      string query =
          $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      var result = await _command.DeleteByID(transaction, query, ID);
      return result;
    }
    #endregion

    #region GetRowByID
    public async Task<MasterDynamicReportTable> GetRowByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query =
          $@"
                            select
                                 id                                as ID
                                ,name                              as Name
                                ,alias                             as Alias
                            from       
                                {tableBase}    
                            where 
                                id = {p}ID
                            "
      ;
      var result = await _command.GetRow<MasterDynamicReportTable>(
          transaction,
          query,
          new
          {
            ID = ID,
          }
      );
      return result;
    }
    #endregion

    #region GetRows
    public async Task<List<MasterDynamicReportTable>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                 id                                as ID
                                ,name                              as Name
                                ,alias                             as Alias
                            from       
                                {tableBase}    
                            where (    
                                    lower(name)                    like lower({p}Keyword)
                                    or lower(alias)                like lower({p}Keyword)
                                  )
                            order by
                                    mod_date desc"
      );
      var result = await _command.GetRows<MasterDynamicReportTable>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit
          }
      );
      return result;
    }
    #endregion

    #region GetRowsForLookup
    public async Task<List<MasterDynamicReportTable>> GetRowsForLookup(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                 id                                as ID
                                ,alias                             as Alias
                            from       
                                {tableBase}    
                            where (    
                                    lower(alias)                like lower({p}Keyword)
                                  )
                            order by
                                    alias asc"
      );
      var result = await _command.GetRows<MasterDynamicReportTable>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit
          }
      );
      return result;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, MasterDynamicReportTable model)
    {
      var p = db.Symbol();
      string query =
          $@"
                            insert into {tableBase} (
                                 id
                                ,cre_date
                                ,cre_by
                                ,cre_ip_address
                                ,mod_date
                                ,mod_by
                                ,mod_ip_address
                                --
                                ,name             
                                ,alias      
                            ) values (
                                {p}ID
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIPAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIPAddress
                                --
                                ,{p}Name
                                ,{p}Alias
                            )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }

    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, MasterDynamicReportTable model)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableBase} 
                            set
                                alias               = {p}Alias   
                                --
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, model);
      return result;
    }
    #endregion

    public async Task<List<MasterDynamicReportTable>> GetReportData(IDbTransaction transaction)
    {
      string query =
                      $@"
                      select
												name                              as Name
                        ,alias                             as Alias
											from
												{tableBase}
                        order by
                            mod_date desc ";
      var result = await _command.GetRows<MasterDynamicReportTable>(
          transaction,
          query);
      return result;
    }
  }
}