using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
	public class MasterDashboardRepository : BaseRepository, IMasterDashboardRepository
	{
		private readonly string tableBase = "master_dashboard";

		#region GetRows
		public async Task<List<MasterDashboard>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
		{
			string p = db.Symbol();

			string query = $@"
							select
								{tableBase}.id AS ID
								,{tableBase}.code AS Code
								,{tableBase}.dashboard_name AS DashboardName
								,{tableBase}.dashboard_type AS DashboardType
								,{tableBase}.dashboard_grid AS DashboardGrid
								,{tableBase}.sp_name AS SPName
								,{tableBase}.is_active AS IsActive
								,{tableBase}.is_editable AS IsEditable
							from
								{tableBase}
							where
								(
									lower({tableBase}.code) like lower({p}Keyword)
									or lower({tableBase}.dashboard_name) like lower({p}Keyword)
									or lower({tableBase}.dashboard_type) like lower({p}Keyword)
									or lower({tableBase}.dashboard_grid) like lower({p}Keyword)
									or lower({tableBase}.sp_name) like lower({p}Keyword)
									or case {tableBase}.is_active
											when 1 then 'yes'
											else 'no'
										end like lower({p}Keyword)
									or case {tableBase}.is_editable
											when 1 then 'yes'
											else 'no'
										end like lower({p}Keyword)
								)
							order by
								{tableBase}.mod_date desc
					";

			query = QueryLimitOffset(query);

			object parameters = new
			{
				Keyword = $"%{keyword}%",
				Offset = offset,
				Limit = limit
			};

			List<MasterDashboard> result = await _command.GetRows<MasterDashboard>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region GetRowsForLookupExcludeByID
		public async Task<List<MasterDashboard>> GetRowsForLookupExcludeByID(IDbTransaction transaction, string? keyword, int offset, int limit, string[] ID)
		{
			string p = db.Symbol();

			if (ID.Length == 0)
			{
				ID = [""];
			}

			string idParam = string.Join(",", ID.Select((x, i) => $"{p}ID{i}")); // @ID0, @ID1, @ID2

			string query = $@"
							select
								{tableBase}.id AS ID
								,{tableBase}.code AS Code
								,{tableBase}.dashboard_name AS DashboardName
							from
								{tableBase}
							where
								{tableBase}.id not in ({idParam})
								and
								{tableBase}.is_active = 1
								and
								(
									lower({tableBase}.code) like lower({p}Keyword)
									or lower({tableBase}.dashboard_name) like lower({p}Keyword)
								)
							order by
								{tableBase}.mod_date desc
					";

			query = QueryLimitOffset(query);

			Dictionary<string, object> parameters = new(){
				{"Keyword", $"%{keyword}%"},
				{"Offset", offset},
				{"Limit", limit}
			};

			foreach (var (id, i) in ID.Select((x, i) => (x, i)))
			{
				parameters.Add($"ID{i}", id);
			}

			List<MasterDashboard> result = await _command.GetRows<MasterDashboard>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region GetRowByID
		public async Task<MasterDashboard> GetRowByID(IDbTransaction transaction, string id)
		{
			string p = db.Symbol();

			string query = $@"
							select
								{tableBase}.id AS ID
								,{tableBase}.code AS Code
								,{tableBase}.dashboard_name AS DashboardName
								,{tableBase}.dashboard_type AS DashboardType
								,{tableBase}.dashboard_grid AS DashboardGrid
								,{tableBase}.sp_name AS SPName
								,{tableBase}.is_active AS IsActive
								,{tableBase}.is_editable AS IsEditable
							from
								{tableBase}
							where
								{tableBase}.id = {p}ID
					";

			object parameters = new
			{
				ID = id
			};

			MasterDashboard result = await _command.GetRow<MasterDashboard>(transaction, query, parameters);

			return result;
		}
		#endregion

		#region Insert
		public async Task<int> Insert(IDbTransaction transaction, MasterDashboard model)
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
								,dashboard_name
								,dashboard_type
								,dashboard_grid
								,sp_name
								,is_active
								,is_editable
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
								,{p}DashboardName
								,{p}DashboardType
								,{p}DashboardGrid
								,{p}SPName
								,{p}IsActive
								,{p}IsEditable
							)
					";

			return await _command.Insert(transaction, query, model);
		}
		#endregion

		#region UpdateByID
		public async Task<int> UpdateByID(IDbTransaction transaction, MasterDashboard model)
		{
			string p = db.Symbol();

			string query = $@"update {tableBase}
							set
								code = {p}Code
								,dashboard_name = {p}DashboardName
								,dashboard_type = {p}DashboardType
								,dashboard_grid = {p}DashboardGrid
								,sp_name = {p}SPName
								,is_active = {p}IsActive
								,is_editable = {p}IsEditable
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

		#region ChangeStatus
		public async Task<int> ChangeStatus(IDbTransaction transaction, MasterDashboard model)
		{
			var p = db.Symbol();

			string query = $@"
				update {tableBase} 
				set
					is_active       = is_active * -1
					--
					,mod_date       = {p}ModDate
					,mod_by         = {p}ModBy
					,mod_ip_address = {p}ModIpAddress
				where
					id = {p}ID
			";

			return await _command.Update(transaction, query, model);
		}
		#endregion

		#region ChangeEditableStatus
		public async Task<int> ChangeEditableStatus(IDbTransaction transaction, MasterDashboard model)
		{
			var p = db.Symbol();

			string query = $@"
				update {tableBase} 
				set
					is_editable       = is_editable * -1
					--
					,mod_date       = {p}ModDate
					,mod_by         = {p}ModBy
					,mod_ip_address = {p}ModIpAddress
				where
					id = {p}ID
			";

			return await _command.Update(transaction, query, model);
		}
		#endregion

		public async Task<List<MasterDashboard>> GetReportData(IDbTransaction transaction)
		{
			string query =
																			$@"
                                select
                                         id                         AS ID
                                        ,code 											AS Code
																				,dashboard_name 						AS DashboardName
																				,dashboard_type 						AS DashboardType
																				,dashboard_grid 						AS DashboardGrid
																				,is_active 									AS IsActive
                                from
                                    {tableBase}
                                order by
                                    mod_date desc";
			var result = await _command.GetRows<MasterDashboard>(
							transaction,
							query);
			return result;
		}
	}
}