using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class SysJobTaskListRepository : BaseRepository, ISysJobTaskListRepository
  {
    private readonly string tableBase = "sys_job_tasklist";
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      return await _command.DeleteByID(transaction, query, id.ToString());
    }

    public async Task<SysJobTasklist> GetCodeGenerate(IDbTransaction transaction, int offset, int limit)
    {
      string query =
                $@"
                                select
                                        code        AS Code
                                from 
                                        {tableBase}
                                order by
                                        code desc

                            ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Offset = offset,
        Limit = limit
      };
      var result = await _command.GetRow<SysJobTasklist>(transaction, query, parameters);
      return result;
    }

    public async Task<SysJobTasklist> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                    select
                        id                   AS   ID
                        ,code                AS   Code
                        ,type                AS   Type
                        ,description         AS   Description
                        ,sp_name             AS   SpName
                        ,order_no            AS   OrderNo
                        ,is_active           AS   IsActive
                        ,row_to_process      AS   RowToProcess
                    from
                        {tableBase}
                    where
                        id = {p}ID
            ";
      var parameters = new { id };
      var result = await _command.GetRow<SysJobTasklist>(transaction, query, parameters);
      return result;
    }

    public async Task<List<SysJobTasklist>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                          select 
                                id                 AS ID
                              ,code                AS   Code
                              ,type                AS   Type
                              ,description         AS   Description
                              ,sp_name             AS   SpName
                              ,order_no            AS   OrderNo
                              ,is_active           AS   IsActive
                              ,row_to_process      AS   RowToProcess
                          from 
                              {tableBase}
                          where 
                          (
                            lower(code)                 like    lower({p}Keyword)
                            or lower(description)       like    lower({p}Keyword)
                            or lower(type)              like    lower({p}Keyword)
                            or lower(sp_name)           like    lower({p}Keyword)
                            or lower(order_no)          like    lower({p}Keyword)
                            or lower(row_to_process)    like    lower({p}Keyword)
                            or case is_active
                                when 1 then 'yes'
                                else		'no'
						                end					      like	  lower({p}Keyword)
                        )
                        order by
                        mod_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<SysJobTasklist>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, SysJobTasklist model)
    {
      var p = db.Symbol();
      string query = $@"
			    insert into {tableBase}
                (
                    id
                    ,cre_date
                    ,cre_by
                    ,cre_ip_address
                    ,mod_date
                    ,mod_by
                    ,mod_ip_address
                    ,code          
                    ,type
                    ,description
                    ,sp_name
                    ,order_no
                    ,is_active
                    ,last_id
                    ,row_to_process
                    ,eod_status
                )
                values
                (
                    {p}ID
                    ,{p}CreDate
                    ,{p}CreBy
                    ,{p}CreIPAddress
                    ,{p}ModDate
                    ,{p}ModBy
                    ,{p}ModIPAddress
                    ,{p}Code
                    ,{p}Type
                    ,{p}Description
                    ,{p}SpName
                    ,{p}OrderNo
                    ,{p}IsActive
                    ,{p}LastID
                    ,{p}RowToProcess
                    ,{p}EodStatus

                )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, SysJobTasklist model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date            =  {p}ModDate
                                ,mod_by             =  {p}ModBy
                                ,mod_ip_address     =  {p}ModIPAddress
                                ,order_no           =  {p}OrderNo
                            where
						            id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
  }
}