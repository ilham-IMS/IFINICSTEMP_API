namespace Domain.Models
{
  public class DynamicButtonProcess : BaseModel
  {
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ModuleID { get; set; }
    public string? ParentMenuID { get; set; }
    public string? URLMenu { get; set; }
    public int OrderKey { get; set; }
    public string? CssIcon { get; set; } = "";
    public int? IsActive { get; set; }
    public string? Type { get; set; }
    public string? IconColor { get; set; }

    #region SysModule
    public string? ModuleCode { get; set; }
    public string? ModuleName { get; set; }
    #endregion

    #region MasterDynamicButtonProcess (Parent Menu)
    public string? ParentMenuName { get; set; }
    public int? IsDynamic  { get; set; }
    #endregion
  }
}
