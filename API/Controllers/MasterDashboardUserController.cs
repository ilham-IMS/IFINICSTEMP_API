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
  public class MasterDashboardUserController : BaseController
  {
    private readonly IMasterDashboardUserService _service;
    public MasterDashboardUserController(IMasterDashboardUserService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
    }

    #region GetRows

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string userID)
    {
      try
      {
        List<MasterDashboardUser> result = await _service.GetRows(keyword, offset, limit, userID);
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
        MasterDashboardUser result = await _service.GetRowByID(id);
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
    public async Task<ActionResult> Insert(List<MasterDashboardUser> models)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.Insert(models));

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    #endregion

    #region UpdateByID
    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID([FromBody] List<MasterDashboardUser> models)
    {
      try
      {
        return ResponseSuccess(new { }, await _service.UpdateByID(models));

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
  }
}