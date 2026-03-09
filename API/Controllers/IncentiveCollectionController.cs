using iFinancing360.API.Helper;
using Domain.Abstract.Service;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [UserAuthorize] // Tambahkan Authorize disini
  [ApiController]
  [SetBaseModelProperties]
  [DecryptQueryString]
	[DecryptRequestBody]
  public class IncentiveCollectionController : BaseController
  {
    private readonly IIncentiveCollectionService _service;
    private readonly InternalAPIClient _internalAPIClient;

    public IncentiveCollectionController(IIncentiveCollectionService service, IConfiguration configuration, InternalAPIClient internalAPIClient) : base(configuration)
    {
      _service = service;
      _internalAPIClient = internalAPIClient;
    }

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string? PeriodeFrom, string? PeriodeTo)
    {
      try
      {
        var data = await _service.GetRows(keyword, offset, limit, PeriodeFrom, PeriodeTo);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpGet("GetRowByID")]
    public async Task<ActionResult> GetRowByID(string ID)
    {
      try
      {
        var data = await _service.GetRowByID(ID);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPost("Insert")]
    public async Task<ActionResult> Insert(IncentiveCollection module)
    {
      try
      {
        return ResponseSuccess(new { module.ID }, await _service.Insert(module));
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID(IncentiveCollection module)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.UpdateByID(module));
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpDelete("DeleteByID")]
    public async Task<ActionResult> DeleteByID([FromBody] string[] id)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.DeleteByID(id));
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    #region GetHTMLPreview
    [HttpGet("GetHTMLPreview")]
    public async Task<ActionResult> GetHTMLPreview(string id, string? PeriodeFrom, string? PeriodeTo)
    {
      try
      {
        var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

        var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
        var sysCompany = resSysCompany?.Data ?? [];

        var incentiveCollectionData = await _service.GetRowByID(id);

        incentiveCollectionData.PeriodeFrom = PeriodeFrom;
        incentiveCollectionData.PeriodeTo = PeriodeTo;
        incentiveCollectionData.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
        incentiveCollectionData.CompanyName = sysCompany?["Name"]?.GetValue<string>();

        var result = await _service.GetHTMLPreview(id, incentiveCollectionData);
        return ResponseSuccess(new { HTML = result });

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion
    #region PrintDocument
    [HttpGet("PrintDocument")]
    public async Task<ActionResult> PrintDocument(string mimeType, string id, string? PeriodeFrom, string? PeriodeTo)
    {
      try
      {
        var incentiveCollectionData = await _service.GetRowByID(id);

        var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

        var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
        var sysCompany = resSysCompany?.Data ?? [];

        incentiveCollectionData.PeriodeFrom = PeriodeFrom;
        incentiveCollectionData.PeriodeTo = PeriodeTo;
        incentiveCollectionData.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
        incentiveCollectionData.CompanyName = sysCompany?["Name"]?.GetValue<string>();

        var content = await _service.PrintDocument(mimeType, id, incentiveCollectionData);
        return ResponseSuccess(content);

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion
    
  }
}