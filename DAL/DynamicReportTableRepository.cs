using System.Data;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class DynamicReportTableRepository : BaseRepository, IDynamicReportTableRepository
  {
    private readonly string tableBase = "dynamic_report_table";
    private readonly string tableMasterDynamicReportTable = "master_dynamic_report_table";
    private readonly string tableDynamicReportTableRelation = "dynamic_report_table_relation";
    private readonly string tableDynamicReport = "dynamic_report";

    #region GetRows
    public Task<List<DynamicReportTable>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drt.id as ID
          ,drt.dynamic_report_id as DynamicReportID
          ,drt.master_dynamic_report_table_id as MasterDynamicReportTableID
          ,drt.alias as Alias
          ,mdrt.alias as TableName
          ,drt.additional_join_clause                  as AdditionalJoinClause
          ,drt.is_table_reference                             as IsTableReference
        from
          {tableBase} drt
        left join
          {tableMasterDynamicReportTable} mdrt on mdrt.id = drt.master_dynamic_report_table_id
        where
          drt.dynamic_report_id = {p}DynamicReportID
        and
          (
            lower(drt.alias) like lower({p}Keyword)
            or lower(mdrt.alias) like lower({p}Keyword)
          )
        order by
          drt.mod_date desc";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        DynamicReportID = dynamicReportID
      };
      return _command.GetRows<DynamicReportTable>(transaction, query, parameters);
    }

    public Task<List<DynamicReportTable>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
        select
          dynamic_report_id as DynamicReportID
          ,master_dynamic_report_table_id as MasterDynamicReportTableID
          ,additional_join_clause                  as AdditionalJoinClause
          ,is_table_reference                             as IsTableReference
        from
          {tableBase}
        where
          (
            title like {p}Keyword
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
      return _command.GetRows<DynamicReportTable>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsByDynamicReport
    public Task<List<DynamicReportTable>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
				drt.id                               as ID
          ,drt.master_dynamic_report_table_id  as MasterDynamicReportTableID
          ,drt.reference_dynamic_report_table_id  as ReferenceDynamicReportTableID
          ,drt.dynamic_report_id               as DynamicReportID
          ,drt.alias                            as Alias
          ,mdrt.name                            as TableName
          ,drt.join_clause                     as JoinClause
          ,mdrt_ref.alias as ReferenceTableAlias
          ,drt.is_table_reference                             as IsTableReference
        from
          {tableBase} drt
        left join
          {tableMasterDynamicReportTable} mdrt on mdrt.id = drt.master_dynamic_report_table_id
        left join {tableBase} drt_ref on (drt_ref.id = drt.reference_dynamic_report_table_id )
        left join {tableMasterDynamicReportTable} mdrt_ref on (mdrt_ref.id = drt_ref.master_dynamic_report_table_id )
        where
          drt.dynamic_report_id = {p}DynamicReportID";

      var parameters = new
      {
        DynamicReportID = dynamicReportID
      };
      return _command.GetRows<DynamicReportTable>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsExclude
    public Task<List<DynamicReportTable>> GetRowsExclude(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drt.id as ID
          ,drt.master_dynamic_report_table_id as MasterDynamicReportTableID
          ,drt.reference_dynamic_report_table_id  as ReferenceDynamicReportTableID
          ,drt.dynamic_report_id as DynamicReportTableID
          ,drt.alias as Alias
          ,mdrt.name as TableName
          ,drt.additional_join_clause                  as AdditionalJoinClause
          ,drt.is_table_reference                             as IsTableReference
        from
          {tableBase} drt
        inner join
          (
            select dr.id
            from  {tableDynamicReport} dr
            inner join {tableBase} drt on drt.dynamic_report_id = dr.id and drt.id = {p}DynamicReportTableID
          ) dr on dr.id = drt.dynamic_report_id
        left join
          {tableMasterDynamicReportTable} mdrt on mdrt.id = drt.master_dynamic_report_table_id
        where
          drt.id != {p}DynamicReportTableID
        and
          (
            mdrt.name like {p}Keyword
            or drt.alias like {p}Keyword
          )
        order by
          drt.alias asc";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        DynamicReportTableID = dynamicReportTableID,
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };
      return _command.GetRows<DynamicReportTable>(transaction, query, parameters);
    }

    #endregion

    #region GetRowByID
    public Task<DynamicReportTable> GetRowByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drt.id                                          as ID
          ,drt.dynamic_report_id                          as DynamicReportID
          ,drt.master_dynamic_report_table_id             as MasterDynamicReportTableID
          ,drt.reference_dynamic_report_table_id  as ReferenceDynamicReportTableID
          ,drt.alias                                       as Alias
          ,drt.join_clause                                as JoinClause
          ,mdrt.alias                                      as TableName
          ,mdrt2.alias                                      as ReferenceTableAlias
          ,drt.is_table_reference                             as IsTableReference
          ,(
              select 	count(drtr.id) 
              from 	{tableDynamicReportTableRelation} drtr
              where	drtr.dynamic_report_table_id = {p}ID
            ) 									                           as TotalRelation
          ,drt.additional_join_clause                  as AdditionalJoinClause
        from
          {tableBase} drt
        left join
          {tableMasterDynamicReportTable} mdrt on mdrt.id = drt.master_dynamic_report_table_id
        left join
          {tableBase} mdrt2 on mdrt2.id = drt.reference_dynamic_report_table_id
        where
          drt.id = {p}ID";

      var parameters = new
      {
        ID = ID
      };
      return _command.GetRow<DynamicReportTable>(transaction, query, parameters);
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, DynamicReportTable model)
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
                        ,master_dynamic_report_table_id  
                        ,reference_dynamic_report_table_id
                        ,alias
                        ,join_clause
                        ,is_table_reference
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
                        ,{p}MasterDynamicReportTableID
                        ,{p}ReferenceDynamicReportTableID
                        ,{p}Alias
                        ,{p}JoinClause
                        ,{p}IsTableReference
                    )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateByID

    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReportTable model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        master_dynamic_report_table_id      = {p}MasterDynamicReportTableID
                        ,reference_dynamic_report_table_id  = {p}ReferenceDynamicReportTableID
                        ,alias                              = {p}Alias
                        ,join_clause                        = {p}JoinClause
                        ,is_table_reference                 = {p}IsTableReference
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

    #region UpdateJoinClauseByID

    public async Task<int> UpdateJoinClauseByID(IDbTransaction transaction, DynamicReportTable model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        join_clause                          = {p}JoinClause 
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

    #region DeleteByID
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

    #region CheckTableExists
    public Task<int> CheckTableExists(IDbTransaction transaction, string MasterDynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        select
				  count(1)
        from
          {tableBase} drt
        where
          drt.master_dynamic_report_table_id = {p}MasterDynamicReportTableID";

      var parameters = new
      {
        MasterDynamicReportTableID
      };
      return _command.GetRow<int>(transaction, query, parameters);
    }
    #endregion

    #region GetTitle
    public Task<string> GetTitle(IDbTransaction transaction, string MasterDynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        select title    as Title
        from {tableBase} drt 
        inner join {tableDynamicReport} dr on dr.id = drt.dynamic_report_id 
        where master_dynamic_report_table_id = {p}MasterDynamicReportTableID
          ";

      var parameters = new
      {
        MasterDynamicReportTableID
      };
      return _command.GetRow<string>(transaction, query, parameters);
    }
    #endregion

    #region Count Table
    public async Task<int> GetCountByAliasName(IDbTransaction transaction, string dynamicReportID, string aliasName)
    {
      var p = db.Symbol();
      string query = $@"
              select 
                count(*) 
              from 
                {tableBase} drt
              where
                drt.dynamic_report_id = {p}DynamicReportID
              and
                drt.alias = {p}AliasName
              ";

      var parameters = new { DynamicReportID = dynamicReportID, AliasName = aliasName };
      var result = await _command.GetRow<int?>(transaction, query, parameters);
      return result ?? 0;
    }
    #endregion


    #region GetRowsForValidateTableRelation
    public Task<List<DynamicReportTable>> GetRowsForValidateTableRelation(IDbTransaction transaction, string dynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        select 
            drt.alias as Alias
        from dynamic_report_table_relation drtr
        inner join dynamic_report_table drt 
            on drt.id = drtr.dynamic_report_table_id
        where drt.is_table_reference = 1
          and (coalesce(drtr.source_master_dynamic_report_column_value, '') = '' 
              or coalesce(drtr.reference_master_dynamic_report_column_value, '') = '')
          and drt.dynamic_report_id = {p}DynamicReportTableID";

      var parameters = new
      {
        DynamicReportTableID = dynamicReportTableID
      };
      return _command.GetRows<DynamicReportTable>(transaction, query, parameters);
    }
    #endregion
  }
}