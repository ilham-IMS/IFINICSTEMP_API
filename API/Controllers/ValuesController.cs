using iFinancing360.API.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("/api/[controller]")]
	[UserAuthorize] // Tambahkan Authorize disini
	[ApiController]
	[DecryptQueryString]
	[DecryptRequestBody]
	public class ValuesController : BaseController
	{
		public readonly IConfiguration _configuration;

		public ValuesController(IConfiguration configuration) : base(configuration)
		{
			_configuration = configuration;
		}

		[HttpGet("Values")]
		public ActionResult GetRows()
		{
			return Ok(new { ID = Guid.NewGuid().ToString().Replace("-", "").ToLower(), Value = "FS00001"[^5..] });
		}
	}
}