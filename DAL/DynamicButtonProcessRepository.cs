using Domain.Models;
using iFinancing360.DAL.Helper;
using System.Data;
using Domain.Abstract.Repository;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Dapper;

namespace DAL
{
  public class DynamicButtonProcessRepository : BaseRepository, IDynamicButtonProcessRepository
  {
    private readonly string tableBase = "dynamic_button_process";

    // Di SysModuleRepository
    public Task<List<DynamicButtonProcess>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }
    public async Task<List<DynamicButtonProcess>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string ParentMenuID)
    {
      var p = db.Symbol();
      string query = $@"
												select   
														{tableBase}.id             	 as ID
														,{tableBase}.code            as Code
														,{tableBase}.name            as Name
														,{tableBase}.is_active       as IsActive
														,{tableBase}.parent_menu_id  as ParentMenuID
														-- SysMenu (Parent)
														,mm.name                     as ParentMenuName
												from 
														{tableBase}
												left join 
														{tableBase} mm on mm.id = {tableBase}.parent_menu_id
				
												where
														(
															lower({tableBase}.name)									like	lower({p}Keyword)
															or lower(mm.name)												like	lower({p}Keyword)
															or case {tableBase}.is_active
																		when	1	then 'yes'
																		else					'no'
																	end																	like	lower({p}Keyword)
														)
												";


      if (ParentMenuID != "all")
      {
        query += $" and {tableBase}.parent_menu_id = {p}ParentMenuID";
      }

      query += $" order by {tableBase}.mod_date desc";

      query = QueryLimitOffset(query);

      var result = await _command.GetRows<DynamicButtonProcess>(transaction, query, new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        ParentMenuID,
      });

      return result;
    }
    public async Task<DynamicButtonProcess> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
											select   
												{tableBase}.id                 	as ID
												,{tableBase}.code					  		as Code
												,{tableBase}.name               as Name
												,{tableBase}.parent_menu_id     as ParentMenuID
												,{tableBase}.url_menu           as URLMenu
												,{tableBase}.order_key          as OrderKey
												,{tableBase}.css_icon           as CssIcon
												,{tableBase}.is_active          as IsActive
												,{tableBase}.type               as Type
												,{tableBase}.icon_color         as IconColor
												-- SysMenu (Parent)
												,mm.name                     		as ParentMenuName
											from 
												{tableBase}
											left join 
												{tableBase} mm on mm.id = {tableBase}.parent_menu_id
											where
													{tableBase}.id = {p}ID ";

      var result = await _command.GetRow<DynamicButtonProcess>(transaction, query, new { ID = id });
      return result;
    }
    public async Task<int> Insert(IDbTransaction transaction, List<DynamicButtonProcess> model)
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
													,name         
													,parent_menu_id        
													,url_menu
													,order_key
													,css_icon
													,is_active
													,type
                          ,icon_color
												) 
												values 
												(
													{p}ID
													,{p}CreDate
                          ,{p}CreBy
                          ,{p}CreIpAddress
                          ,{p}ModDate
                          ,{p}ModBy
                          ,{p}ModIpAddress
                          ,{p}Code
                          ,{p}Name
                          ,{p}ParentMenuID
                          ,{p}URLMenu
                          ,{p}OrderKey
                          ,{p}CssIcon
                          ,{p}IsActive
                          ,{p}Type
                          ,{p}IconColor
												)";

      return await BulkInsertAsync(transaction, query, model);
    }
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicButtonProcess model)
    {
      var p = db.Symbol();

      string query = $@"
                          update {tableBase} 
                          set
														name                = {p}Name
														,module_id          = {p}ModuleID
														,parent_menu_id     = {p}ParentMenuID
														,url_menu           = {p}URLMenu
														,order_key          = {p}OrderKey
														,css_icon           = {p}CssIcon
														,is_active          = {p}IsActive
														,type               = {p}Type
                            ,icon_color         = {p}IconColor
														,mod_date           = {p}ModDate
														,mod_by             = {p}ModBy
														,mod_ip_address     = {p}ModIpAddress
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
                                id = {p}ID";
      return await _command.DeleteByID(transaction, query, id);
    }

    public async Task<int> BulkDelete(IDbTransaction tx)
    {
      const string sql = @"DELETE FROM dynamic_button_process";
      return await tx.Connection.ExecuteAsync(sql, new { }, tx);
    }



    public async Task<List<DynamicButtonProcess>> GetTop(IDbTransaction transaction, int limit)
    {
      string query = QueryLimit(
         $@"
				select
							{tableBase}.code        as Code
				from 
						{tableBase}
				order by
						{tableBase}.code desc
				"
      );

      var parameters = new
      {
        Limit = limit
      };

      var result = await _command.GetRows<DynamicButtonProcess>(transaction, query, parameters);
      return result;
    }


    private async Task<int> BulkInsertAsync<T>(
        IDbTransaction tx,
        string query,
        IEnumerable<T> models)
    {
      if (tx == null) throw new ArgumentNullException(nameof(tx));
      if (query == null) throw new ArgumentNullException(nameof(query));
      if (models == null) throw new ArgumentNullException(nameof(models));

      var affected = await tx.Connection.ExecuteAsync(
          query,
          models,
          transaction: tx);

      return affected;
    }


    public async Task<int> Insert(IDbTransaction transaction, DynamicButtonProcess model)
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
													,name         
													,module_id    
													,parent_menu_id        
													,url_menu
													,order_key
													,css_icon
													,is_active
													,type
                          ,icon_color
												) 
												values 
												(
													{p}ID
													,{p}CreDate
                          ,{p}CreBy
                          ,{p}CreIpAddress
                          ,{p}ModDate
                          ,{p}ModBy
                          ,{p}ModIpAddress
                          ,{p}Code
                          ,{p}Name
                          ,{p}ModuleID
                          ,{p}ParentMenuID
                          ,{p}URLMenu
                          ,{p}OrderKey
                          ,{p}CssIcon
                          ,{p}IsActive
                          ,{p}Type
                          ,{p}IconColor
												)";

      return await _command.Insert(transaction, query, model);
    }

    public async Task<List<DynamicButtonProcess>> GetRowsForLookupParent(IDbTransaction transaction, string? keyword, int offset, int limit, bool withAll = true)
    {
      var p = db.Symbol();
      string query = $@"
												select   
													 id              as ID
													,code           as Code
													,name           as Name
												from 
													{tableBase}
												where 
													(
														lower(code)			like	lower({p}Keyword)
														or lower(name)	like	lower({p}Keyword)
													)
												and
													is_active = 1
												and
													lower(type) = 'parent'
												";


      query += " order by order_key ASC";

      QueryLimitOffset(query);

      var result = await _command.GetRows<DynamicButtonProcess>(transaction, query, new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
      });

      if (withAll && result.Count > 0 && string.IsNullOrEmpty(keyword))
      {
        result = result.Prepend(new DynamicButtonProcess { ID = "all", Code = "ALL", Name = "ALL" }).ToList();
      }

      return result;
    }
  }
}




