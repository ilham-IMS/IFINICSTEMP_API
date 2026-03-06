using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
	public class MasterDashboardUserRepository : BaseRepository, IMasterDashboardUserRepository
	{
		private readonly string tableBase = "master_dashboard_user";
		private readonly string tableDashboard = "master_dashboard";

		#region GetRows
		public async Task<List<MasterDashboardUser>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string userID)
		{
			string p = db.Symbol();

			string query = $@"
							select
								{tableBase}.id AS ID
								,{tableBase}.user_id AS UserID
								,{tableBase}.master_dashboard_id AS MasterDashboardID
								,{tableBase}.order_key AS OrderKey
								,{tableDashboard}.dashboard_name AS DashboardName
								,{tableDashboard}.dashboard_grid AS DashboardGrid
							from
								{tableBase}
							inner join
								{tableDashboard} on {tableBase}.master_dashboard_id = {tableDashboard}.id
							where
                {tableBase}.user_id = {p}UserID
                and
								(
									cast({tableBase}.order_key AS varchar) like lower({p}Keyword)
									or lower({tableDashboard}.dashboard_name) like lower({p}Keyword)
									or lower({tableDashboard}.dashboard_grid) like lower({p}Keyword)
								)
							order by
								{tableBase}.order_key asc
					";

			query = QueryLimitOffset(query);

			object parameters = new
			{
				Keyword = $"%{keyword}%",
				Offset = offset,
				Limit = limit,
				UserID = userID
			};

			List<MasterDashboardUser> result = await _command.GetRows<MasterDashboardUser>(transaction, query, parameters);

			return result;
		}
		public async Task<List<MasterDashboardUser>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
		{
			string p = db.Symbol();

			string query = $@"
							select
								{tableBase}.id AS ID
								,{tableBase}.user_id AS UserID
								,{tableBase}.master_dashboard_id AS MasterDashboardID
								,{tableBase}.order_key AS OrderKey
								,{tableDashboard}.dashboard_name AS DashboardName
								,{tableDashboard}.dashboard_grid AS DashboardGrid
							from
								{tableBase}
							inner join
								{tableDashboard} on {tableBase}.master_dashboard_id = {tableDashboard}.id
							where
								(
									cast({tableBase}.order_key AS varchar) like lower({p}Keyword)
									or lower({tableDashboard}.dashboard_name) like lower({p}Keyword)
									or lower({tableDashboard}.dashboard_grid) like lower({p}Keyword)
								)
							order by
								{tableBase}.order_key asc
					";

			query = QueryLimitOffset(query);

			object parameters = new
			{
				Keyword = $"%{keyword}%",
				Offset = offset,
				Limit = limit
			};

			List<MasterDashboardUser> result = await _command.GetRows<MasterDashboardUser>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region GetRowByID
		public async Task<MasterDashboardUser> GetRowByID(IDbTransaction transaction, string id)
		{
			string p = db.Symbol();

			string query = $@"
							select
								{tableBase}.id AS ID
								,{tableBase}.user_id AS UserID
								,{tableBase}.master_dashboard_id AS MasterDashboardID
								,{tableBase}.order_key AS OrderKey
							from
								{tableBase}
							where
								{tableBase}.id = {p}ID
					";

			object parameters = new
			{
				ID = id
			};

			MasterDashboardUser result = await _command.GetRow<MasterDashboardUser>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region Insert
		public async Task<int> Insert(IDbTransaction transaction, MasterDashboardUser model)
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
								,user_id
								,master_dashboard_id
								,order_key
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
								,{p}UserID
								,{p}MasterDashboardID
								,{p}OrderKey
							)
					";

			return await _command.Insert(transaction, query, model);
		}
		#endregion

		#region UpdateByID
		public async Task<int> UpdateByID(IDbTransaction transaction, MasterDashboardUser model)
		{
			string p = db.Symbol();

			string query = $@"update {tableBase}
							set
								order_key = {p}OrderKey
								--
								,mod_date = {p}ModDate
								,mod_by = {p}ModBy
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