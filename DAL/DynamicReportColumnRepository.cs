using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class DynamicReportColumnRepository : BaseRepository, IDynamicReportColumnRepository
  {
    #region Tables
    private readonly string tableBase = "dynamic_report_column";
    private readonly string tableDynamicReportColumnOrder = "dynamic_report_column_order";
    private readonly string tableMasterDynamicReportColumn = "master_dynamic_report_column";
    private readonly string tableMasterDynamicReportTable = "master_dynamic_report_table";
    private readonly string tableDynamicReportTable = "dynamic_report_table";
    #endregion

    #region GetRowsByDynamicReport (transaction, keyword, offset, limit, dynamicReportID)
    public async Task<List<DynamicReportColumn>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drc.id as ID
          ,drc.dynamic_report_id    			as DynamicReportID
          ,drc.dynamic_report_table_id    as DynamicReportTableID
          ,drc.master_dynamic_report_column_id as MasterDynamicReportColumnID
          ,drc.header_title                    as HeaderTitle
          ,drc.order_key                       as OrderKey
          ,drc.group_function                  as GroupFunction
          ,drc.order_by                        as OrderBy
          ,drc.formula                         as Formula
          ,drt.alias                            as TableAlias
          ,mdrt.alias                           as TableName
          ,mdrc.alias                           as ColumnName
        from
          {tableBase} drc
        left join
					{tableMasterDynamicReportColumn} mdrc on mdrc.id = drc.master_dynamic_report_column_id
        left join 
					{tableDynamicReportTable} drt on drt.id = drc.dynamic_report_table_id
				left join
					{tableMasterDynamicReportTable} mdrt on mdrt.id = drt.master_dynamic_report_table_id and mdrt.id = mdrc.master_dynamic_report_table_id
        where
          drc.dynamic_report_id = {p}DynamicReportID
          and 
          (
            lower(mdrt.alias) like lower({p}Keyword)
            or lower(mdrc.alias) like lower({p}Keyword)
            or lower(drc.header_title) like lower({p}Keyword)
            or lower(drc.group_function) like lower({p}Keyword)
          )
        order by
          drc.order_key asc";

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        DynamicReportID = dynamicReportID
      };
      return await _command.GetRows<DynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsByDynamicReport (transaction, dynamicReportID)
    public async Task<List<DynamicReportColumn>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drc.id as ID
          ,drc.dynamic_report_id    			as DynamicReportID
          ,drc.dynamic_report_table_id    			as DynamicReportTableID
          ,drc.master_dynamic_report_column_id as MasterDynamicReportColumnID
          ,drc.header_title                    as HeaderTitle
          ,drc.order_key                       as OrderKey
          ,drc.group_function                  as GroupFunction
          ,drc.order_by                        as OrderBy
          ,drc.formula                         as Formula
          ,drt.alias                            as TableAlias
          ,mdrt.name                            as TableName
          ,mdrc.name                            as ColumnName
          ,mdrc.is_masking                      as IsMasking
        from
          {tableBase} drc
				left join
					{tableMasterDynamicReportColumn} mdrc on mdrc.id = drc.master_dynamic_report_column_id
        left join 
					{tableDynamicReportTable} drt on drt.id = drc.dynamic_report_table_id
				left join
					{tableMasterDynamicReportTable} mdrt on mdrt.id = drt.master_dynamic_report_table_id and mdrt.id = mdrc.master_dynamic_report_table_id
        where
          drc.dynamic_report_id = {p}DynamicReportID
        order by
          drc.order_key asc";

      var parameters = new
      {
        DynamicReportID = dynamicReportID
      };
      return await _command.GetRows<DynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region GetRows (transaction, keyword, offset, limit)
    public Task<List<DynamicReportColumn>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
        select
          dynamic_report_id as DynamicReportID
          ,master_dynamic_report_column_id as MasterDynamicReportColumnID
        from
          {tableBase}
        where
          (
            lower(title) like lower({p}Keyword)
          )
        order by
          mod_date desc ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };
      return _command.GetRows<DynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsForLookupExcludeByDynamicReport
    public async Task<List<DynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
          select
              drc.id                                 as ID
              ,drc.header_title                      as HeaderTitle
							,drt.alias                              as TableAlias
					from       
              {tableBase} drc
					left join 
							{tableDynamicReportTable} drt on drt.id = drc.dynamic_report_table_id
          left join 
              {tableDynamicReportColumnOrder} drco 
              on  drc.id = drco.dynamic_report_column_id 
          where 
              drco.id is null
          and
              drc.dynamic_report_id = {p}DynamicReportID
          and
              (    
                  lower(drc.header_title)                like lower({p}Keyword)
              )
          order by
                  drc.order_key asc"
      );

      var parameters = new
      {
        Keyword = "%" + keyword + "%",
        Offset = offset,
        Limit = limit,
        DynamicReportID = dynamicReportID
      };
      var result = await _command.GetRows<DynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowByID (transaction, ID)
    public Task<DynamicReportColumn> GetRowByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drc.dynamic_report_id as DynamicReportID
          ,drc.dynamic_report_table_id as DynamicReportTableID
          ,drc.master_dynamic_report_column_id as MasterDynamicReportColumnID
          ,mdrt.id as MasterDynamicReportTableID
          ,mdrt.alias as TableName
          ,mdrc.alias as ColumnName
        from
          {tableBase} drc
        left join
          {tableMasterDynamicReportColumn} mdrc on mdrc.id = drc.master_dynamic_report_column_id
        left join
          {tableMasterDynamicReportTable} mdrt on mdrt.id = mdrc.master_dynamic_report_table_id
        where
          drc.id = {p}ID";

      var parameters = new
      {
        ID = ID
      };
      return _command.GetRow<DynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region GetTopOrderByDynamicReport (transaction, limit, dynamicReportID)
    public Task<List<DynamicReportColumn>> GetTopOrderByDynamicReport(IDbTransaction transaction, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drc.order_key as OrderKey
        from
          {tableBase} drc
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
      return _command.GetRows<DynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region Insert (transaction, model)
    public async Task<int> Insert(IDbTransaction transaction, DynamicReportColumn model)
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
                        ,dynamic_report_id
                        ,dynamic_report_table_id
                        ,master_dynamic_report_column_id  
                        ,header_title
                        ,order_key
                        ,group_function
                        ,order_by
                        ,formula
                    ) values (
                        {p}ID
                        ,{p}CreDate
                        ,{p}CreBy
                        ,{p}CreIPAddress
                        ,{p}ModDate
                        ,{p}ModBy
                        ,{p}ModIPAddress
                        --
                        ,{p}DynamicReportID
                        ,{p}DynamicReportTableID
                        ,{p}MasterDynamicReportColumnID
                        ,{p}HeaderTitle
                        ,{p}OrderKey
                        ,{p}GroupFunction
                        ,{p}OrderBy
                        ,{p}Formula
                    )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateByID (transaction, model)
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReportColumn model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        master_dynamic_report_column_id = {p}MasterDynamicReportColumnID
                        ,header_title   = {p}HeaderTitle
                        ,group_function = {p}GroupFunction
                        ,order_by       = {p}OrderBy
                        ,formula        = {p}Formula
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
    public async Task<List<DynamicReportColumn>> GetRowsOrderByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
				select
					id				as ID
					,order_key as OrderKey
				from
					{tableBase} drc
				where
					drc.dynamic_report_id = {p}DynamicReportID
				order by
					order_key asc
        ";

      var parameters = new
      {
        DynamicReportID = dynamicReportID
      };

      return await _command.GetRows<DynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region ChangeOrder
    public async Task<int> ChangeOrder(IDbTransaction transaction, DynamicReportColumn model)
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
								count(*) 
							from 
								{tableBase} drc
							where
								drc.dynamic_report_id = {p}DynamicReportID
							";

      var parameters = new { DynamicReportID = dynamicReportID };
      var result = await _command.GetRow<int?>(transaction, query, parameters);
      return result ?? 0;
    }

    #endregion

    #region DeleteFormulaByTable (transaction, tableAlias)
    public Task<int> DeleteFormulaByTable(IDbTransaction transaction, string dynamicReportID, string tableAlias)
    {
      var p = db.Symbol();
      string query = $@"
        delete from {tableBase}
        where
          formula like {p}Formula
				and
					dynamic_report_id = {p}DynamicReportID
        ";

      var parameters = new
      {
        Formula = $"%{{{tableAlias}%",
        DynamicReportID = dynamicReportID
      };

      return _command.Delete(transaction, query, parameters);
    }
    #endregion
  }
}