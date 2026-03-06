using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
	public class SysJobTaskRepository : BaseRepository, ISysJobTaskRepository
	{
		private readonly string tableBase = "sys_job_task";

		#region GetRows
		public async Task<List<SysJobTask>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
		{
			string p = db.Symbol();

			string query = $@"
							select
									 id 					as ID
									,code 					as Code
									,row_to_process 		as RowToProcess
							from
								{tableBase}
							where
								(
									lower(code) like lower({p}Keyword)
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

			List<SysJobTask> result = await _command.GetRows<SysJobTask>(transaction, query, parameters);

			return result;
		}

		#endregion

		#region GetRowByID
		public async Task<SysJobTask> GetRowByID(IDbTransaction transaction, string id)
		{
			string p = db.Symbol();

			string query = $@"
							select
									 id 					as ID
									,code 					as Code
									,row_to_process 		as RowToProcess
							from
								{tableBase}
							where
								id = {p}ID
					";

			object parameters = new
			{
				ID = id
			};

			SysJobTask result = await _command.GetRow<SysJobTask>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region Insert
		public async Task<int> Insert(IDbTransaction transaction, SysJobTask model)
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
								,code
								,job_status
								,row_to_process
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
								,{p}Code
								,{p}JobStatus
								,{p}RowToProcess
							)
					";

			return await _command.Insert(transaction, query, model);
		}
		#endregion

		#region UpdateByID
		public async Task<int> UpdateByID(IDbTransaction transaction, SysJobTask model)
		{
			string p = db.Symbol();

			string query = $@"update {tableBase}
							set
								 code  			= {p}code
								,job_status 	= {p}JobStatus
								,row_to_process = {p}RowToProcess
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

		public async Task<SysJobTask> GetRowByCode(IDbTransaction transaction, string code)
		{
			string p = db.Symbol();

			string query = $@"
							select
									 id 					as ID
									,code 					as Code
									,row_to_process 		as RowToProcess
							from
									{tableBase}
							where
									code = {p}Code
					";

			object parameters = new
			{
				Code = code
			};

			SysJobTask result = await _command.GetRow<SysJobTask>(transaction, query, parameters);

			return result;
		}
		#endregion

		public async Task<int> UpdateJobStatus(IDbTransaction transaction, SysJobTask model)
		{
			string p = db.Symbol();

			string query = $@"update {tableBase}
							set
								job_status 	= {p}JobStatus
							where
								id = {p}ID
										";
			return await _command.Update(transaction, query, model);
		}


	}
}