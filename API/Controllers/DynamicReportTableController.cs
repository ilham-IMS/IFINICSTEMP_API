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
  public class DynamicReportTableController : BaseController
  {
    private readonly IDynamicReportTableService _service;

    public DynamicReportTableController(
        IDynamicReportTableService service,
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
        var data = await _service.GetRows(keyword, offset, limit, dynamicReportID);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpGet("GetRowsExclude")]
    public async Task<ActionResult> GetRowsExclude(string? keyword, int offset, int limit, string dynamicReportTableID)
    {
      try
      {
        var data = await _service.GetRowsExclude(keyword, offset, limit, dynamicReportTableID);
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
    public async Task<ActionResult> Insert([FromBody] DynamicReportTable model)
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
    public async Task<ActionResult> UpdateByID([FromBody] DynamicReportTable models)
    {
      try
      {
        var data = await _service.UpdateByID(models);
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
