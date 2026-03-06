namespace Domain.Models
{
  public class DynamicReportColumn : BaseModel
  {
    public string? DynamicReportID { get; set; }
    public string? DynamicReportTableID { get; set; }
    public string? MasterDynamicReportColumnID { get; set; }
    public string? HeaderTitle { get; set; }
    public int? OrderKey { get; set; }
    public string? GroupFunction { get; set; }
    public string? OrderBy { get; set; }
    public string? Formula { get; set; }

    #region Dynamic Report User Table
    public string? TableAlias { get; set; } // Nama Alias dari User
    #endregion
    #region Master Dynamic Report Table
    public string? TableName { get; set; } // Nama Ori Table
    #endregion

    #region Master Dynamic Report Column
    public string? ColumnName { get; set; } // Nama Ori Column
    public int? IsMasking { get; set; } // Flag untuk maskin
    #endregion
  }
}