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
	public class MasterDynamicReportColumnController : BaseController
	{
		private readonly IMasterDynamicReportColumnService _service;

		public MasterDynamicReportColumnController(
				IMasterDynamicReportColumnService service,
				IConfiguration configuration
		)
				: base(configuration)
		{
			_service = service;
		}

		[HttpGet("GetRows")]
		public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string masterDynamicReportTableID)
		{
			try
			{
				var data = await _service.GetRows(keyword, offset, limit, masterDynamicReportTableID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}
		[HttpGet("GetRowsForLookupByDynamicReport")]
		public async Task<ActionResult> GetRowsForLookupByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
		{
			try
			{
				var data = await _service.GetRowsForLookupByDynamicReport(keyword, offset, limit, dynamicReportID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}

		[HttpGet("GetRowsForLookupByDynamicReportTable")]
		public async Task<ActionResult> GetRowsForLookupByDynamicReportTable(string? keyword, int offset, int limit, string dynamicReportTableID)
		{
			try
			{
				var data = await _service.GetRowsForLookupByDynamicReportTable(keyword, offset, limit, dynamicReportTableID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}
		[HttpGet("GetRowsForLookupByDynamicReportTableForRelatedColumn")]
		public async Task<ActionResult> GetRowsForLookupByDynamicReportTableForRelatedColumn(string? keyword, int offset, int limit, string dynamicReportTableID, string relatedMasterDynamicReportColumnID)
		{
			try
			{
				var data = await _service.GetRowsForLookupByDynamicReportTableForRelatedColumn(keyword, offset, limit, dynamicReportTableID, relatedMasterDynamicReportColumnID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}

		[HttpGet("GetRowsForLookup")]
		public async Task<ActionResult> GetRowsForLookup(string? keyword, int offset, int limit, string masterDynamicReportTableID)
		{
			try
			{
				var data = await _service.GetRowsForLookup(keyword, offset, limit, masterDynamicReportTableID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}
		[HttpGet("GetRowsForLookupExcludeByDynamicReport")]
		public async Task<ActionResult> GetRowsForLookupExcludeByDynamicReport(string? keyword, int offset, int limit, string dynamicReportID)
		{
			try
			{
				var data = await _service.GetRowsForLookupExcludeByDynamicReport(keyword, offset, limit, dynamicReportID);
				return ResponseSuccess(data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}

		[HttpGet("GetRowsForeignReferenceToTable")]
		public async Task<ActionResult> GetRowsForeignReferenceToTable(string dynamicReportID, string masterDynamicReportTableID)
		{
			try
			{
				var data = await _service.GetRowsForeignReferenceToTable(dynamicReportID, masterDynamicReportTableID);
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
		public async Task<ActionResult> Insert([FromBody] MasterDynamicReportColumn model)
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
		public async Task<ActionResult> UpdateByID([FromBody] IEnumerable<MasterDynamicReportColumn> models)
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

		[HttpPut("ChangeAvailable")]
		public async Task<ActionResult> ChangeAvailable(MasterDynamicReportColumn model)
		{
			try
			{
				var data = await _service.ChangeAvailable(model);
				return ResponseSuccess(new { }, data);
			}
			catch (Exception ex)
			{
				return ResponseError(ex);
			}
		}
		[HttpPut("ChangeMaskingStatus")]
		public async Task<ActionResult> ChangeMaskingStatus(MasterDynamicReportColumn model)
		{
			try
			{
				var data = await _service.ChangeMaskingStatus(model);
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
