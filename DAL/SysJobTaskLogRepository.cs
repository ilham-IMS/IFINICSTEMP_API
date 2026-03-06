using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
	public class SysJobTaskLogRepository : BaseRepository, ISysJobTaskLogRepository
	{
		private readonly string tableBase = "sys_job_task_log";

		#region GetRows
		public async Task<List<SysJobTaskLog>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
		{
			string p = db.Symbol();

			string query = $@"
							select
									 id 					as ID
									,error_message 			as ErrorMessage
									,stack_trace	 		as StackTrace
							from
								{tableBase}
							where
								(
									lower(error_message) like lower({p}Keyword)
								)
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

			List<SysJobTaskLog> result = await _command.GetRows<SysJobTaskLog>(transaction, query, parameters);

			return result;
		}
		
		#endregion

		#region GetRowByID
		public async Task<SysJobTaskLog> GetRowByID(IDbTransaction transaction, string id)
		{
			string p = db.Symbol();

			string query = $@"
							select
									 id 					as ID
									,error_message 			as ErrorMessage
									,stack_trace	 		as StackTrace
							from
								{tableBase}
							where
								id = {p}ID
					";

			object parameters = new
			{
				ID = id
			};

			SysJobTaskLog result = await _command.GetRow<SysJobTaskLog>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region Insert
		public async Task<int> Insert(IDbTransaction transaction, SysJobTaskLog model)
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
								,sys_job_task_id
								,error_message
								,stack_trace
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
								,{p}SysJobTaskID
								,{p}ErrorMessage
								,{p}StackTrace
							)
					";

			return await _command.Insert(transaction, query, model);
		}
		#endregion

		#region UpdateByID
		public async Task<int> UpdateByID(IDbTransaction transaction, SysJobTaskLog model)
		{
			string p = db.Symbol();

			string query = $@"update {tableBase}
							set
								 error_message 	= {p}ErrorMessage
								,stack_trace    = {p}StackTrace
								-- 
								,mod_date 	    = {p}ModDate
								,mod_by 	    = {p}ModBy
								,mod_ip_address = {p}ModIPAddress
							where
								id = {p}ID
										";

			return await _command.Update(transaction, query, model);
		}
		#endregion

		#region DeleteByID
		public async Task<int> DeleteByID(IDbTransaction transaction, string id)
		{
			string p = db.Symbol();

			string query = $"delete from {tableBase} where id = {p}ID";

			return await _command.DeleteByID(transaction, query, id);
		}

        #endregion

		

    }
}