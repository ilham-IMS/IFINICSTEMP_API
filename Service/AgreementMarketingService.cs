
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;
using iFinancing360.Service.Helper.SMBClient;
using FastReport.AdvMatrix;

namespace Service
{
  public class AgreementMarketingService : BaseService, IAgreementMarketingService
  {

    private readonly IAgreementMarketingRepository _repo;
    private readonly NanoDocClient _nanoDocClient;
    public AgreementMarketingService(IAgreementMarketingRepository repo, NanoDocClient nanoDocClient)
    {
      _repo = repo;
      _nanoDocClient = nanoDocClient;
    }


    public async Task<AgreementMarketing> GetRowByID(string id)
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

    public async Task<List<AgreementMarketing>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<int> Insert(AgreementMarketing model)
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

    public async Task<int> UpdateByID(AgreementMarketing model)
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

    public async Task<FileDoc> GetPreview(AgreementMarketing dataAgreementMarketing, string ID, List<AgreementFeeList> agreementFeeList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        FileDoc fileDoc = await GenerateDocumentFromTemplate(transaction,"docx", ID, dataAgreementMarketing, agreementFeeList);

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

    private async Task<FileDoc> GenerateDocumentFromTemplate(IDbTransaction transaction, string outputFormat, string ID, AgreementMarketing dataAgreementMarketing, List<AgreementFeeList> agreementFeeList)
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
          ["InsurancePremiumUsageRatio"] = dataAgreementMarketing.InsurancePremiumUsageRatio?.ToString("N2") ?? "0",
          ["BPEEffect"] = dataAgreementMarketing.BPEEffect?.ToString("N2") ?? "0",
          ["BPEIncome"] = dataAgreementMarketing.BPEIncome?.ToString("N2") ?? "0",
          ["IncentiveExpense"] = dataAgreementMarketing.IncentiveExpense?.ToString("N2") ?? "0",
          ["NonInterestEffect"] = dataAgreementMarketing.NonInterestEffect?.ToString("N2") ?? "0",
          ["ProfitBeforeMarketingIncentive"] = dataAgreementMarketing.ProfitBeforeMarketingIncentive?.ToString("N2") ?? "0",
          ["IncentiveAmount"] = dataAgreementMarketing.IncentiveAmount?.ToString("N2") ?? "0",
          ["MarketingIncentiveRatio"] = dataAgreementMarketing.MarketingIncentiveRatio?.ToString("N2") ?? "0",
          ["FinanceAmount"] = dataAgreementMarketing.FinanceAmount?.ToString("N2") ?? "0",
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
          ["NonIntName"] = dataAgreementMarketing.NonInterestName ?? "-",
          ["NonIntExp"] = dataAgreementMarketing.NonInterestExpense?.ToString("N2") ?? "0",
          ["NonIntEffRate"] = dataAgreementMarketing.NonInterestEffect?.ToString("N2") ?? "0",
          ["NonIntEffAmount"] = dataAgreementMarketing.NonInterestEffectAmount?.ToString("N2") ?? "0",
          ["ProfBeforeMarketingIncentive"] = dataAgreementMarketing.ProfitBeforeMarketingIncentive?.ToString("N2") ?? "0",
          ["MarIncRatio"] = dataAgreementMarketing.MarketingIncentiveRatio?.ToString("N2") ?? "0",
          ["IncAmount"] = dataAgreementMarketing.IncentiveAmount?.ToString("N2") ?? "0",
          ["FinanceAmount"] = dataAgreementMarketing.FinanceAmount?.ToString("N2") ?? "0",
          ["NetIntMarginRate"] = dataAgreementMarketing.InterestMargin?.ToString("N2") ?? "0",
          ["NetIntMarginAmount"] = dataAgreementMarketing.NetInterestMarginAfterCost?.ToString("N2") ?? "0",
          
          ["FeeList"] = agreementFeeList?.Select((x, i) => new AgreementFeeList
          {
            FeeNo = (i + 1),
            FeeName = x.FeeName,
            FeeAmount = x.FeeAmount,
            FeeRate = x.FeeRate
          }).ToList(),
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

    #region GenerateDocumentAllTypeDoc
    public async Task<FileDoc> GenerateDocumentAllTypeDoc(string mimeType, string ID, AgreementMarketing dataAgreementMarketing, List<AgreementFeeList> agreementFeeList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {

        FileDoc fileDoc = await GenerateDocumentFromTemplate(transaction, mimeType, ID, dataAgreementMarketing, agreementFeeList);

        

        string docName = "INCENTIVE_AGREEMENT";

        transaction.Commit();
        FileDoc result = new()
        {
          Content = fileDoc.Content,
          Name = $"{docName}.{mimeType.ToLower()}",
          MimeType = fileDoc.MimeType,
        };
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion
  }
}