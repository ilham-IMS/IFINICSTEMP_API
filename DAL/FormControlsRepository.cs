using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;


namespace DAL
{
  public class FormControlsRepository : BaseRepository, IFormControlsRepository
  {
    private string tableName = "form_controls";

    public async Task<List<FormControls>> GetRows(
        IDbTransaction transaction,
        string masterFormID
    )
    {
      var p = db.Symbol();
      string query =
          $@"
                            select
                                 id              as ID
                                ,master_form_id  as MasterFormID
                                ,component_name  as ComponentName
                                ,label           as Label
                                ,value           as Value
                                ,visible         as Visible
                                ,required        as Required
                                ,disabled        as Disabled
                                ,area_value      as AreaValue
                                ,step            as Step
                                ,min             as Min
                                ,max             as Max
                                ,style           as Style
                                ,is_active       as IsActives
                                ,name            as Name
                                ,numeric_format  as NumericFormat
                                ,display_order   as DisplayOrder
                            from       
                                {tableName}    
                            where
                                master_form_id = {p}MasterFormID
                            and
                                is_active = 1
                            order by
                                    cre_date asc"
      ;
      var result = await _command.GetRows<FormControls>(
          transaction,
          query,
          new
          {
            MasterFormID = masterFormID
          }
      );
      return result;

    }

    public async Task<List<FormControls>> GetRowsForDataTable(
        IDbTransaction transaction,
        string masterFormID
    )
    {
      var p = db.Symbol();
      string query =
          $@"
                            select
                                 id              as ID
                                ,master_form_id  as MasterFormID
                                ,component_name  as ComponentName
                                ,label           as Label
                                ,value           as Value
                                ,visible         as Visible
                                ,required        as Required
                                ,disabled        as Disabled
                                ,area_value      as AreaValue
                                ,step            as Step
                                ,min             as Min
                                ,max             as Max
                                ,style           as Style
                                ,is_active       as IsActive
                                ,name            as Name
                                ,numeric_format  as NumericFormat
                                ,display_order   as DisplayOrder
                            from       
                                {tableName}    
                            where
                                master_form_id = {p}MasterFormID
                            order by
                                    cre_date desc"
      ;
      var result = await _command.GetRows<FormControls>(
          transaction,
          query,
          new
          {
            MasterFormID = masterFormID
          }
      );
      return result;

    }

    public async Task<FormControls> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query =
          $@"
                        select
                                 id              as ID
                                ,master_form_id as MasterFormID
                                ,component_name as ComponentName
                                ,name           as Name
                                ,label          as Label
                                ,value          as Value
                                ,visible        as Visible
                                ,required       as Required
                                ,disabled       as Disabled
                                ,area_value     as AreaValue
                                ,step           as Step
                                ,min            as Min
                                ,max            as Max
                                ,style          as Style
                                ,display_order  as DisplayOrder
                                ,disabled       as Disabled
                                ,is_active      as IsActive
                                ,numeric_format  as NumericFormat
                        from
                        {tableName}
                    where
                        id = {p}ID";
      var result = await _command.GetRow<FormControls>(
          transaction,
          query,
          new { ID = id }
      );
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, FormControls module)
    {
      var p = db.Symbol();
      string query =
          $@"
                            insert into {tableName} (
                                 id
                                ,cre_date
                                ,cre_by
                                ,cre_ip_address
                                ,mod_date
                                ,mod_by
                                ,mod_ip_address
                                ,master_form_id
                                ,component_name
                                ,label
                                ,value
                                ,visible      
                                ,required             
                                ,disabled
                                ,area_value
                                ,step             
                                ,min             
                                ,max             
                                ,style
                                ,name
                                ,display_order             
                                ,is_active     
                                ,numeric_format
                            ) values (
                                 {p}ID
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIPAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIPAddress
                                ,{p}MasterFormID
                                ,{p}ComponentName
                                ,{p}Label
                                ,{p}Value
                                ,{p}Visible
                                ,{p}Required
                                ,{p}Disabled
                                ,{p}AreaValue
                                ,{p}Step
                                ,{p}Min
                                ,{p}Max
                                ,{p}Style
                                ,{p}Name
                                ,{p}DisplayOrder
                                ,{p}IsActive
                                ,{p}NumericFormat   
                            )";
      var result = await _command.Insert(transaction, query, module);
      return result;
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, FormControls module)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableName} 
                            set
                                 label              = {p}Label
                                ,name               = {p}Name
                                ,component_name     = {p}ComponentName
                                ,min                = {p}Min
                                ,max                = {p}Max
                                ,display_order      = {p}DisplayOrder
                                ,visible            = {p}Visible
                                ,disabled           = {p}Disabled
                                ,required           = {p}Required
                                ,is_active          = {p}IsActive
                                ,numeric_format     = {p}NumericFormat   
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, module);
      return result;
    }

    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
          $@"
                            delete from {tableName}
                            where
                                id = {p}ID";
      var result = await _command.DeleteByID(transaction, query, id);
      return result;
    }

    public async Task<int> ChangeStatus(IDbTransaction transaction, FormControls model)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableName} 
                            set 
                                is_active           = is_active * -1
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, model);
      return result;
    }

    public async Task LockRow(IDbTransaction transaction, string tableName, string ID)
    {
      await _command.LockRow(transaction, tableName, ID);
    }

    public Task<List<FormControls>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }

    public async Task<int> ChangeStatusNonActive(IDbTransaction transaction, FormControls model)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableName} 
                            set 
                                is_active           =  -1
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                master_form_id= {p}MasterFormID";
      var result = await _command.Update(transaction, query, model);
      return result;
    }
  }
}
