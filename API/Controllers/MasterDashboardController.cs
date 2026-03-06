using Microsoft.AspNetCore.Mvc;
using iFinancing360.API.Helper;
using Domain.Abstract.Service;
using Domain.Models;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [UserAuthorize] // Tambahkan Authorize disini
  [ApiController]
  [SetBaseModelProperties]
  [DecryptQueryString]
	[DecryptRequestBody]
  public class MasterDashboardController : BaseController
  {
    private readonly IMasterDashboardService _service;
    public MasterDashboardController(IMasterDashboardService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
    }

    #region GetRows

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit)
    {
      try
      {
        List<MasterDashboard> result = await _service.GetRows(keyword, offset, limit);
        return ResponseSuccess(result);

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

    #region GetRowsForLookupExcludeByID

    [HttpGet("GetRowsForLookupExcludeByID")]
    public async Task<ActionResult> GetRowsForLookupExcludeByID(string? keyword, int offset, int limit, [FromQuery] string[] id)
    {
      try
      {
        List<MasterDashboard> result = await _service.GetRowsForLookupExcludeByID(keyword, offset, limit, id);
        return ResponseSuccess(result);

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

    #region GetRowByID
    [HttpGet("GetRowByID")]
    public async Task<ActionResult> GetRowByID(string id)
    {
      try
      {
        MasterDashboard result = await _service.GetRowByID(id);
        return ResponseSuccess(result);

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

    #region Insert
    [HttpPost("Insert")]
    public async Task<ActionResult> Insert(MasterDashboard model)
    {
      try
      {
        return ResponseSuccess(new { model.ID }, await _service.Insert(model));

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    #endregion

    #region UpdateByID
    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID(MasterDashboard model)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.UpdateByID(model));

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

    #region DeleteByID
    [HttpDelete("DeleteByID")]
    public async Task<ActionResult> DeleteByID([FromBody] string[] ids)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.DeleteByID(ids));

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    #endregion
    // #region ChangeStatus
    // [HttpPut("ChangeStatus")]
    // public async Task<ActionResult> ChangeStatus(MasterDashboard model)
    // {
    //   try
    //   {
    //     return ResponseSuccess(new { }, await _service.ChangeStatus(model));

    //   }
    //   catch (Exception ex)
    //   {
    //     return ResponseError(ex);
    //   }
    // }
    // #endregion
    #region ChangeEditableStatus
    [HttpPut("ChangeEditableStatus")]
    public async Task<ActionResult> ChangeEditableStatus(MasterDashboard model)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.ChangeEditableStatus(model));

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

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