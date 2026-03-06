using iFinancing360.API.Helper;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Abstract.Service;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Reflection;
using System.Net;
using System.Text.Json.Nodes;

namespace API.Controllers
{
  [Route("/api/[controller]")]
  [SetBaseModelProperties]
  [ApiController]
  [UserAuthorize]
  [DecryptQueryString]
  [DecryptRequestBody]
  public class DynamicButtonProcessController : BaseController
  {
    private readonly IDynamicButtonProcessService _service;
    private readonly InternalAPIClient _internalAPIClient;

    public DynamicButtonProcessController(IDynamicButtonProcessService service, IConfiguration configuration, InternalAPIClient internalAPIClient) : base(configuration)
    {
      _service = service;
      _internalAPIClient = internalAPIClient;
    }

    #region HttpGet
    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string ParentMenuID)
    {
      try
      {
        var data = await _service.GetRows(keyword, offset, limit, ParentMenuID);
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

    #region SyncButtonProcess
    [HttpPost("SyncButtonProcess")]
    public async Task<ActionResult> SyncButtonProcess()
    {
      try
      {
        var menuFormSYs = await _internalAPIClient.GetRows("IFINSYS", "SysMenu", "GetRowsByModuleCodeAndIsDynamic", new { ModuleCode = "IFINICS" }, GetHeaders());

        List<DynamicButtonProcess> mappedList = MapJobToClassList<DynamicButtonProcess>(menuFormSYs);

        var result = await _service.SyncButtonProcess(mappedList);
        return ResponseSuccess(result);
      }

      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }




    #endregion GetRowsForLookupParent

    [HttpGet("GetRowsForLookupParent")]
    public async Task<ActionResult> GetRowsForLookupParent(string? keyword, int offset, int limit, bool withAll)
    {
      try
      {
        var data = await _service.GetRowsForLookupParent(keyword, offset, limit);
        return ResponseSuccess(data);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
    private Dictionary<string, string> GetHeaders()
    {
      return Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
      ;
    }

    private ActionResult ResponseSuccess<T>(IEnumerable<T> data, bool credValue = false)
    {
      if (credValue == true)
      {
        return Ok(data);
      }

      List<Dictionary<string, object>> data2 = data.Select((T d) => (from prop in d?.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public) where prop.Name == "ID" || prop.DeclaringType == d.GetType() select prop).ToDictionary((PropertyInfo prop) => prop.Name, (PropertyInfo prop) => prop.GetValue(d))).ToList();
      BodyResponse<List<Dictionary<string, object>>> value = Api.CreateResponse(data.Count(), HttpStatusCode.OK, data2, "");
      return Ok(value);
    }


    protected T? MapJobToClassObj<T>(BodyResponse<JsonObject> jsonObj)
    {
      if (jsonObj?.Data is null)
        throw new ArgumentException("Data is null");

      var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

      // Tidak perlu serialize ulang—langsung deserialize dari JsonObject
      return jsonObj.Data.Deserialize<T>(options);
    }

    protected List<T> MapJobToClassList<T>(BodyResponse<List<JsonObject>>? jsonObj)
    {
      if (jsonObj?.Data == null)
        throw new ArgumentException("Data is null");

      string jsonString = JsonSerializer.Serialize(jsonObj.Data);

      List<T>? mappedList = JsonSerializer.Deserialize<List<T>>(jsonString,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      return mappedList ?? new List<T>();
    }



  }
}