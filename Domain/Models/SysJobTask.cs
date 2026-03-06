namespace Domain.Models
{
	public class SysJobTask : BaseModel
	{
		public string? Code { get; set; }
		public string? JobStatus { get; set; }
		public int? RowToProcess { get; set; }
	}
}