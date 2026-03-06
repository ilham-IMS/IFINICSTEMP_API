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
  public class IncentiveGroupCriteriaController : BaseController
  {
    private readonly IIncentiveGroupCriteriaService _service;

    public IncentiveGroupCriteriaController(IIncentiveGroupCriteriaService service, IConfiguration configuration) : base(configuration)
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

    [HttpGet("GetRowsByGroupID")]
    public async Task<ActionResult> GetRowsByGroupID(string? keyword, int offset, int limit, string groupID)
    {
      try
      {
        var data = await _service.GetRowsByGroupID(keyword, offset, limit, groupID);
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
    public async Task<ActionResult> Insert(IncentiveGroupCriteria module)
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
    public async Task<ActionResult> UpdateByID(List<IncentiveGroupCriteria> module)
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
  }
}