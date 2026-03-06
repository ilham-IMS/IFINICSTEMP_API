using System.Data;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class DynamicReportTableRelationRepository : BaseRepository, IDynamicReportTableRelationRepository
  {
    private readonly string tableBase = "dynamic_report_table_relation";
    private readonly string tableDynamicReportTable = "dynamic_report_table";
    private readonly string tableMasterDynamicReportColumn = "master_dynamic_report_column";
    private readonly string tableMasterDynamicReportTable = "master_dynamic_report_table";

    #region GetRows
    public Task<List<DynamicReportTableRelation>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drtr.id                                       as ID
          ,drtr.dynamic_report_table_id                 as DynamicReportTableID
          ,drtr.source_master_dynamic_report_column_value         as SourceMasterDynamicReportColumnValue
          ,drtr.reference_master_dynamic_report_column_value         as ReferenceMasterDynamicReportColumnValue
          ,drtr.operator as Operator 
        from
          {tableBase} drtr
        where
          drtr.dynamic_report_table_id = {p}DynamicReportTableID
          and
            (
              lower(coalesce(drtr.source_master_dynamic_report_column_value, ''))       like lower({p}Keyword)
              or lower(coalesce(drtr.reference_master_dynamic_report_column_value, ''))     like lower({p}Keyword)
              or lower(coalesce(drtr.operator, ''))   like lower({p}Keyword)
            )
        order by
          drtr.cre_date asc";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        DynamicReportTableID = dynamicReportTableID
      };
      return _command.GetRows<DynamicReportTableRelation>(transaction, query, parameters);
    }
    public Task<List<DynamicReportTableRelation>> GetRows(IDbTransaction transaction, string dynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
         select
          drtr.id                                       as ID
          ,drtr.dynamic_report_table_id                 as DynamicReportTableID    
          ,drtr.source_master_dynamic_report_column_value         as SourceMasterDynamicReportColumnValue
          ,drtr.reference_master_dynamic_report_column_value         as ReferenceMasterDynamicReportColumnValue
          ,drtr.source_master_dynamic_report_column_id         as SourceMasterDynamicReportColumnID
          ,drtr.reference_master_dynamic_report_column_id         as ReferenceMasterDynamicReportColumnID
          ,drtr.operator as Operator 
          ,mdrt.alias as ReferenceTableAlias
          ,mdrc_source.name                            as ColumnName
          ,mdrc_ref.name                               as ReferenceColumnName
          ,COALESCE(drt.reference_dynamic_report_table_id, drt_ref.reference_dynamic_report_table_id) as ReferenceDynamicReportTableID
        from
          {tableBase} drtr
          left join {tableDynamicReportTable} drt on (drt.id = drtr.dynamic_report_table_id)
          left join {tableDynamicReportTable} drt_ref on (drt_ref.id = drt.reference_dynamic_report_table_id )
          left join {tableMasterDynamicReportTable} mdrt on (mdrt.id = drt_ref.master_dynamic_report_table_id )
          left join {tableMasterDynamicReportColumn} mdrc_source on (mdrc_source.id = drtr.source_master_dynamic_report_column_id )
          left join {tableMasterDynamicReportColumn} mdrc_ref on (mdrc_ref.id = drtr.reference_master_dynamic_report_column_id )
        where
          drtr.dynamic_report_table_id = {p}DynamicReportTableID
        order by
          drtr.cre_date asc";

      var parameters = new
      {
        DynamicReportTableID = dynamicReportTableID
      };
      return _command.GetRows<DynamicReportTableRelation>(transaction, query, parameters);
    }

    public Task<List<DynamicReportTableRelation>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drtr.id                                         as ID
          ,drtr.dynamic_report_table_id                   as DynamicReportTableID
          ,drtr.master_dynamic_report_column_id           as MasterDynamicReportColumnID
          ,drtr.reference_dynamic_report_table_id         as ReferenceDynamicReportTableID
          ,drtr.reference_master_dynamic_report_column_id as ReferenceMasterDynamicReportColumnID
          ,mdrc.alias                                     as ColumnAlias
          ,drt_ref.alias                                   as ReferenceTableAlias
          ,mdrc_ref.alias                                 as ReferenceColumnAlias
          ,drt_ref.additional_join_clause                  as AdditionalJoinClause
        from
          {tableBase} drtr
        inner join
          {tableMasterDynamicReportColumn} mdrc on mdrc.id = drtr.master_dynamic_report_column_id
        inner join
          {tableDynamicReportTable} drt_ref on drt_ref.id = drtr.reference_dynamic_report_table_id
        inner join
          {tableMasterDynamicReportColumn} mdrc_ref on mdrc_ref.id = drtr.reference_master_dynamic_report_column_id
        where
          drtr.dynamic_report_table_id = {p}DynamicReportTableID
          and
            (
              or (mdrc.alias)       like ({p}Keyword)
              or (drt_ref.alias)     like ({p}Keyword)
              or (mdrc_ref.alias)   like ({p}Keyword)
            )
        order by
          mdrc.alias asc";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };
      return _command.GetRows<DynamicReportTableRelation>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsByReference
    public Task<List<DynamicReportTableRelation>> GetRowsByReference(IDbTransaction transaction, string? keyword, int offset, int limit, string referenceDynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drtr.id                                       as ID
          ,drtr.dynamic_report_table_id                 as DynamicReportTableID
          ,drtr.master_dynamic_report_column_id         as MasterDynamicReportColumnID
          ,drtr.reference_dynamic_report_table_id         as ReferenceDynamicReportTableID
          ,drtr.reference_master_dynamic_report_column_id as ReferenceMasterDynamicReportColumnID
          ,drt.alias                                    as TableAlias
          ,drt.join_clause                              as TableJoinClause
          ,mdrc.alias                                   as ColumnAlias
          ,drt_ref.alias                                 as ReferenceTableAlias
          ,mdrc_ref.alias                                as ReferenceColumnAlias
          ,drt_ref.additional_join_clause                  as AdditionalJoinClause
        from
          {tableBase} drtr
        inner join
          {tableDynamicReportTable} drt on drt.id = drtr.dynamic_report_table_id
        inner join
          {tableMasterDynamicReportColumn} mdrc on mdrc.id = drtr.master_dynamic_report_column_id
        inner join
          {tableDynamicReportTable} drt_ref on drt_ref.id = drtr.reference_dynamic_report_table_id
        inner join
          {tableMasterDynamicReportColumn} mdrc_ref on mdrc_ref.id = drtr.reference_master_dynamic_report_column_id
        where
          drtr.reference_dynamic_report_table_id = {p}ReferenceDynamicReportTableID
          and
            (
              (mdrc.alias)       like ({p}Keyword)
              or (drt_ref.alias)     like ({p}Keyword)
              or (mdrc_ref.alias)   like ({p}Keyword)
            )
        order by
          mdrc.alias asc";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        ReferenceDynamicReportTableID = referenceDynamicReportTableID
      };
      return _command.GetRows<DynamicReportTableRelation>(transaction, query, parameters);
    }
    #endregion

    #region GetRowByID
    public Task<DynamicReportTableRelation> GetRowByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        drtr.id as ID
          ,drtr.dynamic_report_table_id                    as DynamicReportTableID
          ,drtr.master_dynamic_report_column_id            as MasterDynamicReportColumnID
          ,drtr.reference_dynamic_report_table_id          as ReferenceDynamicReportTableID
          ,drtr.reference_master_dynamic_report_column_id  as ReferenceMasterDynamicReportColumnID
          ,mdrc.name                                       as ColumnName
          ,mdrc.alias                                      as ColumnAlias
          ,drt_ref.table_name                              as ReferenceTableName
          ,drt_ref.alias                                    as ReferenceTableAlias
          ,mdrc_ref.name                                   as ReferenceColumnAlias
          ,mdrc_ref.alias                                  as ReferenceColumnAlias
          ,drt_ref.additional_join_clause                  as AdditionalJoinClause
        from
          {tableBase} drtr
        inner join
          {tableMasterDynamicReportColumn} mdrc on mdrc.id = drtr.master_dynamic_report_column_id
        inner join
          {tableDynamicReportTable} drt_ref on drt_ref.id = drtr.reference_dynamic_report_table_id
        inner join
          {tableMasterDynamicReportColumn} mdrc_ref on mdrc_ref.id = drtr.reference_master_dynamic_report_column_id
        where
          drut.id = {p}ID";

      var parameters = new
      {
        ID = ID
      };
      return _command.GetRow<DynamicReportTableRelation>(transaction, query, parameters);
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, DynamicReportTableRelation model)
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
                        ,dynamic_report_table_id
                        ,source_master_dynamic_report_column_value  
                        ,reference_master_dynamic_report_column_value
                        ,source_master_dynamic_report_column_id
                        ,reference_master_dynamic_report_column_id
                        ,operator  
                    ) values (
                        {p}ID
                        ,{p}CreDate
                        ,{p}CreBy
                        ,{p}CreIPAddress
                        ,{p}ModDate
                        ,{p}ModBy
                        ,{p}ModIPAddress
                        --
                        ,{p}DynamicReportTableID
                        ,{p}SourceMasterDynamicReportColumnValue
                        ,{p}ReferenceMasterDynamicReportColumnValue
                        ,{p}Operator
                        ,{p}SourceMasterDynamicReportColumnID
                        ,{p}ReferenceMasterDynamicReportColumnID
                    )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReportTableRelation model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        dynamic_report_table_id                     = {p}DynamicReportTableID
                        ,source_master_dynamic_report_column_value            = {p}SourceMasterDynamicReportColumnValue
                        ,reference_master_dynamic_report_column_value          = {p}ReferenceMasterDynamicReportColumnValue
                        ,source_master_dynamic_report_column_id            = {p}SourceMasterDynamicReportColumnID
                        ,reference_master_dynamic_report_column_id          = {p}ReferenceMasterDynamicReportColumnID
                        ,operator  = {p}Operator
                        --
                        ,mod_date                                   = {p}ModDate
                        ,mod_by                                     = {p}ModBy
                        ,mod_ip_address                             = {p}ModIPAddress
                    where
                        id = {p}ID
                    ";
      var result = await _command.Update(transaction, query, model);
      return result;
    }

    public async Task<int> UpdateReferenceColumn(IDbTransaction transaction, DynamicReportTableRelation model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        reference_master_dynamic_report_column_value          = {p}ReferenceMasterDynamicReportColumnValue
                        ,reference_master_dynamic_report_column_id          = {p}ReferenceMasterDynamicReportColumnID
                        --
                        ,mod_date                                   = {p}ModDate
                        ,mod_by                                     = {p}ModBy
                        ,mod_ip_address                             = {p}ModIPAddress
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

    #region DeleteByID
    public Task<int> DeleteReferenceDetail(IDbTransaction transaction, string DynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        delete from {tableBase}
        where
          dynamic_report_table_id = {p}DynamicReportTableID
        ";
      return _command.Delete(transaction, query, new
      {
        DynamicReportTableID = DynamicReportTableID
      });
    }
    #endregion

    #region DeleteByReferenceDynamicReportTableID
    public Task<int> DeleteByReferenceDynamicReportTableID(IDbTransaction transaction, string ReferenceDynamicReportTableID)
    {
      var p = db.Symbol();
      string query = $@"
        delete from {tableBase}
        where
          reference_dynamic_report_table_id = {p}ReferenceDynamicReportTableID
        ";
      return _command.Delete(transaction, query, new
      {
        ReferenceDynamicReportTableID = ReferenceDynamicReportTableID
      });
    }
    #endregion
  }
}