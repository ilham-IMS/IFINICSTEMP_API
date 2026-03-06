using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;


namespace DAL
{
  public class DynamicReportParameterRepository : BaseRepository, IDynamicReportParameterRepository
  {
    private string tableBase = "dynamic_report_parameter";
    private string tableDynamicReportTable = "dynamic_report_table";
    private string tableMasterDynamicReportColumn = "master_dynamic_report_column";
    private string tableMasterDynamicReportTable = "master_dynamic_report_table";

    #region GetRowsByDynamicReport
    public async Task<List<DynamicReportParameter>> GetRowsByDynamicReport(IDbTransaction transaction, string? keyword, int offset, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
              select
                  drp.id                                 as ID
                  ,drt.dynamic_report_id                 as DynamicReportID
                  ,drp.dynamic_report_table_id           as DynamicReportTableID
                  ,drp.master_dynamic_report_column_id   as MasterDynamicReportColumnID
                  ,drp.formula                           as Formula
                  ,drp.component_name                    as ComponentName
                  ,drp.label                             as Label
                  ,drp.name                              as Name
                  ,drp.operator                          as Operator
                  ,drp.order_key                         as OrderKey
                  ,mdrc.alias                             as ColumnName
                  ,drt.alias                             as TableName
                  ,drp.default_value                     as DefaultValue
                  ,drp.is_default_value                  as IsDefaultValue
              from       
                  {tableBase} drp 
              left join
                {tableMasterDynamicReportColumn} mdrc on mdrc.id = drp.master_dynamic_report_column_id
              left join
                {tableMasterDynamicReportTable} mdrt on mdrt.id = mdrc.master_dynamic_report_table_id
							left join
								{tableDynamicReportTable} drt on drt.id = drp.dynamic_report_table_id
              where 
                  drp.dynamic_report_id = {p}DynamicReportID
                  and
                    (    
                      lower(drp.component_name) like lower({p}Keyword)
                      or lower(drp.label) like lower({p}Keyword)
                      or lower(drp.name) like lower({p}Keyword)
                      or lower(mdrc.alias) like lower({p}Keyword)
                      or lower(drt.alias) like lower({p}Keyword)
                    )
              order by
                      drp.order_key asc"
      );

      var result = await _command.GetRows<DynamicReportParameter>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit,
            DynamicReportID = dynamicReportID
          }
      );
      return result;

    }
    public async Task<List<DynamicReportParameter>> GetRowsByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query =
          $@"
              select
                  drp.id                                 as ID
									,drp.dynamic_report_table_id           as DynamicReportTableID
                  ,drp.master_dynamic_report_column_id   as MasterDynamicReportColumnID
                  ,drp.operator                          as Operator
                  ,drp.name                              as Name
                  ,drp.label                              as Label
                  ,drp.formula                              as Formula
                  ,drp.default_value                     as DefaultValue
                  ,drp.is_default_value                  as IsDefaultValue
              from       
                  {tableBase} drp
             where dynamic_report_id = {p}DynamicReportID
              order by
                      drp.order_key asc"
      ;

      var result = await _command.GetRows<DynamicReportParameter>(
          transaction,
          query,
          new
          {

            DynamicReportID = dynamicReportID
          }
      );
      return result;

    }
    #endregion

    #region GetRowsComponentByDynamicReport
    public async Task<List<DynamicReportParameter>> GetRowsComponentByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query =
          $@"
              select
                  drp.id                                 as ID
                  ,drp.master_dynamic_report_column_id   as MasterDynamicReportColumnID
                  ,drp.component_name                    as ComponentName
                  ,drp.label                             as Label
                  ,drp.name                              as Name
                  ,drp.operator                          as Operator
                  ,mdrc.alias                             as ColumnName
                  ,drp.default_value                     as DefaultValue
                  ,drp.is_default_value                  as IsDefaultValue
              from       
                  {tableBase} drp 
              left join
                {tableMasterDynamicReportColumn} mdrc on mdrc.id = drp.master_dynamic_report_column_id
              where dynamic_report_id = {p}DynamicReportID 
              order by
                      drp.order_key asc"
      ;

      var result = await _command.GetRows<DynamicReportParameter>(
          transaction,
          query,
          new
          {

            DynamicReportID = dynamicReportID
          }
      );
      return result;

    }
    #endregion

    #region GetRows
    public async Task<List<DynamicReportParameter>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                drp.id                                 as ID
                                ,drp.master_dynamic_report_column_id   as MasterDynamicReportColumnID
                                ,drp.dynamic_report_table_id           as DynamicReportTableID
                                ,drp.component_name                    as ComponentName
                                ,drp.label                             as Label
                                ,drp.name                              as Name
                                ,drp.operator                          as Operator
                                ,drp.order_key                         as OrderKey
                                ,mdrc.alias                            as ColumnName
                                ,drt.alias                             as TableName
                                ,drp.default_value                     as DefaultValue
                                ,drp.is_default_value                  as IsDefaultValue
                            from       
                                {tableBase} drp 
                            inner join
                              {tableMasterDynamicReportColumn} mdrc on mdrc.id = drp.master_dynamic_report_column_id
                            inner join
                              {tableDynamicReportTable} drt on drt.id = drp.dynamic_report_table_id
                            where (    
                                    drp.component_name like {p}Keyword
                                    or drp.label like {p}Keyword
                                    or drp.name like {p}Keyword
                                    or mdrc.alias like {p}Keyword
                                    or drt.alias like {p}Keyword
                                  )
                            order by
                                    drp.order_key asc"
      );
      var result = await _command.GetRows<DynamicReportParameter>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit
          }
      );
      return result;

    }
    #endregion

    #region GetRowByID
    public async Task<DynamicReportParameter> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query =
          $@"
                    select
                        drp.id                                 as ID
                        ,drp.dynamic_report_table_id            as DynamicReportTableID
                        ,drp.master_dynamic_report_column_id   as MasterDynamicReportColumnID
                        ,drp.component_name                    as ComponentName
                        ,drp.label                             as Label
                        ,drp.name                              as Name
                        ,drp.order_key                         as OrderKey
                        ,drp.operator                          as Operator
                        ,mdrc.alias                             as ColumnName
												,drt.alias                             as TableName
                        ,drp.formula                           as Formula
                        ,drp.default_value                     as DefaultValue
                        ,drp.is_default_value                  as IsDefaultValue
                    from       
                        {tableBase} drp 
                    left join
                      {tableMasterDynamicReportColumn} mdrc on mdrc.id = drp.master_dynamic_report_column_id
										left join
										{tableDynamicReportTable} drt on drt.id = drp.dynamic_report_table_id
                    where
                        drp.id = {p}ID";
      var result = await _command.GetRow<DynamicReportParameter>(
          transaction,
          query,
          new { ID = id }
      );
      return result;
    }
    #endregion

    #region GetTopOrder
    public async Task<List<DynamicReportParameter>> GetTopOrderByDynamicReport(IDbTransaction transaction, int limit, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          drp.order_key as OrderKey
        from
          {tableBase} drp
        where
					dynamic_report_id = {p}DynamicReportID 
        order by
          drp.order_key desc ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Limit = limit,
        Offset = 0,
        DynamicReportID = dynamicReportID
      };
      return await _command.GetRows<DynamicReportParameter>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsOrder
    public async Task<List<DynamicReportParameter>> GetRowsOrderByDynamicReport(IDbTransaction transaction, string dynamicReportID)
    {
      var p = db.Symbol();
      string query = $@"
				select
					drp.id				as ID
					,drp.order_key as OrderKey
				from
					{tableBase} drp
				where
					dynamic_report_id = {p}DynamicReportID 
				order by
					drp.order_key asc
        ";

      var parameters = new
      {
        DynamicReportID = dynamicReportID
      };

      return await _command.GetRows<DynamicReportParameter>(transaction, query, parameters);
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, DynamicReportParameter module)
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
                                ,component_name
                                ,label      
                                ,name        
                                ,operator        
                                ,order_key     
                                ,formula
                                ,default_value
                                ,is_default_value
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
                                ,{p}ComponentName
                                ,{p}Label
                                ,{p}Name
                                ,{p}Operator
                                ,{p}OrderKey
                                ,{p}Formula
                                ,{p}DefaultValue
                                ,{p}IsDefaultValue
                            )";
      var result = await _command.Insert(transaction, query, module);
      return result;
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReportParameter module)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableBase} 
                            set
                                master_dynamic_report_column_id    = {p}MasterDynamicReportColumnID
                                ,dynamic_report_table_id           = {p}DynamicReportTableID
                                ,component_name                    = {p}ComponentName   
                                ,label                             = {p}Label   
                                ,name                              = {p}Name   
                                ,operator                          = {p}Operator
                                ,formula                           = {p}Formula
                                ,default_value                     = {p}DefaultValue
                                ,is_default_value                  = {p}IsDefaultValue  
                                --
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, module);
      return result;
    }
    #endregion

    #region ChangeOrder
    public async Task<int> ChangeOrder(IDbTransaction transaction, DynamicReportParameter model)
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

    #region DeleteByID
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
          $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      var result = await _command.DeleteByID(transaction, query, id);
      return result;
    }
    #endregion
  }
}
