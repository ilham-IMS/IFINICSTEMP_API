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
  public class AgreementMarketingController : BaseController
  {
    private readonly IAgreementMarketingService _service;

    public AgreementMarketingController(IAgreementMarketingService service, IConfiguration configuration) : base(configuration)
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
    public async Task<ActionResult> Insert(AgreementMarketing module)
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
    public async Task<ActionResult> UpdateByID(AgreementMarketing module)
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
    
    [HttpGet("GetHTMLPreview")]
    public async Task<ActionResult> GetHTMLPreview(string ID)
    {
      var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

      try
      {
        // Validasi ID dan MimeType dari model
        if (string.IsNullOrEmpty(ID))
          return BadRequest("ID is required");

        // --- Ambil data AgreementMarketing ---
        var dataAgreementMarketing = await _service.GetRowByID(ID);
        if (dataAgreementMarketing == null)
          return NotFound("Data agreement marketing not found");

        

        // --- Generate HTML ---
        var html = await _service.GetPreview(dataAgreementMarketing, ID);

        return File(html.Content, html.MimeType, html.Name);
        // return ResponseSuccess(new { HTML = html });
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }

    [HttpPost("PrintDocument")]
    public async Task<ActionResult> PrintDocument([FromBody] AgreementMarketing model)
    {
      var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

      try
      {
        // Validasi ID dan MimeType dari model
        if (string.IsNullOrEmpty(model.ID))
          return BadRequest("ID is required");

        if (string.IsNullOrEmpty(model.MimeType))
          return BadRequest("MimeType is required");

        // --- Ambil data AgreementMarketing ---
        var dataAgreementMarketing = await _service.GetRowByID(model.ID);
        if (dataAgreementMarketing == null)
          return NotFound("Data agreement marketing not found");

        // --- Transfer properties penting dari model input ---
        dataAgreementMarketing.MimeType = model.MimeType;
        dataAgreementMarketing.ModBy = model.ModBy;
        dataAgreementMarketing.ModDate = model.ModDate;
        dataAgreementMarketing.ModIPAddress = model.ModIPAddress;

        // // --- Call ke CORE untuk melengkapi data ---
        // var resp = await _internalAPIClient.GetRow(
        //     "IFINCORE",
        //     "AgreementMain",
        //     "GetAgreementNoForReportWLDelivery",
        //     new { AgreementNo = dataWarningLetter.AgreementNo },
        //     headers
        // );

        // var listTunggakan = new List<WarningLetterDeliveryDetail>();
        // var dataArr = respTunggakan?.Data as List<JsonObject>;
        // if (dataArr != null)
        // {
        //   foreach (var obj in dataArr)
        //   {
        //     if (obj != null)
        //     {
        //       var tunggakan = new WarningLetterDeliveryDetail
        //       {
        //         AgreementNo = obj["AgreementNo"]?.ToString(),
        //         DueDate = obj["DueDate"]?.GetValue<DateTime>(),
        //         OvdDays = obj["OvdDays"]?.GetValue<int>(),
        //         InstallmentAmount = obj["InstallmentAmount"]?.GetValue<decimal>(),
        //         PorsiPokok = obj["PorsiPokok"]?.GetValue<decimal>(),
        //         Bunga = obj["Bunga"]?.GetValue<decimal>(),
        //         Denda = obj["Denda"]?.GetValue<decimal>(),
        //         TotalTunggakan = obj["TotalTunggakan"]?.GetValue<decimal>()
        //       };
        //       listTunggakan.Add(tunggakan);
        //     }
        //   }
        // }

        // // --- Assign data tunggakan dari CORE (prioritas utama) ---
        // if (listTunggakan.Count > 0)
        // {
        //   dataWarningLetter.ListTunggakan = listTunggakan;
        // }

        
        var content = await _service.GenerateDocumentAllTypeDoc(model.MimeType!,model.ID!, dataAgreementMarketing);
        return ResponseSuccess(content);
        // var result = await _service.PrintDocument(dataWarningLetter);

        // return ResponseSuccess(result);
      }
      catch (Exception ex)
      {
        return ResponseError(ex);
      }
    }
  }
}