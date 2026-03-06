using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class MasterDynamicButtonProcessRepository : BaseRepository, IMasterDynamicButtonProcessRepository
  {
    private readonly string tableBase = "master_dynamic_button_process";

    #region GetRows
    public async Task<List<MasterDynamicButtonProcess>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      string p = db.Symbol();

      string query = $@"
							select
								 id 									  as ID
								,dll_name							  as DllName
								,namespace_name					as NamespaceName
								,class_name							as ClassName
								,method_name						as MethodName
								,description						as Description
								,short_description			as ShortDescription
								,is_active							as IsActive
							from
								{tableBase}
							where
								(
									lower(dll_name) 						    like lower ({p}Keyword)
									or lower (namespace_name) 			like lower ({p}Keyword)
									or lower (class_name)		        like lower ({p}Keyword)
									or lower (method_name)		      like lower ({p}Keyword)
									or lower (description)		      like lower ({p}Keyword)
								)
							order by
								{tableBase}.mod_date desc"
      ;

      query = QueryLimitOffset(query);

      object parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      List<MasterDynamicButtonProcess> result = await _command.GetRows<MasterDynamicButtonProcess>(transaction, query, parameters);

      return result;
    }
    #endregion

    #region GetRowByID
    public async Task<MasterDynamicButtonProcess> GetRowByID(IDbTransaction transaction, string id)
    {
      string p = db.Symbol();

      string query = $@"
							select
								 id 									as ID
								,dll_name							as DllName
								,namespace_name					as NamespaceName
								,class_name							as ClassName
								,method_name						as MethodName
								,short_description				as ShortDescription
								,description						as Description
								,is_active							as IsActive
							from
								{tableBase}
							where
								id = {p}ID
					";

      object parameters = new
      {
        ID = id
      };

      MasterDynamicButtonProcess result = await _command.GetRow<MasterDynamicButtonProcess>(transaction, query, parameters);

      return result;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, MasterDynamicButtonProcess model)
    {
      string p = db.Symbol();

      string query = $@"insert into {tableBase}
							(
								 id
								,cre_date
								,cre_by
								,cre_ip_address
								,mod_date
								,mod_by
								,mod_ip_address
								--
								,dll_name
								,namespace_name
								,class_name
								,method_name
								,short_description
								,description
								,is_active
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
								--
								,{p}DllName
								,{p}NamespaceName
								,{p}ClassName
								,{p}MethodName
								,{p}ShortDescription
								,{p}Description
								,{p}IsActive
							)
					";

      return await _command.Insert(transaction, query, model);
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, MasterDynamicButtonProcess model)
    {
      string p = db.Symbol();

      string query = $@"update {tableBase}
							set
								 dll_name 				    = {p}DllName
								,namespace_name		    = {p}NamespaceName
								,class_name 			    = {p}ClassName
								,method_name 			    = {p}MethodName
								,short_description    = {p}ShortDescription
								,description 			    = {p}Description
								,is_active	 			    = {p}IsActive
                --
                
                ,mod_date           = {p}ModDate
                ,mod_by             = {p}ModBy
                ,mod_ip_address     = {p}ModIPAddress
							where
								id = {p}ID
										";

      var resut = await _command.Update(transaction, query, model);
      return resut;
    }
    #endregion

    #region DeleteByID
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      string p = db.Symbol();

      string query = $@"delete from {tableBase}
								   where id = {p}ID";

      return await _command.DeleteByID(transaction, query, id);
    }

    public async Task<List<MasterDynamicButtonProcess>> GetRowsForLookup(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      string p = db.Symbol();

      string query = $@"
							select
								 id 									as ID
								,dll_name							as DllName
								,namespace_name					as NamespaceName
								,class_name							as ClassName
								,method_name						as MethodName
								,short_description				as ShortDescription
								,description						as Description
								,is_active							as IsActive
							from
								{tableBase}
							where
								(
									dll_name 						like {p}Keyword
									or namespace_name 			like {p}Keyword
									or class_name 					like {p}Keyword
									or method_name 				like {p}Keyword
									or description 				like {p}Keyword
								)
							and
								is_active = 1
							order by
								mod_date desc
					";

      query = QueryLimitOffset(query);

      object parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      List<MasterDynamicButtonProcess> result = await _command.GetRows<MasterDynamicButtonProcess>(transaction, query, parameters);

      return result;
    }
    #endregion

    public async Task<int?> CountByShortDesc(IDbTransaction transaction, string shortDescription, string ID)
    {
      var p = db.Symbol();

      string query = $@"
                        select
                                count(id) as ID
                        from
                                {tableBase}
                        where
                                {tableBase}.short_description = {p}ShortDescription
                        and 
                                {tableBase}.id != {p}ID  
                    ";

      var parameters = new
      {
        ShortDescription = shortDescription,
        ID = ID
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }

    public async Task<int?> CountByDescription(IDbTransaction transaction, string description, string ID)
    {
      var p = db.Symbol();

      string query = $@"
                        select
                                count(id)	as ID
                        from
                                {tableBase}
                        where
                                {tableBase}.description = {p}Description
                        and     {tableBase}.id != {p}ID   
                    ";

      var parameters = new
      {
        Description = description
          ,
        ID = ID
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }

  }
}