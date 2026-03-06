
using System.Data;
using System.Text.Json.Nodes;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IFlowchartNodeRepository : IBaseRepository<FlowchartNode>
  {
    Task<List<FlowchartNode>> GetRows(IDbTransaction transaction, string DynamicButtonProcessRoleID);
    Task<List<FlowchartNode>> GetRowsByCode(IDbTransaction transaction, string code);
    Task<int> RemoveLinkedNode(IDbTransaction transaction, FlowchartNode model);
    Task<bool> IsStartNodeExist(IDbTransaction transaction, string DynamicButtonProcessRoleID);
    Task<List<JsonObject>> GetRowsByRoleCode(IDbTransaction transaction, string roleCode);
    Task<List<FlowchartNode>> GetRowsByDynamicButtonProcessRoleID(IDbTransaction transaction, string dynamicButtonProcessRoleID);

  }

}