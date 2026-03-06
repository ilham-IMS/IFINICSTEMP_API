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
  public class DynamicReportParameterController : BaseController
  {
    private readonly IDynamicReportParameterService _service;

    public DynamicReportParameterController(
        IDynamicReportParameterService service,
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
    [HttpGet("GetRowsComponentByDynamicReport")]
    public async Task<ActionResult> GetRowsComponentByDynamicReport(string dynamicReportID)
    {
      try
      {
        var data = await _service.GetRowsComponentByDynamicReport(dynamicReportID);
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
    public async Task<ActionResult> Insert([FromBody] DynamicReportParameter model)
    {
      try
      {
        var data = await _service.Insert(model);
        return ResponseSuccess(new { ID = model.ID }, data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID([FromBody] DynamicReportParameter model)
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
    [HttpPut("OrderUp")]
    public async Task<ActionResult> OrderUp([FromBody] DynamicReportParameter model)
    {
      try
      {
        var data = await _service.OrderUp(model);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    [HttpPut("OrderDown")]
    public async Task<ActionResult> OrderDown([FromBody] DynamicReportParameter model)
    {
      try
      {
        var data = await _service.OrderDown(model);
        return ResponseSuccess(data);
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
