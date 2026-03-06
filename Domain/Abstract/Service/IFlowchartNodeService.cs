using Domain.Models;

namespace Domain.Abstract.Service
{
	public interface IFlowchartNodeService : IBaseService<FlowchartNode>
	{
		Task<List<FlowchartNode>> GetRows(string DynamicButtonProcessRoleID);
		Task<int> Update(List<FlowchartNode> models);
		Task<List<FlowchartNode>> GetRowsByCode(string roleCode);
		Task<int> DeleteByID(List<FlowchartNode> models);

	}
}