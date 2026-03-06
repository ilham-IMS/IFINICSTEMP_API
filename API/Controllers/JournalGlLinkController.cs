
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
  public class JournalGlLinkController : BaseController
  {
    private readonly IJournalGlLinkService _service;

    public JournalGlLinkController(IJournalGlLinkService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
    }

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit)
    {
      try
      {
        var data = await _service.GetRows(keyword, offset, limit);
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
    public async Task<ActionResult> Insert(JournalGlLink module)
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

    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID(JournalGlLink module)
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

    [HttpPut("ChangeIsActive")]
    public async Task<ActionResult> ChangeIsActive(JournalGlLink module)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.ChangeIsActive(module));
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    [HttpGet("GetReportData")]
    public async Task<IActionResult> GetReportData()
    {
      try
      {
        var report = await _service.GetReportData();
        return ResponseSuccess(report);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }


  }
}