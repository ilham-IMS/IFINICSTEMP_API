namespace Domain.Models
{
  public class DynamicReportParameter : ExtendModel
  {
    public string? DynamicReportID { get; set; }
    public string? DynamicReportTableID { get; set; }
    public string? MasterDynamicReportColumnID { get; set; }
    public string? ComponentName { get; set; }
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? Operator { get; set; }
    public string? Formula { get; set; }
    public int? OrderKey { get; set; }
    public string? DefaultValue { get; set; }
    public int? IsDefaultValue { get; set; }

    #region MasterDynamicReportColumn
    public string? ColumnName { get; set; }

    #endregion

    #region DynamicReportTable
    public string? TableName { get; set; }

    #endregion
  }
}