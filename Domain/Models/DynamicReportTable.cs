namespace Domain.Models
{
  public class DynamicReportTable : ExtendModel
  {
    public string? DynamicReportID { get; set; }
    public string? MasterDynamicReportTableID { get; set; }
    public string? ReferenceDynamicReportTableID { get; set; }
    public string? TableName { get; set; }
    public string? Alias { get; set; }
    public string? ReferenceTableAlias { get; set; }
    public string? JoinClause { get; set; }
    public List<DynamicReportTableRelation>? RelatedTables { get; set; }
    public int? TotalRelation { get; set; }
    public int? IsTableReference { get; set; }
  }
}