using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
	public class MasterApprovalRepository : BaseRepository, IMasterApprovalRepository
	{
		private readonly string tableBase = "master_approval";

		public async Task<List<MasterApproval>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
		{
			var p = db.Symbol();

			string query =
			$@"
                select
                        id                                      as ID
                        ,code                                   as Code
                        ,approval_name		                    as ApprovalName
                        ,reff_approval_category_id	          as ReffApprovalCategoryID
                        ,reff_approval_category_name	        as ReffApprovalCategoryName
                        ,reff_approval_category_name	        as ReffApprovalCategoryName
                        ,is_active	                            as IsActive
                from
                        {tableBase}
                where
                        (
                            lower(code)                                 like    lower({p}Keyword)
                            or lower(approval_name)    		            like    lower({p}Keyword)
                            or lower(reff_approval_category_name)       like    lower({p}Keyword)
                            or case is_active
                                when 1 then 'yes'
                                else		'no'
						    end					                        like	 lower({p}Keyword)
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

			var result = await _command.GetRows<MasterApproval>(transaction, query, parameters);
			return result;
		}
		public async Task<MasterApproval> GetRowByID(IDbTransaction transaction, string ID)
		{
			var p = db.Symbol();

			string query = $@"
                    select
                            id                                      as ID
                            ,code                                   as Code
                            ,approval_name		                    as ApprovalName
                            ,reff_approval_category_id  	        as ReffApprovalCategoryID 
                            ,reff_approval_category_name	        as ReffApprovalCategoryName
                            ,reff_approval_category_code	        as ReffApprovalCategoryCode
                            ,is_active	                            as IsActive         
                    from
                            {tableBase}
                    where
                            id = {p}ID
            ";
			var parameters = new
			{
				ID = ID
			};

			var result = await _command.GetRow<MasterApproval>(transaction, query, parameters);
			return result;
		}
		public async Task<MasterApproval> GetRowByCode(IDbTransaction transaction, string code)
		{
			var p = db.Symbol();

			string query = $@"
                    select
                            id                                      as ID
                            ,code                                   as Code
                            ,approval_name		                    as ApprovalName
                            ,reff_approval_category_id  	        as ReffApprovalCategoryID 
                            ,reff_approval_category_name	        as ReffApprovalCategoryName
                            ,reff_approval_category_code	        as ReffApprovalCategoryCode
                            ,is_active	                            as IsActive         
                    from
                            {tableBase}
                    where
                            code = {p}Code
            ";
			var parameters = new
			{
				Code = code
			};

			var result = await _command.GetRow<MasterApproval>(transaction, query, parameters);
			return result;
		}
		public async Task<int> Insert(IDbTransaction transaction, MasterApproval model)
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
                    ,approval_name
                    ,reff_approval_category_id
                    ,reff_approval_category_name
					          ,reff_approval_category_code
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
                    ,{p}Code
                    ,{p}ApprovalName
                    ,{p}ReffApprovalCategoryID
                    ,{p}ReffApprovalCategoryName
                    ,{p}ReffApprovalCategoryCode
                    ,{p}IsActive
                )";
			return await _command.Insert(transaction, query, model);
		}
		public async Task<int> UpdateByID(IDbTransaction transaction, MasterApproval model)
		{
			var p = db.Symbol();

			string query = $@"
                            update {tableBase}
                            set
                                mod_date                                    =   {p}ModDate
                                ,mod_by                                     =   {p}ModBy
                                ,mod_ip_address                             =   {p}ModIPAddress
                                ,code		                                    =   {p}Code
                                ,approval_name                              =   {p}ApprovalName
                                ,reff_approval_category_id          	      =   {p}ReffApprovalCategoryID 
                                ,reff_approval_category_code        	      =   {p}ReffApprovalCategoryCode
                                ,reff_approval_category_name        	      =   {p}ReffApprovalCategoryName
                                ,is_active        	=   {p}IsActive
                            where
                                id                  = {p}ID";
			return await _command.Update(transaction, query, model);
		}
		public async Task<int> DeleteByID(IDbTransaction transaction, string id)
		{
			var p = db.Symbol();

			string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
			return await _command.DeleteByID(transaction, query, id.ToString());
		}
		public async Task<int> ChangeIsActive(IDbTransaction transaction, MasterApproval model)
		{
			var p = db.Symbol();

			string query = $@"
                            update {tableBase}
                            set
                                is_active           =  is_active * -1
                                ,mod_date           =  {p}ModDate
                                ,mod_by             =  {p}ModBy
                                ,mod_ip_address     =  {p}ModIPAddress
                            where
                                id = {p}ID";
			return await _command.Update(transaction, query, model);
		}

		public async Task<MasterApproval> CheckCode(IDbTransaction transaction, string Code)
		{
			var p = db.Symbol();

			string query = $@"
                            select 
                              count(1) as CountCode 
                                from 
                              {tableBase}
                            where
                                code = {p}Code";
			return await _command.GetRow<MasterApproval>(transaction, query, new { Code = Code });
		}

		public async Task<MasterApproval> CheckName(IDbTransaction transaction, string ApprovalName)
		{
			var p = db.Symbol();

			string query = $@"
                            select 
                              count(1) as CountName
                                from 
                              {tableBase}
                            where
                                approval_name = {p}ApprovalName";
			return await _command.GetRow<MasterApproval>(transaction, query, new { ApprovalName = ApprovalName });
		}

		public async Task<MasterApproval> CheckNameForUpdate(IDbTransaction transaction, string ApprovalName, string ID)
		{
			var p = db.Symbol();

			string query = $@"
                            select 
                              count(1) as CountNameForUpdate
                                from 
                              {tableBase}
                            where
                                approval_name = {p}ApprovalName
                                and id <> {p}ID";
			return await _command.GetRow<MasterApproval>(transaction, query, new { ApprovalName = ApprovalName, ID = ID });
		}

    #region CounData
    public async Task<MasterApproval> CountData(IDbTransaction transaction, string reffApprovalCategoryID)
    {
        var p = db.Symbol();
    
        string query = $@"
          select
            id  as ID
          from
            {tableBase}
          where
            reff_approval_category_id = {p}ReffApprovalCategoryID
        ";
    
        // Binding params
        var parameters = new
        {
            ReffApprovalCategoryID = reffApprovalCategoryID
        };
    
        var result = await _command.GetRow<MasterApproval>(transaction, query, parameters);
    
        return result;
    }
    #endregion
	}
}