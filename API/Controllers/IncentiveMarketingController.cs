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
  public class IncentiveMarketingController : BaseController
  {
    private readonly IIncentiveMarketingService _service;

    public IncentiveMarketingController(IIncentiveMarketingService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
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
    public async Task<ActionResult> Insert(IncentiveMarketing module)
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
    public async Task<ActionResult> UpdateByID(IncentiveMarketing module)
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
    public async Task<ActionResult> GetHTMLPreview(string id)
    {
      try
      {

        var incentiveMarketingData = await _service.GetRowByID(id);

        var result = await _service.GetHTMLPreview(id, incentiveMarketingData);
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
    public async Task<ActionResult> PrintDocument(string mimeType, string id)
    {
      try
      {
        var incentiveMarketingData = await _service.GetRowByID(id);

        var content = await _service.PrintDocument(mimeType, id, incentiveMarketingData);
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