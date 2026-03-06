using iFinancing360.API.Helper;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Abstract.Service;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [SetBaseModelProperties]
  [ApiController]
  [UserAuthorize]
  [DecryptQueryString]
  [DecryptRequestBody]
  public class DynamicButtonProcessRoleController : BaseController
  {
    private readonly IDynamicButtonProcessRoleService _service;

    public DynamicButtonProcessRoleController(IDynamicButtonProcessRoleService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
    }

    #region HttpGet
    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string dynamicButtonProcessID)
    {
      try
      {
        var data = await _service.GetRows(keyword, offset, limit, dynamicButtonProcessID);
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
    #endregion HttpGet
    #region HttpGetByRoleCode
    [HttpGet("GetRowByRoleCode")]
    public async Task<ActionResult> GetRowByRoleCode(string RoleCode)
    {
      try
      {
        var data = await _service.GetRowByRoleCode(RoleCode);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion 

    #region HttpPost
    [HttpPost("Insert")]
    public async Task<ActionResult> Insert(DynamicButtonProcessRole model)
    {
      try
      {
        var result = await _service.Insert(model);
        return ResponseSuccess(new { model.ID }, result);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion HttpPost

    #region HttpPut
    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID(DynamicButtonProcessRole model)
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
    #endregion HttpPut

    #region HttpDelete
    [HttpDelete("Delete")]
    public async Task<ActionResult> Delete([FromBody] string[] IDs)
    {
      try
      {
        var result = await _service.DeleteByID(IDs);
        return ResponseSuccess(new { }, result);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #endregion HttpDelete
    [HttpPost("SyncButtonProcessRole")]
    public async Task<ActionResult> SyncButtonProcessRole([FromBody] DynamicButtonProcessRole model)
    {
      try
      {
        var result = await _service.SyncButtonProcessRole(model);
        return ResponseSuccess(new { }, result);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
  }
}