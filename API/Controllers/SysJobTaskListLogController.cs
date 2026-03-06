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
  public class SysJobTaskListLogController : BaseController
  {
    private readonly ISysJobTaskListLogService _service;

    public SysJobTaskListLogController(ISysJobTaskListLogService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
    }

    [HttpPost("Insert")]
    public async Task<ActionResult> Insert(SysJobTaskListLog module)
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
  }
}