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
    private readonly IIncentiveSchemeService _incentiveSchemeService;

    public AgreementIncentiveMarketingController(IAgreementIncentiveMarketingService service, IConfiguration configuration, InternalAPIClient internalAPIClient, IAgreementFeeService agreementFeeService, IIncentiveSchemeService incentiveSchemeService) : base(configuration)
    {
      _service = service;
      _internalAPIClient = internalAPIClient;
      _agreementFeeService = agreementFeeService;
      _incentiveSchemeService = incentiveSchemeService;
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

        var resItParam = await _internalAPIClient.GetRow("IFINSYS", "SysITParameter", "GetRow", headers: headers);
        var itParam = resItParam.Data;
        var SystemDateTime = itParam?["SystemDate"]?.GetValue<DateTime>() ?? throw new Exception("SystemDate is null");

        var data = await _service.GetRowsByIncentiveID(keyword, offset, limit, incentiveID);

        var resIncentiveScheme = await _incentiveSchemeService.GetIncentiveRatioMarketing(SystemDateTime.Date);

        foreach (var item in data)
        {
          var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = item.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

          item.BPETotalAmount = (item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
          item.BPETotal = (item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0) / (item.NetFinance ?? 1);
          item.BPERatio = (item.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (item.TotalInsurancePremiAmount ?? 1);
          item.BPEIncomeIncentiveExpense = ((item.CommissionRate ?? 0) * (item.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - item.BPETotalAmount;
          item.BPEEffect = ((item.InterestMargin ?? 1) * (item.InterestMarginAmount ?? 1)) != 0 ? item.BPEIncomeIncentiveExpense / ((item.InterestMargin ?? 1) * (item.InterestMarginAmount ?? 1)) : 0;

          var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", -1);
          item.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

          var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", 1);
          item.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

          item.NonInterestEffectAmount = item.NonInterestIncome - item.NonInterestExpense;
          item.NonInterestEffect = ((item.InterestMargin ?? 1) * (item.InterestMarginAmount ?? 1)) != 0 ? item.NonInterestEffectAmount / ((item.InterestMargin ?? 1) * (item.InterestMarginAmount ?? 1)) : 0;

          var totalInterestMargin = (item.InterestMargin ?? 0) + (item.BPEEffect ?? 0) + (item.NonInterestEffect ?? 0);
          var profitBeforeMarketingIncentive = (item.InterestMarginAmount ?? 0) + (item.BPEIncomeIncentiveExpense ?? 0) + (item.NonInterestEffectAmount ?? 0);
          item.MarketingIncentiveRatio = profitBeforeMarketingIncentive * (resIncentiveScheme.IncentiveRatio ?? 0);
          item.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (item.MarketingIncentiveRatio ?? 0);
          item.InsurancePremiumUsageRatio = ((item.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / (item.TotalInsurancePremiAmount ?? 1 * item.CommissionRate ?? 1);
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
            if (string.IsNullOrEmpty(ID))
                return BadRequest("ID is required");

            var resItParam = await _internalAPIClient.GetRow("IFINSYS", "SysITParameter", "GetRow", headers: headers);
            var itParam = resItParam.Data;
            var SystemDateTime = itParam?["SystemDate"]?.GetValue<DateTime>() ?? throw new Exception("SystemDate is null");

            var resIncentiveScheme = await _incentiveSchemeService.GetIncentiveRatioMarketing(SystemDateTime.Date);

            var dataAgreementMarketing = await _service.GetRowByID(ID);
            if (dataAgreementMarketing == null)
                return NotFound("Data agreement incentive marketing not found");

            var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
            var sysCompany = resSysCompany?.Data ?? [];

            dataAgreementMarketing.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
            dataAgreementMarketing.CompanyName = sysCompany?["Name"]?.GetValue<string>();

            dataAgreementMarketing.IncentiveRatio = resIncentiveScheme.IncentiveRatio;

            var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = dataAgreementMarketing.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

            dataAgreementMarketing.ProvisionFeeAmount = resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0;
            dataAgreementMarketing.BPETotalAmount = (dataAgreementMarketing.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
            
            var netFinance = (dataAgreementMarketing.NetFinance ?? 1);
            dataAgreementMarketing.BPETotal = netFinance != 0 ? ((dataAgreementMarketing.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / netFinance : 0;
            
            var totalInsurancePremium = (dataAgreementMarketing.TotalInsurancePremiAmount ?? 1);
            dataAgreementMarketing.BPERatio = totalInsurancePremium != 0 ? (dataAgreementMarketing.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / totalInsurancePremium : 0;
            
            dataAgreementMarketing.BPEIncomeIncentiveExpense = ((dataAgreementMarketing.CommissionRate ?? 0) * (dataAgreementMarketing.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - dataAgreementMarketing.BPETotalAmount;
            
            var interestMarginDivisor = (dataAgreementMarketing.InterestMargin ?? 0) * (dataAgreementMarketing.InterestMarginAmount ?? 0);
            dataAgreementMarketing.BPEEffect = interestMarginDivisor != 0 ? dataAgreementMarketing.BPEIncomeIncentiveExpense / interestMarginDivisor : 0;

            var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, int.MaxValue, dataAgreementMarketing.ID ?? "", -1);
            dataAgreementMarketing.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

            var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, int.MaxValue, dataAgreementMarketing.ID ?? "", 1);
            dataAgreementMarketing.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

            dataAgreementMarketing.NonInterestEffectAmount = dataAgreementMarketing.NonInterestIncome - dataAgreementMarketing.NonInterestExpense;
            dataAgreementMarketing.NonInterestEffect = interestMarginDivisor != 0 ? dataAgreementMarketing.NonInterestEffectAmount / interestMarginDivisor : 0;

            var totalInterestMargin = (dataAgreementMarketing.InterestMargin ?? 0) + (dataAgreementMarketing.BPEEffect ?? 0) + (dataAgreementMarketing.NonInterestEffect ?? 0);
            var profitBeforeMarketingIncentive = (dataAgreementMarketing.InterestMarginAmount ?? 0) + (dataAgreementMarketing.BPEIncomeIncentiveExpense ?? 0) + (dataAgreementMarketing.NonInterestEffectAmount ?? 0);

            dataAgreementMarketing.TotalInterestMargin = totalInterestMargin;
            dataAgreementMarketing.MarketingIncentiveRatio = profitBeforeMarketingIncentive * (resIncentiveScheme.IncentiveRatio ?? 0);
            dataAgreementMarketing.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (dataAgreementMarketing.MarketingIncentiveRatio ?? 0);

            var interestMarginRatioDivisor = (dataAgreementMarketing.InterestMarginAmount * dataAgreementMarketing.InterestMargin * -1);
            dataAgreementMarketing.MarketingIncentiveRatioInterest = interestMarginRatioDivisor != 0 ? dataAgreementMarketing.MarketingIncentiveRatio / interestMarginRatioDivisor : 0;

            dataAgreementMarketing.MarketingIncentiveRatioFinance = netFinance != 0 ? dataAgreementMarketing.MarketingIncentiveRatio / netFinance : 0;

            var insurancePremiumDivisor = (dataAgreementMarketing.TotalInsurancePremiAmount * dataAgreementMarketing.CommissionRate);
            dataAgreementMarketing.InsurancePremiumUsageRatio = insurancePremiumDivisor != 0 ? (((dataAgreementMarketing.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / insurancePremiumDivisor) : 0;

            dataAgreementMarketing.ProfitBeforeMarketingIncentive = dataAgreementMarketing.InterestMarginAmount + dataAgreementMarketing.BPEIncomeIncentiveExpense + dataAgreementMarketing.NonInterestEffectAmount;

            var netInterestMarginDivisor = (dataAgreementMarketing.InterestMargin * dataAgreementMarketing.InterestMarginAmount);
            dataAgreementMarketing.NetInterestMargin = netInterestMarginDivisor != 0 ? dataAgreementMarketing.NetInterestMarginAfterCost / netInterestMarginDivisor : 0;

            var html = await _service.GetPreview(dataAgreementMarketing, ID);
            return File(html.Content, html.MimeType, html.Name);
        }
        catch (Exception ex)
        {
            return ResponseError(ex);
        }
    }

    [HttpPost("PrintDocument")]
    public async Task<ActionResult> PrintDocument([FromBody] AgreementIncentiveMarketing model)
    {
        var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

        try
        {
            if (string.IsNullOrEmpty(model.ID))
                return BadRequest("ID is required");

            var resItParam = await _internalAPIClient.GetRow("IFINSYS", "SysITParameter", "GetRow", headers: headers);
            var itParam = resItParam.Data;
            var SystemDateTime = itParam?["SystemDate"]?.GetValue<DateTime>() ?? throw new Exception("SystemDate is null");

            var resIncentiveScheme = await _incentiveSchemeService.GetIncentiveRatioMarketing(SystemDateTime.Date);

            if (string.IsNullOrEmpty(model.MimeType))
                return BadRequest("MimeType is required");

            var dataAgreementMarketing = await _service.GetRowByID(model.ID);
            if (dataAgreementMarketing == null)
                return NotFound("Data agreement marketing not found");

            var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
            var sysCompany = resSysCompany?.Data ?? [];

            dataAgreementMarketing.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
            dataAgreementMarketing.CompanyName = sysCompany?["Name"]?.GetValue<string>();

            dataAgreementMarketing.IncentiveRatio = resIncentiveScheme.IncentiveRatio;

            var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = dataAgreementMarketing.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

            dataAgreementMarketing.ProvisionFeeAmount = resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0;
            dataAgreementMarketing.BPETotalAmount = (dataAgreementMarketing.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
            
            var netFinance = (dataAgreementMarketing.NetFinance ?? 1);
            dataAgreementMarketing.BPETotal = netFinance != 0 ? ((dataAgreementMarketing.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / netFinance : 0;
            
            var totalInsurancePremium = (dataAgreementMarketing.TotalInsurancePremiAmount ?? 1);
            dataAgreementMarketing.BPERatio = totalInsurancePremium != 0 ? (dataAgreementMarketing.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / totalInsurancePremium : 0;
            
            dataAgreementMarketing.BPEIncomeIncentiveExpense = ((dataAgreementMarketing.CommissionRate ?? 0) * (dataAgreementMarketing.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - dataAgreementMarketing.BPETotalAmount;
            
            var interestMarginDivisor = (dataAgreementMarketing.InterestMargin ?? 0) * (dataAgreementMarketing.InterestMarginAmount ?? 0);
            dataAgreementMarketing.BPEEffect = interestMarginDivisor != 0 ? dataAgreementMarketing.BPEIncomeIncentiveExpense / interestMarginDivisor : 0;

            var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, int.MaxValue, dataAgreementMarketing.ID ?? "", -1);
            dataAgreementMarketing.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

            var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, int.MaxValue, dataAgreementMarketing.ID ?? "", 1);
            dataAgreementMarketing.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

            dataAgreementMarketing.NonInterestEffectAmount = dataAgreementMarketing.NonInterestIncome - dataAgreementMarketing.NonInterestExpense;
            dataAgreementMarketing.NonInterestEffect = interestMarginDivisor != 0 ? dataAgreementMarketing.NonInterestEffectAmount / interestMarginDivisor : 0;

            var totalInterestMargin = (dataAgreementMarketing.InterestMargin ?? 0) + (dataAgreementMarketing.BPEEffect ?? 0) + (dataAgreementMarketing.NonInterestEffect ?? 0);
            var profitBeforeMarketingIncentive = (dataAgreementMarketing.InterestMarginAmount ?? 0) + (dataAgreementMarketing.BPEIncomeIncentiveExpense ?? 0) + (dataAgreementMarketing.NonInterestEffectAmount ?? 0);

            dataAgreementMarketing.TotalInterestMargin = totalInterestMargin;
            dataAgreementMarketing.MarketingIncentiveRatio = profitBeforeMarketingIncentive * (resIncentiveScheme.IncentiveRatio ?? 0);
            dataAgreementMarketing.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (dataAgreementMarketing.MarketingIncentiveRatio ?? 0);

            var interestMarginRatioDivisor = (dataAgreementMarketing.InterestMarginAmount * dataAgreementMarketing.InterestMargin * -1);
            dataAgreementMarketing.MarketingIncentiveRatioInterest = interestMarginRatioDivisor != 0 ? dataAgreementMarketing.MarketingIncentiveRatio / interestMarginRatioDivisor : 0;

            dataAgreementMarketing.MarketingIncentiveRatioFinance = netFinance != 0 ? dataAgreementMarketing.MarketingIncentiveRatio / netFinance : 0;

            var insurancePremiumDivisor = (dataAgreementMarketing.TotalInsurancePremiAmount * dataAgreementMarketing.CommissionRate);
            dataAgreementMarketing.InsurancePremiumUsageRatio = insurancePremiumDivisor != 0 ? (((dataAgreementMarketing.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / insurancePremiumDivisor) : 0;

            dataAgreementMarketing.ProfitBeforeMarketingIncentive = dataAgreementMarketing.InterestMarginAmount + dataAgreementMarketing.BPEIncomeIncentiveExpense + dataAgreementMarketing.NonInterestEffectAmount;

            var netInterestMarginDivisor = (dataAgreementMarketing.InterestMargin * dataAgreementMarketing.InterestMarginAmount);
            dataAgreementMarketing.NetInterestMargin = netInterestMarginDivisor != 0 ? dataAgreementMarketing.NetInterestMarginAfterCost / netInterestMarginDivisor : 0;

            var content = await _service.GenerateDocumentAllTypeDoc(model.MimeType!, model.ID!, dataAgreementMarketing);
            return ResponseSuccess(content);
        }
        catch (Exception ex)
        {
            return ResponseError(ex);
        }
    }
  }
}