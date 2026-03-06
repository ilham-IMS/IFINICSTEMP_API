
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;

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

    public async Task<FileDoc> GetPreview(AgreementMarketing dataAgreementMarketing, string ID)
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

    private async Task<FileDoc> GenerateDocumentFromTemplate(IDbTransaction transaction, string outputFormat, string ID, AgreementMarketing dataAgreementMarketing)
    {

        
        string basePath = Env.GetString("REPORT_TEMPLATE_PATH");
        string templatePath = Path.Combine(basePath, "AgreementTemplate.docx");
       
        if (!File.Exists(templatePath))
        {
          throw new FileNotFoundException($"Template tidak ditemukan di path: {templatePath}", templatePath);
        }

        var data = new Dictionary<string, object?>
        {
          ["AgreementNo"] = dataAgreementMarketing.AgreementNo ?? "-",
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
          

          
          // ["ListTunggakan"] = dataWLDelivery.ListTunggakan?.Select((x, i) => new ListTunggakan
          // {
          //   AgreementNo = x.AgreementNo,
          //   DueDate = x.DueDate?.ToString("dd MMMM yyyy"),
          //   OvdDays = x.OvdDays,
          //   Pokok = x.PorsiPokok?.ToString("N2") ?? "0",          // String, bukan decimal.Parse
          //   Bunga = x.Bunga?.ToString("N2") ?? "0",
          //   InstallmentAmount = x.InstallmentAmount?.ToString("N2") ?? "0",
          //   Denda = x.Denda?.ToString("N2") ?? "0",
          //   BarangTunggakan = dataWLDelivery.Barang ?? "-",
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

    #region GenerateDocumentAllTypeDoc
    public async Task<FileDoc> GenerateDocumentAllTypeDoc(string mimeType, string ID, AgreementMarketing dataAgreementMarketing)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {

        FileDoc fileDoc = await GenerateDocumentFromTemplate(transaction, mimeType, ID, dataAgreementMarketing);

        

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