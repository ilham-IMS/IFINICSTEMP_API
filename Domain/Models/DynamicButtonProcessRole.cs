
namespace Domain.Models
{
	public class DynamicButtonProcessRole : ExtendModel
	{
		public string? Code { get; set; }
		public string? DynamicButtonProcessID { get; set; }
		public string? Name { get; set; }
		public string? RoleAccess { get; set; }
		public int? IsDynamic { get; set; }
	}
}