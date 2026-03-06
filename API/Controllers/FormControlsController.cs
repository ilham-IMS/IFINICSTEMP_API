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
    public class FormControlsController : BaseController
    {
        private readonly IFormControlsService _service;

        public FormControlsController(
            IFormControlsService service,
            IConfiguration configuration
        )
            : base(configuration)
        {
            _service = service;
        }

        [HttpGet("GetRows")]
        public async Task<ActionResult> GetRows(string MasterFormID)
        {
            try
            {
                var data = await _service.GetRows(MasterFormID);
                return ResponseSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseError(ex);
            }
        }

        [HttpGet("GetRowsForDataTable")]
        public async Task<ActionResult> GetRowsForDataTable(string MasterFormID)
        {
            try
            {
                var data = await _service.GetRowsForDataTable(MasterFormID);
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
        public async Task<ActionResult> Insert([FromBody] FormControls model)
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
        public async Task<ActionResult> UpdateByID([FromBody] FormControls model)
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

        [HttpPut("ChangeStatus")]
        public async Task<ActionResult> ChangeStatus([FromBody] FormControls ID)
        {
            try
            {
                var data = await _service.ChangeStatus(ID);
                return ResponseSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseError(ex);
            }
        }
    }
}
