using Microsoft.AspNetCore.Mvc;
using iFinancing360.API.Helper;
using Domain.Abstract.Service;

using Domain.Models;
using System.Text.RegularExpressions;
using MoreLinq;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [ApiController]
  [UserAuthorize] // Tambahkan Authorize disini
  [SetBaseModelProperties]
  [DecryptQueryString]
	[DecryptRequestBody]
  public class DynamicReportController : BaseController
  {
    private readonly IDynamicReportService _service;

    public DynamicReportController(
        IDynamicReportService service,
        IConfiguration configuration
    )
        : base(configuration)
    {
      _service = service;
    }


    #region GetRows
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
    #endregion

    #region GetRowsPublished
    [HttpGet("GetRowsPublishedByUser")]
    public async Task<ActionResult> GetRowsPublishedByUser(string? keyword, int offset, int limit, string userCode)
    {
      try
      {
        var data = await _service.GetRowsPublishedByUser(keyword, offset, limit, userCode);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

    #region GetRowByID
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
    #endregion

    #region GetQuery
    [HttpGet("GetQuery")]
    public async Task<ActionResult> GetQuery(string id, bool asString = false)
    {
      try
      {
        var data = await _service.GetQuery(id);
        if (asString) return Ok(data);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion

    [HttpPost("Insert")]
    public async Task<ActionResult> Insert([FromBody] DynamicReport model)
    {
      try
      {
        var data = await _service.Insert(model);
        return ResponseSuccess(new { model.ID }, data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID([FromBody] DynamicReport model)
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

    [HttpPost("Print")]
    public async Task<ActionResult> Print([FromBody] DynamicReport model)
    {
      try
      {
        var data = await _service.Print(model);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    [HttpPost("ChangePublishStatus")]
    public async Task<ActionResult> ChangePublishStatus([FromBody] DynamicReport model)
    {
      try
      {
        var data = await _service.ChangePublishStatus(model);
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
