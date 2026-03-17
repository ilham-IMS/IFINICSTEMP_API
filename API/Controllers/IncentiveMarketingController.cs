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
  public class IncentiveMarketingController : BaseController
  {
    private readonly IIncentiveMarketingService _service;
    private readonly IAgreementIncentiveMarketingService _agreementIncentiveMarketingService;
    private readonly IAgreementFeeService _agreementFeeService;
    private readonly InternalAPIClient _internalAPIClient;
    private readonly IIncentiveSchemeService _incentiveSchemeService;

    public IncentiveMarketingController(IIncentiveMarketingService service, IConfiguration configuration, InternalAPIClient internalAPIClient, IAgreementIncentiveMarketingService agreementIncentiveMarketingService, IAgreementFeeService agreementFeeService, IIncentiveSchemeService incentiveSchemeService) : base(configuration)
    {
      _service = service;
      _internalAPIClient = internalAPIClient;
      _agreementIncentiveMarketingService = agreementIncentiveMarketingService;
      _agreementFeeService = agreementFeeService;
      _incentiveSchemeService = incentiveSchemeService;
    }

    [HttpGet("GetRows")]
    public async Task<ActionResult> GetRows(string? keyword, int offset, int limit, string? PeriodeFrom, string? PeriodeTo)
    {
      try
      {
        var data = await _service.GetRows(keyword, offset, limit, PeriodeFrom, PeriodeTo);
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
    public async Task<ActionResult> Insert(IncentiveMarketing module)
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
    public async Task<ActionResult> UpdateByID(IncentiveMarketing module)
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

    #region GetHTMLPreview
    [HttpPost("GetHTMLPreview")]
    public async Task<ActionResult> GetHTMLPreview(string[] ids, string? PeriodeFrom, string? PeriodeTo)
    {
        try
        {
            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

            var resItParam = await _internalAPIClient.GetRow("IFINSYS", "SysITParameter", "GetRow", headers: headers);
            var itParam = resItParam.Data;
            var SystemDateTime = itParam?["SystemDate"]?.GetValue<DateTime>() ?? throw new Exception("SystemDate is null");

            var resIncentiveScheme = await _incentiveSchemeService.GetIncentiveRatioMarketing(SystemDateTime.Date);

            var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
            var sysCompany = resSysCompany?.Data ?? [];

            var incentiveMarketingData = await _service.GetRowByID(ids.FirstOrDefault() ?? "");

            incentiveMarketingData.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
            incentiveMarketingData.CompanyName = sysCompany?["Name"]?.GetValue<string>();

            var dataAgreement = new List<AgreementIncentiveMarketing>();
            var dataIncentiveMarketing = new List<IncentiveMarketing>();
            foreach (var id in ids)
            {
                var agreementItems = await _agreementIncentiveMarketingService.GetRowsByIncentiveID("", 0, int.MaxValue, id);
                var incentiveMarketingItem = await _service.GetRowByID(id);
                if (incentiveMarketingItem != null)
                {
                    dataIncentiveMarketing.Add(incentiveMarketingItem);
                }
                dataAgreement.AddRange(agreementItems);
            }

            // Determine PeriodeFrom and PeriodeTo
            string periodeFrom = PeriodeFrom ?? "-";
            string periodeTo = PeriodeTo ?? "-";

            if (string.IsNullOrEmpty(PeriodeFrom) && dataIncentiveMarketing.Count > 0)
            {
                var minPeriod = dataIncentiveMarketing
                    .Where(x => !string.IsNullOrEmpty(x.IncentivePeriode) && int.TryParse(x.IncentivePeriode, out _))
                    .OrderBy(x => int.Parse(x.IncentivePeriode))
                    .FirstOrDefault()?.IncentivePeriode;
                periodeFrom = minPeriod ?? "-";
            }

            if (string.IsNullOrEmpty(PeriodeTo) && dataIncentiveMarketing.Count > 0)
            {
                var maxPeriod = dataIncentiveMarketing
                    .Where(x => !string.IsNullOrEmpty(x.IncentivePeriode) && int.TryParse(x.IncentivePeriode, out _))
                    .OrderByDescending(x => int.Parse(x.IncentivePeriode))
                    .FirstOrDefault()?.IncentivePeriode;
                periodeTo = maxPeriod ?? "-";
            }

            incentiveMarketingData.PeriodeFrom = periodeFrom;
            incentiveMarketingData.PeriodeTo = periodeTo;

            foreach (var item in dataAgreement)
            {
                var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = item.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

                item.BPETotalAmount = (item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
                
                var netFinance = (item.NetFinance ?? 1);
                item.BPETotal = netFinance != 0 ? ((item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / netFinance : 0;
                
                var insurancePremium = (item.TotalInsurancePremiAmount ?? 1);
                item.BPERatio = insurancePremium != 0 ? (item.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / insurancePremium : 0;
                
                item.BPEIncomeIncentiveExpense = ((item.CommissionRate ?? 0) * (item.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - item.BPETotalAmount;
                
                var interestMarginDivisor = (item.InterestMargin ?? 0) * (item.InterestMarginAmount ?? 0);
                item.BPEEffect = interestMarginDivisor != 0 ? item.BPEIncomeIncentiveExpense / interestMarginDivisor : 0;

                var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", -1);
                item.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

                var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", 1);
                item.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

                item.NonInterestEffectAmount = item.NonInterestIncome - item.NonInterestExpense;
                item.NonInterestEffect = interestMarginDivisor != 0 ? item.NonInterestEffectAmount / interestMarginDivisor : 0;

                var totalInterestMargin = (item.InterestMargin ?? 0) + (item.BPEEffect ?? 0) + (item.NonInterestEffect ?? 0);
                var profitBeforeMarketingIncentive = (item.InterestMarginAmount ?? 0) + (item.BPEIncomeIncentiveExpense ?? 0) + (item.NonInterestEffectAmount ?? 0);
                item.MarketingIncentiveRatio = profitBeforeMarketingIncentive * (resIncentiveScheme.IncentiveRatio ?? 0);
                item.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (item.MarketingIncentiveRatio ?? 0);
                
                var insurancePremiumDivisor = (item.TotalInsurancePremiAmount * item.CommissionRate);
                item.InsurancePremiumUsageRatio = insurancePremiumDivisor != 0 ? (((item.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / insurancePremiumDivisor) : 0;
                
                item.ProfitBeforeMarketingIncentive = item.InterestMarginAmount + item.BPEIncomeIncentiveExpense + item.NonInterestEffectAmount;
            }

            var result = await _service.GetHTMLPreview(ids.FirstOrDefault() ?? "", incentiveMarketingData, dataAgreement);
            return ResponseSuccess(new { HTML = result });

        }
        catch (Exception ex)
        {
            return ResponseError(ex);
        }
    }
    #endregion

    #region PrintDocument
    [HttpPost("PrintDocument")]
    public async Task<ActionResult> PrintDocument(string mimeType, string[] ids, string? PeriodeFrom, string? PeriodeTo)
    {
        try
        {
            var incentiveMarketingData = await _service.GetRowByID(ids.FirstOrDefault() ?? "");

            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

            var resItParam = await _internalAPIClient.GetRow("IFINSYS", "SysITParameter", "GetRow", headers: headers);
            var itParam = resItParam.Data;
            var SystemDateTime = itParam?["SystemDate"]?.GetValue<DateTime>() ?? throw new Exception("SystemDate is null");

            var resIncentiveScheme = await _incentiveSchemeService.GetIncentiveRatioMarketing(SystemDateTime.Date);

            var resSysCompany = await _internalAPIClient.GetRow("IFINSYS", "SysCompany", "GetRowByCode", parameters: new { code = "COMP" }, headers: headers);
            var sysCompany = resSysCompany?.Data ?? [];

            incentiveMarketingData.CompanyFileName = sysCompany?["FileName"]?.GetValue<string>();
            incentiveMarketingData.CompanyName = sysCompany?["Name"]?.GetValue<string>();

            var dataAgreement = new List<AgreementIncentiveMarketing>();
            var dataIncentiveMarketing = new List<IncentiveMarketing>();
            foreach (var id in ids)
            {
                var agreementItems = await _agreementIncentiveMarketingService.GetRowsByIncentiveID("", 0, int.MaxValue, id);
                var incentiveMarketingItem = await _service.GetRowByID(id);
                if (incentiveMarketingItem != null)
                {
                    dataIncentiveMarketing.Add(incentiveMarketingItem);
                }
                dataAgreement.AddRange(agreementItems);
            }

            // Determine PeriodeFrom and PeriodeTo
            string periodeFrom = PeriodeFrom ?? "-";
            string periodeTo = PeriodeTo ?? "-";

            if (string.IsNullOrEmpty(PeriodeFrom) && dataIncentiveMarketing.Count > 0)
            {
                var minPeriod = dataIncentiveMarketing
                    .Where(x => !string.IsNullOrEmpty(x.IncentivePeriode) && int.TryParse(x.IncentivePeriode, out _))
                    .OrderBy(x => int.Parse(x.IncentivePeriode))
                    .FirstOrDefault()?.IncentivePeriode;
                periodeFrom = minPeriod ?? "-";
            }

            if (string.IsNullOrEmpty(PeriodeTo) && dataIncentiveMarketing.Count > 0)
            {
                var maxPeriod = dataIncentiveMarketing
                    .Where(x => !string.IsNullOrEmpty(x.IncentivePeriode) && int.TryParse(x.IncentivePeriode, out _))
                    .OrderByDescending(x => int.Parse(x.IncentivePeriode))
                    .FirstOrDefault()?.IncentivePeriode;
                periodeTo = maxPeriod ?? "-";
            }

            incentiveMarketingData.PeriodeFrom = periodeFrom;
            incentiveMarketingData.PeriodeTo = periodeTo;

            foreach (var item in dataAgreement)
            {
                var resFeeProv = await _internalAPIClient.GetRow("IFINLOS", "ApplicationFee", "GetRowByApplicationMainIDFeeCode", parameters: new { ApplicationMainID = item.ApplicationMainID, FeeCode = "PROV" }, headers: headers);

                item.BPETotalAmount = (item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0);
                
                var netFinance = (item.NetFinance ?? 1);
                item.BPETotal = netFinance != 0 ? ((item.TotalRefundAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / netFinance : 0;
                
                var insurancePremium = (item.TotalInsurancePremiAmount ?? 1);
                item.BPERatio = insurancePremium != 0 ? (item.BPETotalAmount - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / insurancePremium : 0;
                
                item.BPEIncomeIncentiveExpense = ((item.CommissionRate ?? 0) * (item.TotalInsurancePremiAmount ?? 0) + (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) - item.BPETotalAmount;
                
                var interestMarginDivisor = (item.InterestMargin ?? 0) * (item.InterestMarginAmount ?? 0);
                item.BPEEffect = interestMarginDivisor != 0 ? item.BPEIncomeIncentiveExpense / interestMarginDivisor : 0;

                var resFeeNon = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", -1);
                item.NonInterestExpense = resFeeNon?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

                var resFeeInt = await _agreementFeeService.GetRowsByAgreementID("", 0, 1, item.ID ?? "", 1);
                item.NonInterestIncome = resFeeInt?.Where(x => x.FeeAmount != null).Sum(x => x.FeeAmount ?? 0) ?? 0;

                item.NonInterestEffectAmount = item.NonInterestIncome - item.NonInterestExpense;
                item.NonInterestEffect = interestMarginDivisor != 0 ? item.NonInterestEffectAmount / interestMarginDivisor : 0;

                var totalInterestMargin = (item.InterestMargin ?? 0) + (item.BPEEffect ?? 0) + (item.NonInterestEffect ?? 0);
                var profitBeforeMarketingIncentive = (item.InterestMarginAmount ?? 0) + (item.BPEIncomeIncentiveExpense ?? 0) + (item.NonInterestEffectAmount ?? 0);
                item.MarketingIncentiveRatio = profitBeforeMarketingIncentive * (resIncentiveScheme.IncentiveRatio ?? 0);
                item.NetInterestMarginAfterCost = profitBeforeMarketingIncentive - (item.MarketingIncentiveRatio ?? 0);
                
                var insurancePremiumDivisor = (item.TotalInsurancePremiAmount * item.CommissionRate);
                item.InsurancePremiumUsageRatio = insurancePremiumDivisor != 0 ? (((item.BPETotalAmount ?? 0) - (resFeeProv?.Data?["FeeAmount"]?.GetValue<decimal>() ?? 0)) / insurancePremiumDivisor) : 0;
                
                item.ProfitBeforeMarketingIncentive = item.InterestMarginAmount + item.BPEIncomeIncentiveExpense + item.NonInterestEffectAmount;
            }

            var content = await _service.PrintDocument(mimeType, ids.FirstOrDefault() ?? "", incentiveMarketingData, dataAgreement);
            return ResponseSuccess(content);

        }
        catch (Exception ex)
        {
            return ResponseError(ex);
        }
    }
    #endregion
    
  }
}