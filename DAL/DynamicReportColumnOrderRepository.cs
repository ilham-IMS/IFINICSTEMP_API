using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class DynamicReportColumnOrderRepository : BaseRepository, IDynamicReportColumnOrderRepository
  {
    #region Tables
    private readonly string tableBase = "dynamic_report_column_order";
    private readonly string tableDynamicReportColumn = "dynamic_report_column";
    private readonly string tableDynamicReportTable = "dynamic_report_table";
    #endregion

    #region GetRowsByDynamicReport (transaction, keyword, offset, limit, dynamicReportID)
    public async Task<List<DynamicReportColumnOrder>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drco.id as ID
          ,drco.dynamic_report_column_id as DynamicReportColumnID
          ,drco.order_key                       as OrderKey
          ,drco.order_by                        as OrderBy
          ,drc.header_title                     as ColumnTitle
					,drt.alias                            as TableAlias
        from
          {tableBase} drco
        inner join
          {tableDynamicReportColumn} drc 
						on drc.id = drco.dynamic_report_column_id
				left join
					{tableDynamicReportTable} drt 
					on drt.id = drc.dynamic_report_table_id
        where
          drc.dynamic_report_id = {p}DynamicReportID
          and 
          (
            lower(drt.alias) like lower({p}Keyword)
            or lower(drc.header_title) like lower({p}Keyword)
            or lower(drco.order_by) like lower({p}Keyword)
          )
        order by
          drco.order_key asc";

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        DynamicReportID = dynamicReportID
      };
      return await _command.GetRows<DynamicReportColumnOrder>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsByDynamicReport (transaction, dynamicReportID)
    public async Task<List<DynamicReportColumnOrder>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drco.id as ID
          ,drco.dynamic_report_column_id as DynamicReportColumnID
          ,drco.order_key                       as OrderKey
          ,drco.order_by                        as OrderBy
          ,drc.header_title                     as ColumnTitle
        from
          {tableBase} drco
        inner join
          {tableDynamicReportColumn} drc 
						on drc.id = drco.dynamic_report_column_id
        where
          drc.dynamic_report_id = {p}DynamicReportID
        order by
          drco.order_key asc";

      var parameters = new
      {
        DynamicReportID = dynamicReportID
      };
      return await _command.GetRows<DynamicReportColumnOrder>(transaction, query, parameters);
    }
    #endregion

    #region GetRows (transaction, keyword, offset, limit)
    public Task<List<DynamicReportColumnOrder>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drco.id as ID
          ,drco.dynamic_report_column_id as DynamicReportColumnID
          ,drco.order_key                       as OrderKey
          ,drco.order_by                        as OrderBy
        from
          {tableBase} drco
        order by
          drco.order_key asc";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };
      return _command.GetRows<DynamicReportColumnOrder>(transaction, query, parameters);
    }
    #endregion

    #region GetRowByID (transaction, ID)
    public Task<DynamicReportColumnOrder> GetRowByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drco.id as ID
          ,drco.dynamic_report_column_id as DynamicReportColumnID
          ,drco.order_key                       as OrderKey
          ,drco.order_by                        as OrderBy
          ,drc.header_title                     as ColumnTitle
        from
          {tableBase} drco
        inner join
          {tableDynamicReportColumn} drc 
						on drc.id = drco.dynamic_report_column_id
        where
          drc.id = {p}ID";

      var parameters = new
      {
        ID = ID
      };
      return _command.GetRow<DynamicReportColumnOrder>(transaction, query, parameters);
    }
    #endregion

    #region GetTopOrderByDynamicReport (transaction, limit, dynamicReportID)
    public Task<List<DynamicReportColumnOrder>> GetTopOrderByDynamicReport(IDbTransaction transaction, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drco.order_key                       as OrderKey
        from
          {tableBase} drco
        inner join
          {tableDynamicReportColumn} drc 
						on drc.id = drco.dynamic_report_column_id
        where 
          drc.dynamic_report_id = {p}DynamicReportID
        order by
          drc.order_key desc ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Limit = limit,
        Offset = 0,
        DynamicReportID = dynamicReportID
      };
      return _command.GetRows<DynamicReportColumnOrder>(transaction, query, parameters);
    }
    #endregion

    #region Insert (transaction, model)
    public async Task<int> Insert(IDbTransaction transaction, DynamicReportColumnOrder model)
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
                        ,dynamic_report_column_id
                        ,order_key
                        ,order_by
                    ) values (
                        {p}ID
                        ,{p}CreDate
                        ,{p}CreBy
                        ,{p}CreIPAddress
                        ,{p}ModDate
                        ,{p}ModBy
                        ,{p}ModIPAddress
                        --
                        ,{p}DynamicReportColumnID
                        ,{p}OrderKey
                        ,{p}OrderBy
                    )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateByID (transaction, model)
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReportColumnOrder model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        order_by       = {p}OrderBy
                        --
                        ,mod_date         = {p}ModDate
                        ,mod_by           = {p}ModBy
                        ,mod_ip_address   = {p}ModIPAddress
                    where
                        id = {p}ID
                    ";
      var result = await _command.Update(transaction, query, model);
      return result;
    }
    #endregion

    #region DeleteByID (transaction, ID)
    public Task<int> DeleteByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        delete from {tableBase}
        where
          id = {p}ID
        ";
      return _command.DeleteByID(transaction, query, ID);
    }
    #endregion

    #region GetRowsOrder
    public async Task<List<DynamicReportColumnOrder>> GetRowsOrderByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
				select
					drco.id				as ID
					,drco.order_key as OrderKey
				from
					{tableBase} drco
        inner join
          {tableDynamicReportColumn} drc 
						on drc.id = drco.dynamic_report_column_id
				where
					drc.dynamic_report_id = {p}DynamicReportID
				order by
					drco.order_key asc
        ";

      var parameters = new
      {
        DynamicReportID = dynamicReportID
      };

      return await _command.GetRows<DynamicReportColumnOrder>(transaction, query, parameters);
    }
    #endregion

    #region ChangeOrder
    public async Task<int> ChangeOrder(IDbTransaction transaction, DynamicReportColumnOrder model)
    {
      var p = db.Symbol();
      string query = $@"
				update {tableBase}
				set
					order_key = {p}OrderKey
				where
					id = {p}ID
        ";
      return await _command.Update(transaction, query, model);
    }
    #endregion

    #region GetCountByDynamicReport
    public async Task<int> GetCountByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      string p = db.Symbol();
      string query = $@"
							select 
								count(drco.id) 
							from 
								{tableBase} drco
              inner join
                {tableDynamicReportColumn} drc 
                  on drc.id = drco.dynamic_report_column_id
              where
                drc.dynamic_report_id = {p}DynamicReportID
							";

      var parameters = new { DynamicReportID = dynamicReportID };
      var result = await _command.GetRow<int?>(transaction, query, parameters);
      return result ?? 0;
    }

    #endregion
  }
}