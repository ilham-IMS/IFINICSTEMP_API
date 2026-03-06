using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;

using Domain.Models;
using System.Text.Json.Nodes;


namespace DAL
{
  public class FlowchartNodeRepository : BaseRepository, IFlowchartNodeRepository
  {
    private string tableBase = "flowchart_node";


    public async Task<List<FlowchartNode>> GetRows(IDbTransaction transaction, string masterFlowchartID)
    {
      string p = db.Symbol();
      string query =
          $@"
                            select
                                 id                    as ID
                                ,node_name             as NodeName
                                ,method_name           as MethodName
                                ,source_link           as SourceLink
                                ,target_link           as TargetLink
                                ,x_coordinat           as XCoordinat
                                ,y_coordinat           as YCoordinat
                                ,short_description     as ShortDescription  
                            from       
                                {tableBase}
                            where
                                sys_menu_role_id = {p}SysMenuRoleID    
                            order by
                                    mod_date desc "
      ;
      var properties = new
      {
        SysMenuRoleID = masterFlowchartID
      };
      var result = await _command.GetRows<FlowchartNode>(
          transaction,
          query,
          properties
      );
      return result;

    }

    public async Task<List<FlowchartNode>> GetRowsByCode(IDbTransaction transaction, string code
    )
    {
      string p = db.Symbol();
      string query =
          $@"
                            select
                                 fn.id                 as ID
                                ,fn.node_name               as NodeName
                                ,fn.method_name        as MethodName
                                ,fn.source_link        as SourceLink
                                ,fn.target_link        as TargetLink
                                ,fn.x_coordinat        as XCoordinat
                                ,fn.y_coordinat        as YCoordinat
                                ,fn.short_description  as ShortDescription
                            from       
                                {tableBase} fn
                            inner join
                                 dynamic_button_process_role smr on smr.id = fn.sys_menu_role_id
                            where
                                smr.role_code = {p}Code    
                            order by
                                    fn.mod_date desc "
      ;
      var properties = new
      {
        Code = code
      };
      var result = await _command.GetRows<FlowchartNode>(
          transaction,
          query,
          properties
      );
      return result;

    }

    public async Task<FlowchartNode> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query =
          $@"
                        select
                                 id                    as ID
                                ,node_name                  as NodeName
                                ,method_name           as MethodName
                                ,source_link           as SourceLink
                                ,target_link           as TargetLink
                                ,x_coordinat           as XCoordinat
                                ,y_coordinat           as YCoordinat
                                ,short_description     as ShortDescription
																,sys_menu_role_id      as DynamicButtonProcessRoleID
                        from
                        {tableBase}
                    where
                        id = {p}ID";
      var result = await _command.GetRow<FlowchartNode>(
          transaction,
          query,
          new { ID = id }
      );
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, FlowchartNode module)
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
                                ,sys_menu_role_id
                                ,node_name
                                ,method_name  
                                ,source_link  
                                ,target_link  
                                ,x_coordinat  
                                ,y_coordinat      
                                ,short_description
                            ) values (
                                 {p}ID
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIPAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIPAddress
                                ,{p}DynamicButtonProcessRoleID
                                ,{p}NodeName
                                ,{p}MethodName
                                ,{p}SourceLink
                                ,{p}TargetLink
                                ,{p}XCoordinat
                                ,{p}YCoordinat
                                ,{p}ShortDescription
                            )";
      var result = await _command.Insert(transaction, query, module);
      return result;
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, FlowchartNode module)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableBase} 
                            set
                                 node_name                = {p}NodeName
                                ,source_link              = {p}SourceLink
                                ,target_link              = {p}TargetLink
                                ,x_coordinat              = {p}XCoordinat
                                ,y_coordinat              = {p}YCoordinat
                                ,short_description        = {p}ShortDescription
                                ,mod_date                 = {p}ModDate
                                ,mod_by                   = {p}ModBy
                                ,mod_ip_address           = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, module);
      return result;
    }

    public async Task<int> RemoveLinkedNode(IDbTransaction transaction, FlowchartNode model)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableBase} 
                            set
                                target_link              = null
                            where
                                id = (select id from {tableBase} where target_link = {p}NodeName and sys_menu_role_id = {p}DynamicButtonProcessRoleID)";
      var result = await _command.Update(transaction, query, model);
      return result;
    }

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

    public Task<List<FlowchartNode>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> IsStartNodeExist(IDbTransaction transaction, string SysMenuRoleID)
    {
      var p = db.Symbol();
      var query = $@"
        select 
          count(id)
        from
          {tableBase}
        where
          upper(node_name) = upper('START')
          and
          sys_menu_role_id = {p}SysMenuRoleID
      ";

      var parameters = new
      {
        SysMenuRoleID = SysMenuRoleID
      };

      var result = await _command.GetRow<int?>(transaction, query, parameters);
      return result > 0;
    }

    public async Task<List<JsonObject>> GetRowsByRoleCode(IDbTransaction transaction, string roleCode)
    {
      string p = db.Symbol();
      string query =
          $@"
                            select
                                 fn.id                    as ID
                                ,fn.node_name             as NodeName
                                ,fn.method_name           as MethodName
                                ,fn.source_link           as SourceLink
                                ,fn.target_link           as TargetLink
                                ,fn.x_coordinat           as XCoordinat
                                ,fn.y_coordinat           as YCoordinat
                                ,fn.short_description     as ShortDescription  
                            from       
                                {tableBase} fn
                            inner join 
                                dynamic_button_process_role smr on smr.id = fn.sys_menu_role_id
                            where
                                smr.role_code = {p}RoleCode    
                            order by
                                    fn.mod_date desc "
      ;
      var properties = new
      {
        RoleCode = roleCode
      };
      var result = await _command.GetRows<FlowchartNode>(
          transaction,
          query,
          properties
      );

      var jsonRows = result.Select(node =>
      {
        var jo = new JsonObject
        {
          ["DynamicButtonProcessRoleID"] = JsonValue.Create(node.DynamicButtonProcessRoleID),
          ["NodeName"] = JsonValue.Create(node.NodeName),
          ["MethodName"] = JsonValue.Create(node.MethodName),
          ["SourceLink"] = JsonValue.Create(node.SourceLink),
          ["TargetLink"] = JsonValue.Create(node.TargetLink),
          ["XCoordinat"] = JsonValue.Create(node.XCoordinat),
          ["YCoordinat"] = JsonValue.Create(node.YCoordinat),
          ["ShortDescription"] = JsonValue.Create(node.ShortDescription)
        };
        return jo;
      }).ToList();
      return jsonRows;

    }

    public async Task<List<FlowchartNode>> GetRowsByDynamicButtonProcessRoleID(IDbTransaction transaction, string dynamicButtonProcessRoleID)
    {
      string p = db.Symbol();
      string query =
                         $@"
                            select
                                 fn.id                 as ID
                                ,fn.node_name          as NodeName
                                ,fn.method_name        as MethodName
                                ,fn.source_link        as SourceLink
                                ,fn.target_link        as TargetLink
                                ,fn.x_coordinat        as XCoordinat
                                ,fn.y_coordinat        as YCoordinat
                                ,fn.short_description  as ShortDescription
                            from       
                                {tableBase} fn
                            inner join
                                 dynamic_button_process_role smr on smr.id = fn.sys_menu_role_id
                            where
                                fn.sys_menu_role_id = {p}MenuRoleID    
                            order by
                                    fn.cre_date asc ";

      var properties = new
      {
        MenuRoleID = dynamicButtonProcessRoleID
      };
      var result = await _command.GetRows<FlowchartNode>(
          transaction,
          query,
          properties
      );
      return result;

    }
    public async Task<string> ProcessB()
    {
      return "Process B";
    }


    public string ProcessC()
    {
      return "Process C";
    }

    public string Start()
    {
      return "Start";
    }

    public string End()
    {
      return "End";
    }

  }
}
