namespace Domain.Models
{
  public class DynamicReportColumnOrder : BaseModel
  {
    public string? DynamicReportColumnID { get; set; }
    public string? OrderBy { get; set; }
    public int? OrderKey { get; set; }

    #region DynamicReportColumn
    public string? ColumnTitle { get; set; }

		#region DynamicReportTable
    public string? TableAlias { get; set; }
		#endregion
    #endregion
  }
}