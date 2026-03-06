namespace Domain.Models
{
	public class SysJobTaskLog : BaseModel
	{
		public string? SysJobTaskID { get; set; }
		public string? ErrorMessage { get; set; }
		public string? StackTrace { get; set; }
	}
}