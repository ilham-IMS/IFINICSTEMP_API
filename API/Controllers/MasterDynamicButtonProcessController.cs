using Microsoft.AspNetCore.Mvc;
using iFinancing360.API.Helper;
using Domain.Abstract.Service;
using Domain.Models;
using DotNetEnv;
using iFinancing360.Helper;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [UserAuthorize] // Tambahkan Authorize disini
  [ApiController]
  [SetBaseModelProperties]
  [DecryptQueryString]
	[DecryptRequestBody]
  public class MasterDynamicButtonProcessController : BaseController
  {
    private readonly IMasterDynamicButtonProcessService _service;
    public MasterDynamicButtonProcessController(IMasterDynamicButtonProcessService service, IConfiguration configuration) : base(configuration)
    {
      _service = service;
    }

    [HttpPost("CheckDllExist")]
    public async Task<ActionResult> CheckDllExist([FromBody] MasterDynamicButtonProcess model)
    {
      try
      {
        string assemblyName = $"{model.DllName}{model.NamespaceName}{model.ClassName}{model.MethodName}";
        bool result = await _service.IsDLLExist(assemblyName);
        return ResponseSuccess(new { Found = result });

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    #region GetRows


    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit)
    {
      try
      {
        List<MasterDynamicButtonProcess> result = await _service.GetRows(keyword, offset, limit);
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
        MasterDynamicButtonProcess result = await _service.GetRowByID(id);
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
    public async Task<ActionResult> Insert(MasterDynamicButtonProcess model)
    {
      try
      {
        return ResponseSuccess(new { model.ID }, await _service.Insert(model));

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    #endregion

    #region UpdateByID
    [HttpPut("UpdateByID")]
    public async Task<ActionResult> UpdateByID(MasterDynamicButtonProcess model)
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

    [HttpGet("GetRowsForLookup")]
    public async Task<ActionResult> GetRowsForLookup(string? keyword, int offset, int limit)
    {
      try
      {
        List<MasterDynamicButtonProcess> result = await _service.GetRowsForLookup(keyword, offset, limit);
        return ResponseSuccess(result);

      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPost("UploadDLL")]
    public async Task<IActionResult> UploadDll([FromBody] FileDoc fileDoc)
    {
      if (fileDoc?.Content == null || fileDoc.Content.Length == 0)
      {
        return BadRequest("No file uploaded.");
      }

      if (string.IsNullOrEmpty(fileDoc.Name))
      {
        return BadRequest("File name is required.");
      }

      // Tentukan path penyimpanan DLL di dalam container
      var filePath = Path.Combine(Env.GetString("DLL_PATH"), fileDoc.Name);

      try
      {
        // Simpan DLL ke dalam container
        await System.IO.File.WriteAllBytesAsync(filePath, fileDoc.Content);
        return ResponseSuccess(new { File = $"DLL {fileDoc.Name} uploaded successfully." });
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

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

  }
}