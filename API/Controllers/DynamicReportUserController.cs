using Microsoft.AspNetCore.Mvc;
using iFinancing360.API.Helper;
using Domain.Abstract.Service;

using Domain.Models;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [ApiController]
  [UserAuthorize] // Tambahkan Authorize disini
  [SetBaseModelProperties]
  [DecryptQueryString]
	[DecryptRequestBody]
  public class DynamicReportUserController : BaseController
  {
    private readonly IDynamicReportUserService _service;

    public DynamicReportUserController(
        IDynamicReportUserService service,
        IConfiguration configuration
    )
        : base(configuration)
    {
      _service = service;
    }

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string dynamicReportID)
    {
      try
      {
        var data = await _service.GetRowsByDynamicReport(keyword, offset, limit, dynamicReportID);
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
    public async Task<ActionResult> Insert([FromBody] List<DynamicReportUser> models)
    {
      try
      {
        var data = await _service.Insert(models);
        return ResponseSuccess(null, data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID([FromBody] DynamicReportUser model)
    {
      try
      {
        var data = await _service.UpdateByID(model);
        return ResponseSuccess(new { }, data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpDelete("DeleteByID")]
    public async Task<ActionResult> DeleteByID([FromBody] string[] ID)
    {
      try
      {
        var data = await _service.DeleteByID(ID);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
  }
}
