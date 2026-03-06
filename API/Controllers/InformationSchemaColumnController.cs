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
  public class InformationSchemaColumnController : BaseController
  {
    private readonly IInformationSchemaColumnService _service;

    public InformationSchemaColumnController(
        IInformationSchemaColumnService service,
        IConfiguration configuration
    )
        : base(configuration)
    {
      _service = service;
    }

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string tableName)
    {
      try
      {
        var data = await _service.GetRows(tableName);
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
    public async Task<ActionResult> Insert([FromBody] InformationSchemaColumn model)
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
    public async Task<ActionResult> UpdateByID([FromBody] InformationSchemaColumn model)
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
