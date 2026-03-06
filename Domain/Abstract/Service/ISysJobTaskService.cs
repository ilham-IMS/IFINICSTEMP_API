using Domain.Models;

namespace Domain.Abstract.Service
{
	public interface ISysJobTaskService : IBaseService<SysJobTask>
	{
		Task<SysJobTask> GetRowByCode(string code);
		Task<int> UpdateJobStatus(SysJobTask model);

	}
}