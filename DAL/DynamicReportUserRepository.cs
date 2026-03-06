using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;


namespace DAL
{
  public class DynamicReportUserRepository : BaseRepository, IDynamicReportUserRepository
  {
    private string tableBase = "dynamic_report_user";

    #region GetRowsByDynamicReport
    public async Task<List<DynamicReportUser>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
              select
                  dru.id                                 as ID
                  ,dru.dynamic_report_id           as DynamicReportID
                  ,dru.user_id                     as UserID
									,dru.user_code                   as UserCode
									,dru.user_name                   as UserName
							from 
									{tableBase} dru
              where 
                  dru.dynamic_report_id = {p}DynamicReportID
                  and
                    (    
                      dru.user_code like {p}Keyword
                      or dru.user_name like {p}Keyword
                    )
              order by
                      dru.mod_date asc"
      );

      var result = await _command.GetRows<DynamicReportUser>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit,
            DynamicReportID = dynamicReportID
          }
      );
      return result;

    }
    #endregion

    
    #region GetRows
    public async Task<List<DynamicReportUser>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
							select
									dru.id                                 as ID
                  ,dru.dynamic_report_id           as DynamicReportID
                  ,dru.user_id                     as UserID
									,dru.user_code                   as UserCode
									,dru.user_name                   as UserName
							from 
									{tableBase} dru
              where 
									(    
										dru.user_code like {p}Keyword
										or dru.user_name like {p}Keyword
									)
							order by
											dru.mod_date asc"
      );
      var result = await _command.GetRows<DynamicReportUser>(
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

    #region GetRowByID
    public async Task<DynamicReportUser> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query =
          $@"
                    select
                        dru.id                                 as ID
												,dru.dynamic_report_id           as DynamicReportID
												,dru.user_id                     as UserID
												,dru.user_code                   as UserCode
												,dru.user_name                   as UserName
                    from       
                        {tableBase} dru 
                    where
                        dru.id = {p}ID";
      var result = await _command.GetRow<DynamicReportUser>(
          transaction,
          query,
          new { ID = id }
      );
      return result;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, DynamicReportUser module)
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
                                ,dynamic_report_id             
                                ,user_id             
                                ,user_code             
                                ,user_name             
                            ) values (
                                 {p}ID
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIPAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIPAddress
                                --
                                ,{p}DynamicReportID
                                ,{p}UserID
                                ,{p}UserCode
                                ,{p}UserName
                            )";
      var result = await _command.Insert(transaction, query, module);
      return result;
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReportUser module)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableBase} 
                            set
                                user_id    = {p}UserID
                                ,user_code    = {p}UserCode
																,user_name    = {p}UserName
																--
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, module);
      return result;
    }
    #endregion


    #region DeleteByID
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
          $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      var result = await _command.DeleteByID(transaction, query, id);
      return result;
    }
    #endregion
  }
}
