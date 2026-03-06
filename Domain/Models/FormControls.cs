
namespace Domain.Models
{
   public class FormControls : BaseModel
   {
      public string? MasterFormID { get; set; }
      public string? ComponentName { get; set; }
      public string? Label { get; set; }
      public object? Value { get; set; }
      public int? Visible { get; set; }
      public int? Required { get; set; }
      public int? Disabled { get; set; }
      public object? AreaValue { get; set; }
      public string? Step { get; set; }
      public int? Min { get; set; }
      public int? Max { get; set; }
      public string? Style { get; set; }
      public int? IsActive { get; set; }
      public string? Name { get; set; }
      public int? DisplayOrder { get; set; }
      public string? NumericFormat { get; set; }
      public Dictionary<string, string>? Items { get; set; }

   }
}