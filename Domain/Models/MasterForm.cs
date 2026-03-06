
namespace Domain.Models
{
   public class MasterForm : BaseModel
   {
      public string? Code { get; set; }
      public string? Name { get; set; }
      public string? Label { get; set; }
      public int? IsActive { get; set; }
   }
}