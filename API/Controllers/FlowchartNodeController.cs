using Microsoft.AspNetCore.Mvc;
using iFinancing360.API.Helper;
using Domain.Abstract.Service;

using Domain.Models;

namespace API.Controllers
{
	[Route("/api/[controller]")]
	//[UserAuthorize] // Tambahkan Authorize disini
	[ApiController]
	[SetBaseModelProperties]

	[UserAuthorize]
	[DecryptQueryString]
	[DecryptRequestBody]
	public class FlowchartNodeController : BaseController
	{
		private readonly IFlowchartNodeService _service;
		private readonly IConfiguration _configuration;

		public FlowchartNodeController(
				IFlowchartNodeService service,
				IConfiguration configuration
		)
				: base(configuration)
		{
			_service = service;
			_configuration = configuration;
		}

		[HttpGet("GetRows")]
		public async Task<ActionResult> GetRows(string DynamicButtonProcessRoleID)
		{
			try
			{
				var data = await _service.GetRows(DynamicButtonProcessRoleID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}

		[HttpGet("GetRowsByCode")]
		public async Task<ActionResult> GetRowsByCode(string roleCode)
		{
			try
			{
				var data = await _service.GetRowsByCode(roleCode);
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
		public async Task<ActionResult> Insert([FromBody] FlowchartNode model)
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

		[HttpPut("Update")]
		public async Task<ActionResult> UpdateByID([FromBody] List<FlowchartNode> models)
		{
			try
			{
				var data = await _service.Update(models);
				return ResponseSuccess(new { }, data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}

		[HttpDelete("DeleteByID")]
		public async Task<ActionResult> DeleteByID([FromBody] List<FlowchartNode> models)
		{
			try
			{
				var data = await _service.DeleteByID(models);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}
	}
}
