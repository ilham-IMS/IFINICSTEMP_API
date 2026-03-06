using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
	public class MasterDynamicButtonProcess : ExtendModel
	{
		public string? DllName { get; set; }
		public string? NamespaceName { get; set; }
		public string? ClassName { get; set; }
		public string? MethodName { get; set; }
		public string? ShortDescription { get; set; }
		public string? Description { get; set; }
		public int? IsActive { get; set; }
		public string? MethodFullName { get; set; }
	}
}