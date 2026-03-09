using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
  public class IncentiveCollection : BaseModel
  {  
    public string? ClientID { get; set; }
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public string? IncentivePeriode { get; set; }
    public decimal? TotalIncentiveAmount { get; set; }
    // Periode
    public string? PeriodeFrom { get; set; }
    public string? PeriodeTo { get; set; }
    //Print
    public string? MimeType { get; set; }
    public IFormFile? File { get; set; }

    //Company Info
    public string? CompanyName { get; set; }
    public string? CompanyFileName { get; set; }
  }
}