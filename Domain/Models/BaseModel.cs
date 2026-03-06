namespace Domain.Models
{
  public class BaseModel
  {
    public string? ID { get; set; }
    public DateTime? CreDate { get; set; }
    public string? CreBy { get; set; }
    public string? CreIPAddress { get; set; }
    public DateTime? ModDate { get; set; }
    public string? ModBy { get; set; }
    public string? ModIPAddress { get; set; }
  }
}
