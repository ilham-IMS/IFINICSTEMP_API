
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;
using System.Web.Mvc;
using iFinancing360.Service.Helper.SMBClient;

namespace Service
{
  public class AgreementIncentiveMarketingService : BaseService, IAgreementIncentiveMarketingService
  {

    private readonly IAgreementIncentiveMarketingRepository _repo;
    private readonly IAgreementFeeRepository _agreementFeeRepo;
    private readonly IAgreementRefundRepository _agreementRefundRepo;
    private readonly IIncentiveMarketingRepository _incentiveMarketingRepo;
    private readonly NanoDocClient _nanoDocClient;
    public AgreementIncentiveMarketingService(IAgreementIncentiveMarketingRepository repo, IAgreementFeeRepository agreementFeeRepo, IAgreementRefundRepository agreementRefundRepo, IIncentiveMarketingRepository incentiveMarketingRepo, NanoDocClient nanoDocClient)
    {
      _repo = repo;
      _agreementFeeRepo = agreementFeeRepo;
      _agreementRefundRepo = agreementRefundRepo;
      _incentiveMarketingRepo = incentiveMarketingRepo;
      _nanoDocClient = nanoDocClient;
    }


    public async Task<AgreementIncentiveMarketing> GetRowByID(string id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = await _repo.GetRowByID(transaction, id);
          transaction.Commit();
          return result;
        }

        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }

    public async Task<List<AgreementIncentiveMarketing>> GetRows(string? keyword, int offset, int limit)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = await _repo.GetRows(transaction, keyword, offset, limit);
          transaction.Commit();
          return result;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }

    public async Task<List<AgreementIncentiveMarketing>> GetRowsByIncentiveID(string? keyword, int offset, int limit, string incentiveID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = await _repo.GetRowsByIncentiveID(transaction, keyword, offset, limit, incentiveID);
          transaction.Commit();
          return result;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }

    public async Task<int> Insert(AgreementIncentiveMarketing model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          
          int result = await _repo.Insert(transaction, model);
          transaction.Commit();
          return result;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }

    public async Task<int> UpdateByID(AgreementIncentiveMarketing model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
          try
          {
            
          int result = await _repo.UpdateByID(transaction, model);
          transaction.Commit();
          return result;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }
    
    public async Task<int> DeleteByID(string[] idList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = 0;
          foreach (var id in idList)
          {
            result += await _repo.DeleteByID(transaction, id);
          }
          transaction.Commit();

          return result;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }

    public async Task<FileDoc> GetPreview(AgreementIncentiveMarketing dataAgreementMarketing, string ID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        FileDoc fileDoc = await GenerateDocumentFromTemplate(transaction,"docx", ID, dataAgreementMarketing);

        transaction.Commit();
        return fileDoc;
      }
      catch (Exception ex)
      {
        System.Console.WriteLine(ex.Message);
        transaction.Rollback();
        throw;
      }
    }

    private async Task<FileDoc> GenerateDocumentFromTemplate(IDbTransaction transaction, string outputFormat, string ID, AgreementIncentiveMarketing dataAgreementMarketing)
    {

        
        string basePath = Env.GetString("REPORT_TEMPLATE_PATH");
        string templatePath = Path.Combine(basePath, "AgreementTemplate.docx");
       
        if (!File.Exists(templatePath))
        {
          throw new FileNotFoundException($"Template tidak ditemukan di path: {templatePath}", templatePath);
        }

        // var file = await GetFile(dataAgreementMarketing.CompanyFileName!);

        // // Convert file ke base64 untuk digunakan sebagai inline image
        // string base64Image = Convert.ToBase64String(file?.Content!);
        // string imageHtml = $"<img src=\"data:image/png;base64,{base64Image}\" alt=\"PT BOT FINANCE INDONESIA\" style=\"height: 50px;\" />";

        var data = new Dictionary<string, object?>
        {
          ["AgreementNo"] = dataAgreementMarketing.AgreementNo ?? "-",
          ["CompanyName"] = dataAgreementMarketing.CompanyName ?? "-",
          // ["CompanyLogo"] = base64Image,
          ["ClientName"] = dataAgreementMarketing.ClientName ?? "-",
          ["ApprovedDate"] = dataAgreementMarketing.ApprovedDate?.ToString("dd MMMM yyyy") ?? "-",
          ["DisbursementDate"] = dataAgreementMarketing.DisbursementDate?.ToString("dd MMMM yyyy") ?? "-",
          ["IncentivePeriod"] = dataAgreementMarketing.IncentivePeriod?.ToString() ?? "0",
          ["PaymentMethod"] = dataAgreementMarketing.PaymentMethod ?? "-",
          ["CurrencyCode"] = dataAgreementMarketing.CurrencyCode ?? "-",
          ["NetFinance"] = dataAgreementMarketing.NetFinance?.ToString("N2") ?? "0",
          ["InterestRate"] = dataAgreementMarketing.InterestRate?.ToString("N2") ?? "0",
          ["CostRate"] = dataAgreementMarketing.CostRate?.ToString("N2") ?? "0",
          ["InterestMargin"] = dataAgreementMarketing.InterestMargin?.ToString("N2") ?? "0",
          ["BPERatio"] = dataAgreementMarketing.BPERatio?.ToString("N2") ?? "0",
          ["TotalInsurancePremiAmount"] = dataAgreementMarketing.TotalInsurancePremiAmount?.ToString("N2") ?? "0",
          ["BPEEffect"] = dataAgreementMarketing.BPEEffect?.ToString("N2") ?? "0",
          ["BPEIncome"] = dataAgreementMarketing.BPEIncomeIncentiveExpense?.ToString("N2") ?? "0",
          ["NonInterestEffect"] = dataAgreementMarketing.NonInterestEffect?.ToString("N2") ?? "0",
          ["ProfitBeforeMarketingIncentive"] = dataAgreementMarketing.ProfitBeforeMarketingIncentive?.ToString("N2") ?? "0",
          ["MarketingIncentiveRatio"] = dataAgreementMarketing.MarketingIncentiveRatio?.ToString("N2") ?? "0",
          ["PrintDate"] = DateTime.Now.ToString("dddd, MMMM d, yyyy"),
          ["PrintTime"] = DateTime.Now.ToString("h:mm tt"),
          ["InsuranceRate"] = dataAgreementMarketing.InsuranceRate?.ToString("N2") ?? "0",
          ["InterestAmount"] = dataAgreementMarketing.InterestAmount?.ToString("N2") ?? "0",
          ["CostAmount"] = dataAgreementMarketing.CostAmount?.ToString("N2") ?? "0",
          ["InterestMarginAmount"] = dataAgreementMarketing.InterestMarginAmount?.ToString("N2") ?? "0",
          ["CCYRate"] = dataAgreementMarketing.CCYRate?.ToString("N2") ?? "0",
          ["BPETotal"] = dataAgreementMarketing.BPETotal?.ToString("N2") ?? "0",
          ["BPETotalAmount"] = dataAgreementMarketing.BPETotalAmount?.ToString("N2") ?? "0",
          ["InsPremUsageRatio"] = dataAgreementMarketing.InsurancePremiumUsageRatio?.ToString("N2") ?? "0",
          ["BPEIncomeExpense"] = dataAgreementMarketing.BPEIncomeIncentiveExpense?.ToString("N2") ?? "0",
          ["NonIntIncome"] = dataAgreementMarketing.NonInterestIncome?.ToString("N2") ?? "0",
          ["NonIntExp"] = dataAgreementMarketing.NonInterestExpense?.ToString("N2") ?? "0",
          ["NonIntEffRate"] = dataAgreementMarketing.NonInterestEffect?.ToString("N2") ?? "0",
          ["NonIntEffAmount"] = dataAgreementMarketing.NonInterestEffectAmount?.ToString("N2") ?? "0",
          ["TotalInterestMargin"] = dataAgreementMarketing.TotalInterestMargin?.ToString("N2") ?? "0",
          ["ProfBeforeMarketingIncentive"] = dataAgreementMarketing.ProfitBeforeMarketingIncentive?.ToString("N2") ?? "0",
          ["MarketingIncRatioInt"] = dataAgreementMarketing.MarketingIncentiveRatioInterest?.ToString("N2") ?? "0",
          ["MarIncRatioFin"] = dataAgreementMarketing.MarketingIncentiveRatioFinance?.ToString("N2") ?? "0",
          ["IncAmount"] = dataAgreementMarketing.MarketingIncentiveRatio?.ToString("N2") ?? "0",
          ["NetIntMarginRate"] = dataAgreementMarketing.InterestMargin?.ToString("N2") ?? "0",
          ["NetIntMarginAmount"] = dataAgreementMarketing.NetInterestMarginAfterCost?.ToString("N2") ?? "0",
          
          // ["FeeList"] = agreementFeeList?.Select((x, i) => new AgreementFeeList
          // {
          //   FeeNo = (i + 1),
          //   FeeName = x.FeeName,
          //   FeeAmount = x.FeeAmount,
          //   FeeRate = x.FeeRate
          // }).ToList(),

          // ["CommList"] = agreementCommList?.Select((x, i) => new AgreementCommissionList
          // {
          //   CommNo = (i + 1),
          //   CommName = x.CommName,
          //   CommAmount = x.CommAmount,
          //   CommRate = x.CommRate
          // }).ToList(),

          // ["ReffList"] = agreementReferrallist?.Select((x, i) => new AgreementReferralList
          // {
          //   ReferralNo = (i + 1),
          //   ReferralName = x.ReferralName,
          //   ReferralAmount = x.ReferralAmount,
          //   ReferralRate = x.ReferralRate
          // }).ToList(),
        };

        // Panggil client

        using var templateStream = File.OpenRead(templatePath);

        // 4. Panggil NanoDocClient
        var nanoResult = await _nanoDocClient.GenerateAsync(
            templateStream: templateStream,
            data: data,
            outputFormat: outputFormat
        );

        if (!nanoResult.IsSuccess)
        {
          throw new Exception($"Gagal generate dokumen: {nanoResult.ErrorMessage}");
        }

        // 5. Kembalikan Result
        return new FileDoc
        {
          Name = Path.GetFileNameWithoutExtension(templatePath),
          Content = nanoResult.Content,
          MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };
    }

    public async Task<FileDoc?> GetFile(string FileName)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {

        if (FileName == null) throw new Exception("File doesn't exist");
        var file = await SMBClient.DownloadFileAsync(FileName);

        transaction.Commit();
        return file;
      }
      catch (Exception ex)
      {
        transaction.Rollback();
        throw new Exception($"Error downloading file '{FileName}': {ex.Message}", ex);
      }
    }

    public async Task<int> ProcessSync(IDbTransaction transaction, InterfaceAgreementIncentiveMarketing interfaceModel)
    {
      {
        try
        {
          int result = 0;
          
          var incentiveMarketing = new IncentiveMarketing
          {
            ID = GUID(),
            CreDate = interfaceModel?.CreDate,
            CreBy = interfaceModel?.CreBy,
            CreIPAddress = interfaceModel?.CreIPAddress,
            ModDate = interfaceModel?.ModDate,
            ModBy = interfaceModel?.ModBy,
            ModIPAddress = interfaceModel?.ModIPAddress,
            ClientID = interfaceModel?.ClientID,
            ClientNo = interfaceModel?.ClientNo,
            ClientName = interfaceModel?.ClientName,
            IncentivePeriode = DateTime.Now.ToString("yyyyMM"),
            TotalIncentiveAmount = interfaceModel?.InterestMarginAmount
          };
          result += await _incentiveMarketingRepo.Insert(transaction, incentiveMarketing);

          var agreementIncentiveMarketing = new AgreementIncentiveMarketing
          {
            ID = GUID(),
            CreDate = interfaceModel?.CreDate,
            CreBy = interfaceModel?.CreBy,
            CreIPAddress = interfaceModel?.CreIPAddress,
            ModDate = interfaceModel?.ModDate,
            ModBy = interfaceModel?.ModBy,
            ModIPAddress = interfaceModel?.ModIPAddress,
            IncentiveMarketingID = incentiveMarketing.ID,
            ApplicationMainID = interfaceModel?.ApplicationMainID,
            AgreementNo = interfaceModel?.AgreementNo,
            ClientID = interfaceModel?.ClientID,
            ClientNo = interfaceModel?.ClientNo,
            ClientName = interfaceModel?.ClientName,
            ApprovedDate = interfaceModel?.ApprovedDate,
            DisbursementDate = interfaceModel?.DisbursementDate,
            IncentivePeriod = interfaceModel?.IncentivePeriod,
            PaymentMethod = interfaceModel?.PaymentMethod,
            CurrencyID = interfaceModel?.CurrencyID,
            CurrencyCode = interfaceModel?.CurrencyCode,
            CurrencyDesc = interfaceModel?.CurrencyDesc,
            NetFinance = interfaceModel?.NetFinance,
            InterestRate = interfaceModel?.InterestRate,
            CostRate = interfaceModel?.CostRate,
            InterestMargin = interfaceModel?.InterestMargin,
            InsuranceRate = interfaceModel?.InsuranceRate,
            InterestAmount = interfaceModel?.InterestAmount,
            CostAmount = interfaceModel?.CostAmount,
            InterestMarginAmount = interfaceModel?.InterestMarginAmount,
            TotalInsurancePremiAmount = interfaceModel?.TotalInsurancePremiAmount,
            CCYRate = interfaceModel?.CCYRate,
            CommissionRate = interfaceModel?.CommissionRate,
            VendorID = interfaceModel?.VendorID,
            VendorCode = interfaceModel?.VendorCode,
            VendorName = interfaceModel?.VendorName,
            AgentID = interfaceModel?.AgentID,
            AgentCode = interfaceModel?.AgentCode,
            AgentName = interfaceModel?.AgentName
          };

          result += await _repo.Insert(transaction, agreementIncentiveMarketing);

          foreach (var fee in interfaceModel?.AgreementFees!)
          {  
            var agreementFee = new AgreementFee
            {
              ID = GUID(),
              CreDate = interfaceModel?.CreDate,
              CreBy = interfaceModel?.CreBy,
              CreIPAddress = interfaceModel?.CreIPAddress,
              ModDate = interfaceModel?.ModDate,
              ModBy = interfaceModel?.ModBy,
              ModIPAddress = interfaceModel?.ModIPAddress,
              AgreementIncentiveID = agreementIncentiveMarketing.ID,
              FeeID = fee.FeeID,
              FeeCode = fee.FeeCode,
              FeeName = fee.FeeName,
              FeeAmount = fee.FeeAmount,
              FeePaymentType = fee.FeePaymentType,
              FeePaidAmount = fee.FeePaidAmount,
              FeeReduceDisburseAmount = fee.FeeReduceDisburseAmount,
              FeeCapitalizeAmount = fee.FeeCapitalizeAmount,
              InsuranceYear = fee.InsuranceYear,
              Remarks = fee.Remarks,
              FeeRate = fee.FeeRate,
              IsInternalIncome = fee.IsInternalIncome
            };

            result += await _agreementFeeRepo.Insert(transaction, agreementFee);
          }

          foreach (var refund in interfaceModel?.AgreementRefunds!)
          {
            var agreementRefund = new AgreementRefund
            {
              ID = GUID(),
              CreDate = interfaceModel?.CreDate,
              CreBy = interfaceModel?.CreBy,
              CreIPAddress = interfaceModel?.CreIPAddress,
              ModDate = interfaceModel?.ModDate,
              ModBy = interfaceModel?.ModBy,
              ModIPAddress = interfaceModel?.ModIPAddress,
              AgreementIncentiveID = agreementIncentiveMarketing.ID,
              RefundID = refund.RefundID,
              RefundCode = refund.RefundCode,
              RefundDesc = refund.RefundDesc,
              RefundAmount = refund.RefundAmount,
              RefundRate = refund.RefundRate,
              CalculateBy = refund.CalculateBy
            };

            result += await _agreementRefundRepo.Insert(transaction, agreementRefund);
          }

          return result;
        }
        catch (Exception)
        {
          
          throw;
        }

      }
    }
  }
}