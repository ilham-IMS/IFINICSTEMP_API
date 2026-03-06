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
  public class MasterDynamicReportTableController : BaseController
  {
    private readonly IMasterDynamicReportTableService _service;

    public MasterDynamicReportTableController(
        IMasterDynamicReportTableService service,
        IConfiguration configuration
    )
        : base(configuration)
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
    [HttpGet("GetRowsForLookup")]
    public async Task<ActionResult> GetRowsForLookup(string? keyword, int offset, int limit)
    {
      try
      {
        var data = await _service.GetRowsForLookup(keyword, offset, limit);
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
    public async Task<ActionResult> Insert([FromBody] List<MasterDynamicReportTable> model)
    {
      try
      {
        var data = await _service.Insert(model);
        return ResponseSuccess(new { }, data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID([FromBody] MasterDynamicReportTable model)
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

    #region GetRowByExt
        [HttpGet("GetRowByExt")]
        public async Task<ActionResult> GetRowByExt(string ID)
        {
            try
            {
                var data = await _service.GetRowForParent(ID);
                return ResponseSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseError(ex);
            }
        }
        #endregion


  }
}
