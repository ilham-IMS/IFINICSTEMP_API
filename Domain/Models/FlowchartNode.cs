
namespace Domain.Models
{
  public class FlowchartNode : BaseModel
  {
    public string? DynamicButtonProcessRoleID { get; set; }
    public string? NodeName { get; set; }
    public string? MethodName { get; set; }
    public string? SourceLink { get; set; }
    public string? TargetLink { get; set; }
    public double? XCoordinat { get; set; }
    public double? YCoordinat { get; set; }
    public string? ShortDescription { get; set; }
  }
}