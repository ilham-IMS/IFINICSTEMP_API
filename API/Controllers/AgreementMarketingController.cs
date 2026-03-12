using iFinancing360.API.Helper;
using Domain.Abstract.Service;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

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
    private readonly InternalAPIClient _internalAPIClient;

    public AgreementMarketingController(IAgreementMarketingService service, IConfiguration configuration, InternalAPIClient internalAPIClient) : base(configuration)
    {
      _service = service;
      _internalAPIClient = internalAPIClient;
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

        var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
        var sysCompany = resSysCompany?.Data ?? [];

        var resAgreementFee = await _internalAPIClient.GetRows("IFINCORE", "AgreementFee", "GetRows", parameters: new { Keyword = "", Offset=0, Limit = int.MaxValue, AgreementID = dataAgreementMarketing.AgreementID }, headers: headers);
        var AgreementFee = resAgreementFee?.Data ?? [];

        var resAgreementReff = await _internalAPIClient.GetRows("IFINCORE", "AgreementRefund", "GetRows", parameters: new { Keyword = "", Offset=0, Limit = int.MaxValue, AgreementID = dataAgreementMarketing.AgreementID }, headers: headers);
        var AgreementRefund = resAgreementReff?.Data ?? [];

        dataAgreementMarketing.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
        dataAgreementMarketing.CompanyName = sysCompany?["Name"]?.GetValue<string>();

        var agreementFeelist = new List<AgreementFeeList>();
        var dataArr = AgreementFee as List<JsonObject>;
        if (dataArr != null)
        {
          foreach (var obj in dataArr)
          {
            if (obj != null)
            {
              var fees = new AgreementFeeList
              {
                FeeName = obj["FeeName"]?.GetValue<string>(),
                FeeAmount = (obj["FeeAmount"]?.GetValue<decimal>() * 100)?.ToString("N0"),
                FeeRate = (obj["FeeRate"]?.GetValue<decimal>() * 100)?.ToString("N0")
              };
              agreementFeelist.Add(fees);
            }
          }
        }

        var agreementCommlist = new List<AgreementCommissionList>();
        var dataComm = AgreementRefund as List<JsonObject>;
        if (dataComm != null)
        {
          foreach (var obj in dataComm)
          {
            if (obj != null)
            {
              var Comms = new AgreementCommissionList
              {
                CommName = obj["RefundDesc"]?.GetValue<string>(),
                CommAmount = (obj["RefundAmount"]?.GetValue<decimal>())?.ToString("N0"),
                CommRate = (obj["RefundRate"]?.GetValue<decimal>())?.ToString("N0")
              };
              agreementCommlist.Add(Comms);
            }
          }
        }

        var agreementReferrallist = new List<AgreementReferralList>();
        var dataReferral = AgreementFee as List<JsonObject>;
        if (dataReferral != null)
        {
          foreach (var obj in dataReferral)
          {
            if (obj != null)
            {
              var Refs = new AgreementReferralList
              {
                ReferralName = obj["FeeName"]?.GetValue<string>(),
                ReferralAmount = (obj["FeeAmount"]?.GetValue<decimal>() * 100)?.ToString("N0"),
                ReferralRate = (obj["FeeRate"]?.GetValue<decimal>() * 100)?.ToString("N0")
              };
              agreementReferrallist.Add(Refs);
            }
          }
        }

        // --- Generate HTML ---
        var html = await _service.GetPreview(dataAgreementMarketing, ID, agreementFeelist, agreementCommlist, agreementReferrallist);

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

        var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
        var sysCompany = resSysCompany?.Data ?? [];

        var resAgreementFee = await _internalAPIClient.GetRows("IFINCORE", "AgreementFee", "GetRows", parameters: new { Keyword = "", Offset=0, Limit = int.MaxValue, AgreementID = dataAgreementMarketing.AgreementID }, headers: headers);
        var AgreementFee = resAgreementFee?.Data ?? [];

         var resAgreementReff = await _internalAPIClient.GetRows("IFINCORE", "AgreementRefund", "GetRows", parameters: new { Keyword = "", Offset=0, Limit = int.MaxValue, AgreementID = dataAgreementMarketing.AgreementID }, headers: headers);
        var AgreementRefund = resAgreementReff?.Data ?? [];

        dataAgreementMarketing.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
        dataAgreementMarketing.CompanyName = sysCompany?["Name"]?.GetValue<string>();

        var agreementFeelist = new List<AgreementFeeList>();
        var dataArr = AgreementFee as List<JsonObject>;
        if (dataArr != null)
        {
          foreach (var obj in dataArr)
          {
            if (obj != null)
            {
              var tunggakan = new AgreementFeeList
              {
                FeeName = obj["FeeName"]?.GetValue<string>(),
                FeeAmount = (obj["FeeAmount"]?.GetValue<decimal>() * 100)?.ToString("N0"),
                FeeRate = (obj["FeeRate"]?.GetValue<decimal>() * 100)?.ToString("N0")
              };
              agreementFeelist.Add(tunggakan);
            }
          }
        }

        var agreementCommlist = new List<AgreementCommissionList>();
        var dataComm = AgreementRefund as List<JsonObject>;
        if (dataComm != null)
        {
          foreach (var obj in dataComm)
          {
            if (obj != null)
            {
              var tunggakan = new AgreementCommissionList
              {
                CommName = obj["RefundDesc"]?.GetValue<string>(),
                CommAmount = (obj["RefundAmount"]?.GetValue<decimal>())?.ToString("N0"),
                CommRate = (obj["RefundRate"]?.GetValue<decimal>())?.ToString("N0")
              };
              agreementCommlist.Add(tunggakan);
            }
          }
        }

        var agreementReferrallist = new List<AgreementReferralList>();
        var dataReferral = AgreementFee as List<JsonObject>;
        if (dataReferral != null)
        {
          foreach (var obj in dataReferral)
          {
            if (obj != null)
            {
              var Refs = new AgreementReferralList
              {
                ReferralName = obj["FeeName"]?.GetValue<string>(),
                ReferralAmount = (obj["FeeAmount"]?.GetValue<decimal>() * 100)?.ToString("N0"),
                ReferralRate = (obj["FeeRate"]?.GetValue<decimal>() * 100)?.ToString("N0")
              };
              agreementReferrallist.Add(Refs);
            }
          }
        }

        var content = await _service.GenerateDocumentAllTypeDoc(model.MimeType!,model.ID!, dataAgreementMarketing, agreementFeelist, agreementCommlist, agreementReferrallist);
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