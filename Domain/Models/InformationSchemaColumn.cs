namespace Domain.Models
{
  public class InformationSchemaColumn : BaseModel
  {
    public string? TableName { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public int? OrderKey { get; set; }
    public string? IsNullable { get; set; }
  }
}