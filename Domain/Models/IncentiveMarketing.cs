using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class IncentiveMarketing : BaseModel
  {  
    public string? ClientID { get; set; }
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public string? IncentivePeriode { get; set; }
    public decimal? TotalIncentiveAmount { get; set; }

    //Print
    //Upload
      public string? MimeType { get; set; }
      public IFormFile? File { get; set; }
  }
}