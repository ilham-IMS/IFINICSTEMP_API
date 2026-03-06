namespace Domain.Models
{
  public class MasterDynamicReportColumn : BaseModel
  {
    public string? MasterDynamicReportTableID { get; set; }
    public string? Name { get; set; }
    public string? Alias { get; set; }
    public string? Type { get; set; }
    public int? OrderKey { get; set; }
    public int? IsAvailable { get; set; }
    public int? IsMasking { get; set; }

    public string? ReportTableID { get; set; }
    public string? TableName { get; set; }
    public string? ReportTableReferenceID { get; set; }
    public string? TableReferenceID { get; set; }
    public string? TableReference { get; set; }
    public string? ColumnReferenceID { get; set; }
    public string? ColumnReference { get; set; }
  }
}