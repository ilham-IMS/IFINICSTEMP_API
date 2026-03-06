namespace Domain.Models
{
  public class DynamicReportTableRelation : BaseModel
  {
    public string? DynamicReportTableID { get; set; }
    public string? ReferenceDynamicReportTableID { get; set; }
    public string? SourceMasterDynamicReportColumnValue { get; set; }
    public string? ReferenceMasterDynamicReportColumnValue { get; set; }
    public string? SourceMasterDynamicReportColumnID { get; set; }
    public string? ReferenceMasterDynamicReportColumnID { get; set; }
    public string? Operator { get; set; }
    public string? ReferenceTableAlias { get; set; }
    public string? ColumnName { get; set; }
    public string? ReferenceColumnName { get; set; }
  }
}