
using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class MasterDynamicReportColumnRepository : BaseRepository, IMasterDynamicReportColumnRepository
  {
    private string tableBase = "master_dynamic_report_column";
    private readonly string tableMasterDynamicReportTable = "MASTER_DYNAMIC_REPORT_TABLE";
    private readonly string tableInformationSchemaColumn = "vw_ifin_columns";
    private readonly string tableDynamicReport = "dynamic_report";
    private readonly string tableDynamicReportTable = "dynamic_report_table";
    private readonly string tableDynamicReportColumn = "dynamic_report_column";
    private readonly string viewColumn = "vw_ifin_columns";

    #region DeleteByID
    public async Task<int> DeleteByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();

      string query =
          $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      var result = await _command.DeleteByID(transaction, query, ID);
      return result;
    }
    #endregion

    #region GetRowByID
    public async Task<MasterDynamicReportColumn> GetRowByID(IDbTransaction transaction, string ID)
    {
      string p = db.Symbol();
      string query = $@"
        select
          id                              as ID
          ,master_dynamic_report_table_id as MasterDynamicReportTableID
          ,name                              as Name
          ,alias                             as Alias
          ,type                              as Type
          ,order_key                         as OrderKey
          ,is_available                      as IsAvailable
          ,is_masking                        as IsMasking
        from
          {tableBase}
        where
          id = {p}ID
      ";

      var parameters = new
      {
        ID = ID
      };

      return await _command.GetRow<MasterDynamicReportColumn>(transaction, query, parameters);
    }
    #endregion

    #region GetRows
    public async Task<List<MasterDynamicReportColumn>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string masterDynamicReportTableID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                id                                 as ID
                                ,master_dynamic_report_table_id    as MasterDynamicReportTableID
                                ,name                              as Name
                                ,alias                             as Alias
                                ,type                              as Type
                                ,order_key                         as OrderKey
                                ,is_available                      as IsAvailable
                                ,is_masking                        as IsMasking
                            from       
                                {tableBase}    
                            where 
                                master_dynamic_report_table_id = {p}MasterDynamicReportTableID
                            and
                                (    
                                    lower(name)                    like lower({p}Keyword)
                                    or lower(alias)                like lower({p}Keyword)
                                )
                            order by
                                    order_key asc"
      );
      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit,
            MasterDynamicReportTableID = masterDynamicReportTableID
          }
      );
      return result;
    }
    #endregion

    #region GetRowsByTables
    public async Task<List<MasterDynamicReportColumn>> GetRowsByTables(IDbTransaction transaction, IEnumerable<string> masterDynamicReportTableID)
    {
      var p = db.Symbol();

      if (!masterDynamicReportTableID.Any()) masterDynamicReportTableID = [""];

      var masterDynamicReportTableIDs = masterDynamicReportTableID.Select((x, i) => $"{p}MasterDynamicReportTableID{i}").Aggregate((x, y) => $"{x},{y}"); // {p}MasterDynamicReportTableID0, {p}MasterDynamicReportTableID1, ...

      string query =
          $@"
                            select
                                mdrc.id                                 as ID
                                ,mdrc.master_dynamic_report_table_id    as MasterDynamicReportTableID
                                ,mdrc.name                              as Name
                                ,mdrc.alias                             as Alias
                                ,mdrc.is_masking                        as IsMasking
                                ,isc.table_reference                    as TableReference   
                                ,mdrt_ref.id                            as TableReferenceID   
                                ,isc.column_reference                   as ColumnReference
                            from       
                                {tableBase} mdrc
                            inner join
                                {tableMasterDynamicReportTable} mdrt on mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID = mdrt.ID
                            inner join
                                {tableInformationSchemaColumn} isc on lower(isc.column_name) = lower(mdrc.name) and lower(isc.table_name) = lower(mdrt.name)
														left join 
																{tableMasterDynamicReportTable} mdrt_ref on lower(mdrt_ref.name) = lower(isc.table_reference)
                            where 
                                master_dynamic_report_table_id in ({masterDynamicReportTableIDs})
                            order by
                                    mdrc.order_key asc"
      ;

      var parameters = new Dictionary<string, object>();

      foreach (var (id, index) in masterDynamicReportTableID.Select((x, i) => (x, i)))
      {
        parameters.Add($"MasterDynamicReportTableID{index}", id);
      }
      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowsForeignReferenceToTable
    public async Task<List<MasterDynamicReportColumn>> GetRowsForeignReferenceToTable(IDbTransaction transaction, string dynamicReportID, string masterDynamicReportTableID)
    {
      var p = db.Symbol();

      string query =
          $@"
                            select
                                mdrc.id                                 as ID
                                ,drt.id                                 as ReportTableID
                                ,drt.alias                               as TableName
                                ,mdrc_ref.id                            as ColumnReferenceID
                            from       
                                {tableBase} mdrc
                            inner join
                                {tableMasterDynamicReportTable} mdrt on mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID = mdrt.ID
                            inner join
                                {tableDynamicReportTable} drt on drt.MASTER_DYNAMIC_REPORT_TABLE_ID = mdrt.ID
                            inner join
                                {tableDynamicReport} dr on dr.id = drt.DYNAMIC_REPORT_ID and dr.ID = {p}DynamicReportID
                            inner join
                                {tableInformationSchemaColumn} isc on isc.column_name = mdrc.name and isc.table_name = mdrt.name
                            inner join 
                                {tableMasterDynamicReportTable} mdrt_ref on mdrt_ref.name = isc.table_reference
                            inner join 
                              {tableBase} mdrc_ref on isc.column_reference = mdrc_ref.NAME and mdrt_ref.id = mdrc_ref.MASTER_DYNAMIC_REPORT_TABLE_ID 
                            where 
                                mdrt_ref.id = {p}MasterDynamicReportTableID
                            order by
                                    mdrc.order_key asc"
      ;

      var parameters = new Dictionary<string, object>()
      {
        ["MasterDynamicReportTableID"] = masterDynamicReportTableID,
        ["DynamicReportID"] = dynamicReportID
      };

      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowsForeignByTables
    public async Task<List<MasterDynamicReportColumn>> GetRowsForeignByReportTable(IDbTransaction transaction, string dynamicReportID, string masterDynamicReportTableID)
    {
      var p = db.Symbol();

      string query =
          $@"
                            select
                                mdrc.id                                 as ID
                                ,mdrc.master_dynamic_report_table_id    as MasterDynamicReportTableID
                                ,mdrc.name                              as Name
                                ,mdrc.alias                             as Alias
                                ,drt_ref.id                             as ReportTableReferenceID   
                                ,isc.table_reference                    as TableReference   
                                ,mdrc_ref.id                            as ColumnReferenceID
                                ,isc.column_reference                   as ColumnReference
                            from       
                                {tableBase} mdrc
                            inner join
                                {tableMasterDynamicReportTable} mdrt on mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID = mdrt.ID
                            inner join
                                {tableDynamicReportTable} drt on drt.MASTER_DYNAMIC_REPORT_TABLE_ID = mdrt.ID
                            inner join
                                {tableDynamicReport} dr on dr.id = drt.DYNAMIC_REPORT_ID and dr.ID = {p}DynamicReportID
                            inner join
                                {tableInformationSchemaColumn} isc on isc.column_name = mdrc.name and isc.table_name = mdrt.name
                            inner join 
                                {tableMasterDynamicReportTable} mdrt_ref on mdrt_ref.name = isc.table_reference
                            inner join
                                {tableDynamicReportTable} drt_ref on  drt_ref.MASTER_DYNAMIC_REPORT_TABLE_ID  = mdrt_ref.ID and drt_ref.DYNAMIC_REPORT_ID = dr.ID 
                            inner join 
                              {tableBase} mdrc_ref on isc.column_reference = mdrc_ref.NAME and mdrt_ref.id = mdrc_ref.MASTER_DYNAMIC_REPORT_TABLE_ID 
                            where 
                                mdrc.master_dynamic_report_table_id = {p}MasterDynamicReportTableID
                            order by
                                    mdrc.order_key asc"
      ;

      var parameters = new Dictionary<string, object>()
      {
        ["MasterDynamicReportTableID"] = masterDynamicReportTableID,
        ["DynamicReportID"] = dynamicReportID
      };

      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowsForLookup
    public async Task<List<MasterDynamicReportColumn>> GetRowsForLookup(IDbTransaction transaction, string? keyword, int offset, int limit, string masterDynamicReportTableID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                id                                 as ID
                                ,master_dynamic_report_table_id    as MasterDynamicReportTableID
                                ,alias                             as Alias
                                ,type                              as Type
                            from       
                                {tableBase}    
                            where 
                                master_dynamic_report_table_id = {p}MasterDynamicReportTableID
                            and 
                                is_available = 1
                            and
                                (    
                                    lower(alias)                like lower({p}Keyword)
                                )
                            order by
                                    order_key asc"
      );
      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit,
            MasterDynamicReportTableID = masterDynamicReportTableID
          }
      );
      return result;
    }
    #endregion

    #region GetRowsForLookupExcludeByDynamicReport
    public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupExcludeByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
          select
              mdrc.id                                 as ID
              ,mdrc.alias                             as Alias
              ,drt.alias                             as TableName  
							,drt.id                                as ReportTableID	
          from       
              {tableBase} mdrc
          inner join
            {tableMasterDynamicReportTable} mdrt
              on mdrt.id = mdrc.master_dynamic_report_table_id
          inner join 
            {tableDynamicReportTable} drt 
              on  drt.MASTER_DYNAMIC_REPORT_TABLE_ID  = mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID 
          inner join 
            {tableDynamicReport} dru 
              on  drt.dynamic_report_id = dru.ID  
                  and dru.ID = {p}DynamicReportID
          --left join 
          --  {tableDynamicReportColumn} druc 
          --    on  druc.MASTER_DYNAMIC_REPORT_COLUMN_ID = mdrc.ID 
          --        and dru.ID = drt.dynamic_report_id 
					--				and druc.dynamic_report_table_id = drt.id
          where 
         --    druc.id is null
         --and 
              mdrc.is_available = 1
          and
              (    
                  lower(mdrc.alias)                like lower({p}Keyword)
                  or lower(drt.alias)                like lower({p}Keyword)
              )
          order by
                  mdrt.alias asc, mdrc.order_key asc"
      );

      var parameters = new
      {
        Keyword = "%" + keyword + "%",
        Offset = offset,
        Limit = limit,
        DynamicReportID = dynamicReportID
      };
      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowsForLookupByDynamicReport
    public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
          select
              mdrc.id                                 as ID
              ,mdrc.alias                             as Alias
              ,drt.id                                as ReportTableID  
              ,drt.alias                             as TableName  
          from       
              {tableBase} mdrc
          inner join
            {tableMasterDynamicReportTable} mdrt
              on mdrt.id = mdrc.master_dynamic_report_table_id
          inner join 
            {tableDynamicReportTable} drt 
              on  drt.MASTER_DYNAMIC_REPORT_TABLE_ID  = mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID 
          inner join 
            {tableDynamicReport} dru 
              on  drt.dynamic_report_id = dru.ID  
									and dru.ID = {p}DynamicReportID

          where 
              mdrc.is_available = 1
          and
              (    
                  lower(mdrc.alias)                like lower({p}Keyword)
                  or lower(drt.alias)                like lower({p}Keyword)
              )
          order by
                  mdrt.alias asc, mdrc.order_key asc"
      );

      var parameters = new
      {
        Keyword = "%" + keyword + "%",
        Offset = offset,
        Limit = limit,
        DynamicReportID = dynamicReportID
      };
      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowsForLookupByDynamicReportTable
    public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTable(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
          select
              mdrc.id                                 as ID
              ,mdrc.alias                             as Alias
              ,mdrt.alias                             as TableName  
          from       
              {tableBase} mdrc
          inner join
            {tableMasterDynamicReportTable} mdrt
              on mdrt.id = mdrc.master_dynamic_report_table_id
          inner join 
            {tableDynamicReportTable} drt 
              on  drt.MASTER_DYNAMIC_REPORT_TABLE_ID  = mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID 
              and drt.id =  {p}DynamicReportTableID
					inner join 
  					{viewColumn} vic on lower(vic.COLUMN_NAME) = lower(mdrc.NAME) and lower(mdrt.NAME)  = lower(vic.TABLE_NAME) 
          where 
              (    
                  lower(mdrc.alias)                like lower({p}Keyword)
              )
          order by
                  vic.table_reference desc, mdrc.order_key asc"
      );

      var parameters = new
      {
        Keyword = "%" + keyword + "%",
        Offset = offset,
        Limit = limit,
        DynamicReportTableID = dynamicReportTableID
      };

      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRowsForLookupByDynamicReportTableForRelatedColumn
    public async Task<List<MasterDynamicReportColumn>> GetRowsForLookupByDynamicReportTableForRelatedColumn(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportTableID, string relatedMasterDynamicReportColumnID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
          select
              mdrc.id                                 as ID
              ,mdrc.alias                             as Alias
              ,mdrt.alias                             as TableName  
          from       
              {tableBase} mdrc
          inner join
            {tableMasterDynamicReportTable} mdrt
              on mdrt.id = mdrc.master_dynamic_report_table_id
          inner join 
            {tableDynamicReportTable} drt 
              on  drt.MASTER_DYNAMIC_REPORT_TABLE_ID  = mdrc.MASTER_DYNAMIC_REPORT_TABLE_ID 
              and drt.id =  {p}DynamicReportTableID
          inner join 
            {viewColumn} vic on lower(vic.COLUMN_NAME) = lower(mdrc.NAME) and lower(mdrt.NAME)  = lower(vic.TABLE_NAME) 
        inner join
        (
          select vic.data_type
									,vic.CHARACTER_MAXIMUM_LENGTH
									,vic.NUMERIC_SCALE
									,vic.NUMERIC_PRECISION
          FROM   {tableBase} mdrc
          inner join {tableMasterDynamicReportTable} mdrt on mdrt.id = mdrc.master_dynamic_report_table_id
          inner join 
            {viewColumn}   vic on lower(vic.COLUMN_NAME) = lower(mdrc.NAME) and lower(mdrt.NAME)  = lower(vic.TABLE_NAME) 
          where mdrc.ID = {p}MasterDynamicReportColumnID
        ) related on vic.data_type = related.data_type
                  and case when vic.character_maximum_length is null then 0 else vic.character_maximum_length end = case when related.character_maximum_length is null then 0 else related.character_maximum_length end
                  and case when vic.numeric_scale is null then 0 else vic.numeric_scale end = case when related.numeric_scale is null then 0 else related.numeric_scale end
                  and case when vic.numeric_precision is null then 0 else vic.numeric_precision end = case when related.numeric_precision is null then 0 else related.numeric_precision end
          where 
              (    
                  (mdrc.alias)                like ({p}Keyword)
                  or (mdrt.alias)                like ({p}Keyword)
              )
          order by
                  vic.table_reference desc, mdrc.order_key asc"
      );

      var parameters = new
      {
        Keyword = "%" + keyword + "%",
        Offset = offset,
        Limit = limit,
        DynamicReportTableID = dynamicReportTableID,
        MasterDynamicReportColumnID = relatedMasterDynamicReportColumnID
      };

      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          parameters
      );
      return result;
    }
    #endregion

    #region GetRows
    public async Task<List<MasterDynamicReportColumn>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                id                                 as ID
                                ,master_dynamic_report_table_id    as MasterDynamicReportTableID
                                ,name                              as Name
                                ,alias                             as Alias
                                ,type                              as Type
                                ,order_key                         as OrderKey
                                ,is_available                      as IsAvailable
                                ,is_masking                        as IsMasking
                            from       
                                {tableBase}    
                            where 
                                (    
                                    lower(name)                    like lower({p}Keyword)
                                    or lower(alias)                like lower({p}Keyword)
                                )
                            order by
                                    order_key asc"
      );
      var result = await _command.GetRows<MasterDynamicReportColumn>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit,
          }
      );
      return result;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, MasterDynamicReportColumn model)
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
                        ,master_dynamic_report_table_id             
                        ,name             
                        ,alias 
                        ,type     
                        ,order_key
                        ,is_available     
                        ,is_masking 
                    ) values (
                        {p}ID
                        ,{p}CreDate
                        ,{p}CreBy
                        ,{p}CreIPAddress
                        ,{p}ModDate
                        ,{p}ModBy
                        ,{p}ModIPAddress
                        --
                        ,{p}MasterDynamicReportTableID
                        ,{p}Name
                        ,{p}Alias
                        ,{p}Type
                        ,{p}OrderKey
                        ,{p}IsAvailable
                        ,{p}IsMasking
                    )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, MasterDynamicReportColumn model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        alias = {p}Alias
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

    #region ChangeAvailable
    public async Task<int> ChangeAvailable(IDbTransaction transaction, MasterDynamicReportColumn model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        is_available = is_available * -1
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

    #region ChangeMaskingStatus
    public async Task<int> ChangeMaskingStatus(IDbTransaction transaction, MasterDynamicReportColumn model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        is_masking = is_masking * -1
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
  }
}