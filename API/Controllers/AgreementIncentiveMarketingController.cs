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
  public class AgreementIncentiveMarketingController : BaseController
  {
    private readonly IAgreementIncentiveMarketingService _service;
    private readonly IAgreementFeeService _agreementFeeService;
    private readonly InternalAPIClient _internalAPIClient;

    public AgreementIncentiveMarketingController(IAgreementIncentiveMarketingService service, IConfiguration configuration, InternalAPIClient internalAPIClient, IAgreementFeeService agreementFeeService) : base(configuration)
    {
      _service = service;
      _internalAPIClient = internalAPIClient;
      _agreementFeeService = agreementFeeService;
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

    [HttpGet("GetRowsByIncentiveID")]
    public async Task<ActionResult> GetRowsByIncentiveID(string? keyword, int offset, int limit, string incentiveID)
    {
      try
      {
        var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

        

        var data = await _service.GetRowsByIncentiveID(keyword, offset, limit, incentiveID);

        foreach (var item in data)
        {
          var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = item.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

          item.BPETotalAmount = (item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
          item.BPETotal = (item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0) / (item.NetFinance ?? 1);
          item.BPERatio = (item.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (item.TotalInsurancePremiAmount ?? 1);
          item.BPEIncomeIncentiveExpense = ((item.CommissionRate ?? 0) * (item.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - item.BPETotalAmount;
          item.BPEEffect = item.BPEIncomeIncentiveExpense / ((item.InterestMargin ?? 0) * (item.InterestMarginAmount ?? 0));

          var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", -1);
          item.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

          var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", 1);
          item.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

          item.NonInterestEffectAmount = item.NonInterestIncome - item.NonInterestExpense;
          item.NonInterestEffect = item.NonInterestEffectAmount / ((item.InterestMargin ?? 0) * (item.InterestMarginAmount ?? 0));

          var totalInterestMargin = (item.InterestMargin ?? 0) + (item.BPEEffect ?? 0) + (item.NonInterestEffect ?? 0);
          var profitBeforeMarketingIncentive = (item.InterestMarginAmount ?? 0) + (item.BPEIncomeIncentiveExpense ?? 0) + (item.NonInterestEffectAmount ?? 0);
          item.MarketingIncentiveRatio = profitBeforeMarketingIncentive * 0.0384m;
          item.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (item.MarketingIncentiveRatio ?? 0);
          item.InsurancePremiumUsageRatio = ((item.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (item.TotalInsurancePremiAmount * item.CommissionRate);
          item.ProfitBeforeMarketingIncentive = item.InterestMarginAmount + item.BPEIncomeIncentiveExpense + item.NonInterestEffectAmount;
        }

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
    public async Task<ActionResult> Insert(AgreementIncentiveMarketing module)
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
    public async Task<ActionResult> UpdateByID(AgreementIncentiveMarketing module)
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
          return NotFound("Data agreement incentive marketing not found");

        var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
        var sysCompany = resSysCompany?.Data ?? [];


        dataAgreementMarketing.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
        dataAgreementMarketing.CompanyName = sysCompany?["Name"]?.GetValue<string>();

        var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = dataAgreementMarketing.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

        dataAgreementMarketing.BPETotalAmount = (dataAgreementMarketing.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
        dataAgreementMarketing.BPETotal = ((dataAgreementMarketing.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (dataAgreementMarketing.NetFinance ?? 1);
        dataAgreementMarketing.BPERatio = (dataAgreementMarketing.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (dataAgreementMarketing.TotalInsurancePremiAmount ?? 1);
        dataAgreementMarketing.BPEIncomeIncentiveExpense = ((dataAgreementMarketing.CommissionRate ?? 0) * (dataAgreementMarketing.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - dataAgreementMarketing.BPETotalAmount;
        dataAgreementMarketing.BPEEffect = dataAgreementMarketing.BPEIncomeIncentiveExpense / ((dataAgreementMarketing.InterestMargin ?? 0) * (dataAgreementMarketing.InterestMarginAmount ?? 0));

        var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, int.MaxValue, dataAgreementMarketing.ID ?? "", -1);
        dataAgreementMarketing.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

        var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, int.MaxValue, dataAgreementMarketing.ID ?? "", 1);
        dataAgreementMarketing.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

        dataAgreementMarketing.NonInterestEffectAmount = dataAgreementMarketing.NonInterestIncome - dataAgreementMarketing.NonInterestExpense;

        dataAgreementMarketing.NonInterestEffect = dataAgreementMarketing.NonInterestEffectAmount / ((dataAgreementMarketing.InterestMargin ?? 0) * (dataAgreementMarketing.InterestMarginAmount ?? 0));

        var totalInterestMargin = (dataAgreementMarketing.InterestMargin ?? 0) + (dataAgreementMarketing.BPEEffect ?? 0) + (dataAgreementMarketing.NonInterestEffect ?? 0);

        var profitBeforeMarketingIncentive = (dataAgreementMarketing.InterestMarginAmount ?? 0) + (dataAgreementMarketing.BPEIncomeIncentiveExpense ?? 0) + (dataAgreementMarketing.NonInterestEffectAmount ?? 0);

        dataAgreementMarketing.TotalInterestMargin = totalInterestMargin;
        
        dataAgreementMarketing.MarketingIncentiveRatio = profitBeforeMarketingIncentive * 0.0384m;
        dataAgreementMarketing.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (dataAgreementMarketing.MarketingIncentiveRatio ?? 0);

        dataAgreementMarketing.MarketingIncentiveRatioInterest = dataAgreementMarketing.MarketingIncentiveRatio / (dataAgreementMarketing.InterestMarginAmount * dataAgreementMarketing.InterestMargin * -1);

        dataAgreementMarketing.MarketingIncentiveRatioFinance = dataAgreementMarketing.MarketingIncentiveRatio / dataAgreementMarketing.NetFinance ;

        dataAgreementMarketing.InsurancePremiumUsageRatio = ((dataAgreementMarketing.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (dataAgreementMarketing.TotalInsurancePremiAmount * dataAgreementMarketing.CommissionRate);

        dataAgreementMarketing.ProfitBeforeMarketingIncentive = dataAgreementMarketing.InterestMarginAmount + dataAgreementMarketing.BPEIncomeIncentiveExpense + dataAgreementMarketing.NonInterestEffectAmount;

        dataAgreementMarketing.NetInterestMargin = dataAgreementMarketing.NetInterestMarginAfterCost / (dataAgreementMarketing.InterestMargin * dataAgreementMarketing.InterestMarginAmount);

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
  }
}