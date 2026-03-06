using Domain.Models;
using iFinancing360.DAL.Helper;
using System.Data;
using Domain.Abstract.Repository;


namespace DAL
{
  public class DynamicButtonProcessRoleRepository : BaseRepository, IDynamicButtonProcessRoleRepository
  {
    private readonly string tableBase = "dynamic_button_process_role";


    public async Task<List<DynamicButtonProcessRole>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicButtonProcessID)
    {
      var p = db.Symbol();
      string query = $@"
                            select   
                                 id               as ID
                                ,role_code        as Code
                                ,role_name        as Name
                                ,role_access      as RoleAccess
                                ,is_dynamic       as IsDynamic
                            from 
                                {tableBase}
                            where
                                is_dynamic = 1
                            and
                                (
                                    lower(role_code)        like lower({p}Keyword)
                                    or lower(role_name)     like lower({p}Keyword)
                                    or lower(role_access)   like lower({p}Keyword)
                                    or case role_access
                                    when 'A' then 'ACCESS'
                                    when 'C' then 'CREATE/UPDATE/GENERATE/UPLOAD'
                                    when 'U' then 'MATCHING/VALIDATE/EDITABLE'
                                    when 'D' then 'DELETE'
                                    when 'O' then 'POST/PROCEED/APPROVE'
                                    when 'R' then 'CANCEL/REJECT'  
                                    when 'P' then 'PRINT/DOWNLOAD'          
                                end                               like lower({p}Keyword) 
                                )
                            and
                                menu_id = {p}MenuID
                            order by 
                                cre_date asc
                            ";

      query = QueryLimitOffset(query);

      var result = await _command.GetRows<DynamicButtonProcessRole>(transaction, query, new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        MenuID = dynamicButtonProcessID
      });
      return result;
    }
    public async Task<List<DynamicButtonProcessRole>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
                            select   
                                 id               as ID
                                ,role_code        as Code
                                ,role_name        as Name
                                ,role_access      as RoleAccess
                                ,is_dynamic       as IsDynamic
                            from 
                                {tableBase}
                            where
                                (
                                    lower(role_code)        like lower({p}Keyword)
                                    or lower(role_name)     like lower({p}Keyword)
                                    or lower(role_access)   like lower({p}Keyword)
                                )
                            order by mod_date desc
                                ";

      var result = await _command.GetRows<DynamicButtonProcessRole>(transaction, query, new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      });
      return result;
    }

    public async Task<DynamicButtonProcessRole> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query = $@"
                            select   
                                id                as ID                 
                                ,menu_id          as DynamicButtonProcessID
                                ,role_code        as Code
                                ,role_name        as Name
                                ,role_access      as RoleAccess
                                ,is_dynamic       as IsDynamic
                            from 
                                {tableBase}
                            where
                                id = {p}ID";

      var result = await _command.GetRow<DynamicButtonProcessRole>(transaction, query, new { ID = id });
      return result;
    }
    public async Task<DynamicButtonProcessRole> GetRowByRoleCode(IDbTransaction transaction, string roleCode)
    {
      var p = db.Symbol();
      string query = $@"
                            select   
                                id                as ID                 
                                ,menu_id          as DynamicButtonProcessID
                                ,role_code        as Code
                                ,role_name        as Name
                                ,role_access      as RoleAccess
                                ,is_dynamic       as IsDynamic
                            from 
                                {tableBase}
                            where 
                                role_code = {p}RoleCode";

      var result = await _command.GetRow<DynamicButtonProcessRole>(transaction, query, new { RoleCode = roleCode });
      return result;
    }
    public async Task<int> Insert(IDbTransaction transaction, DynamicButtonProcessRole model)
    {
      var p = db.Symbol();
      string query = $@"
                            insert into {tableBase} (      
                                id     
                                ,role_code      
                                ,role_name
                                ,menu_id        
                                ,role_access    
                                ,cre_date
                                ,cre_by
                                ,cre_ip_address
                                ,mod_date
                                ,mod_by
                                ,mod_ip_address
                                ,is_dynamic
                            ) values (
                                {p}ID
                                ,{p}Code
                                ,{p}Name
                                ,{p}DynamicButtonProcessID
                                ,{p}RoleAccess
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIpAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIpAddress
                                ,{p}IsDynamic
                            )";
      return await _command.Insert(transaction, query, model);
    }
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicButtonProcessRole model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase} 
                            set
                                 role_name           = {p}Name
                                ,menu_id             = {p}DynamicButtonProcessID
                                ,role_access         = {p}RoleAccess
                                ,is_dynamic          = {p}IsDynamic
                                ,mod_date            = {p}ModDate
                                ,mod_by              = {p}ModBy
                                ,mod_ip_address      = {p}ModIpAddress
                            where
                                id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID ";
      return await _command.DeleteByID(transaction, query, id);
    }
  }
}



