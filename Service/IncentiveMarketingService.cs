
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;
using System.Globalization;
using iFinancing360.Service.Helper.Report;
using iFinancing360.Service.Helper.SMBClient;

namespace Service
{
  public class IncentiveMarketingService : BaseService, IIncentiveMarketingService
  {

    private readonly IIncentiveMarketingRepository _repo;
    private readonly NanoDocClient _nanoDocClient;
    private readonly GlobalConfig _globalConfig;
    private readonly IAgreementMarketingRepository _repoAgreementMarketing;

    public IncentiveMarketingService(IIncentiveMarketingRepository repo, NanoDocClient nanoDocClient, GlobalConfig globalConfig, IAgreementMarketingRepository repoAgreementMarketing)
    {
      _repo = repo;
      _nanoDocClient = nanoDocClient;
      _globalConfig = globalConfig;
      _repoAgreementMarketing = repoAgreementMarketing;
    }


    public async Task<IncentiveMarketing> GetRowByID(string id)
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

    public async Task<List<IncentiveMarketing>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<List<IncentiveMarketing>> GetRows(string? keyword, int offset, int limit, string PeriodeFrom, string PeriodeTo)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          var result = await _repo.GetRows(transaction, keyword, offset, limit, PeriodeFrom, PeriodeTo);
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

    public async Task<int> Insert(IncentiveMarketing model)
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

    public async Task<int> UpdateByID(IncentiveMarketing model)
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

    #region GetHTMLPreview
    public async Task<string> GetHTMLPreview(string id, IncentiveMarketing incentiveMarketingData, List<AgreementIncentiveMarketing> dataAgreement)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        string envPath = Env.GetString("REPORT_TEMPLATE_PATH");
        string templatePath = Path.Combine(envPath, "IncentiveMarketingTemplate.html");
        string htmlContent = await File.ReadAllTextAsync(templatePath);

        DateTime systemDate = _globalConfig.SystemDateTime ?? DateTime.Now;

        var file = await GetFile(incentiveMarketingData.CompanyFileName!);

        // Convert file ke base64 untuk digunakan sebagai inline image
        string base64Image = Convert.ToBase64String(file?.Content!);
        string imageHtml = $"<img src=\"data:image/png;base64,{base64Image}\" alt=\"PT BOT FINANCE INDONESIA\" style=\"height: 50px;\" />";

        // Tambahan: Setup parameter untuk header
        var parameters = new Dictionary<string, string>
            {
                { "ReportTitle", "Incentive Calculation Marketing" },
                { "PrintDate", systemDate.ToString("dddd, MMMM d yyyy", CultureInfo.InvariantCulture) },
                { "PrintTime", systemDate.ToString("hh:mm tt", CultureInfo.InvariantCulture) + " (GMT +7)" },
                { "CompanyName", incentiveMarketingData.CompanyName ?? "-" },
                { "ImageLogo", imageHtml },
                { "PeriodeFrom", incentiveMarketingData.IncentivePeriode ?? "-" },
                { "PeriodeTo", incentiveMarketingData.IncentivePeriode ?? "-" }
            };

        foreach (var parameter in parameters)
        {
          string placeholder = $"{{{{{parameter.Key}}}}}";
          htmlContent = htmlContent.Replace(placeholder, parameter.Value);
        }

        string tableRows = string.Empty;
        if (dataAgreement.Count == 0)
        {
          tableRows = @"<tr>
                        <td colspan=""18"" style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell; text-align: center;"">No data available for the selected period.</td>
                    </tr>";
        } else
        {
          foreach (var item in dataAgreement)
          {
            tableRows += $@"<tr>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.AgreementNo ?? "-"} / {item.ClientName ?? "-"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.ApprovedDate?.ToString("dd-MMM-yyyy") ?? "-"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.DisbursementDate?.ToString("dd-MMM-yyyy") ?? "-"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.IncentivePeriod.ToString() ?? "-"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.PaymentMethod ?? "-"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.CurrencyCode ?? "-"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.NetFinance ?? 0}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.InterestRate ?? 0}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.CostRate ?? 0}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.InterestMargin ?? 0}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{(item.BPERatio * 100)?.ToString("N2") ?? "0.00"}%</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{(item.InsurancePremiumUsageRatio * 100)?.ToString("N2") ?? "0.00"}%</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{(item.BPEEffect * 100)?.ToString("N2") ?? "0.00"}%</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.BPEIncomeIncentiveExpense?.ToString("N2") ?? "0.00"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.NonInterestEffect?.ToString("N2") ?? "0.00"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.ProfitBeforeMarketingIncentive?.ToString("N2") ?? "0.00"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.MarketingIncentiveRatio?.ToString("N2") ?? "0.00"}</td>
                          <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.NetInterestMarginAfterCost?.ToString("N2") ?? "0.00"}</td>
                      </tr>";
          }
        }

        htmlContent = htmlContent.Replace("{{table_rows}}", tableRows);

        transaction.Commit();

        return htmlContent;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region PrintDocument
    public async Task<FileDoc> PrintDocument(string mimeType, string id, IncentiveMarketing incentiveMarketingData, List<AgreementIncentiveMarketing> dataAgreement)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        string envPath = Env.GetString("REPORT_TEMPLATE_PATH");
        string templatePath = Path.Combine(envPath, "IncentiveMarketingTemplate.html");
        string htmlContent = await File.ReadAllTextAsync(templatePath);

        DateTime systemDate = _globalConfig.SystemDateTime ?? DateTime.Now;

        // Tambahan: Setup parameter untuk header
        var parameters = new Dictionary<string, string>
            {
                { "ReportTitle", "Incentive Calculation Marketing" },
                { "PrintDate", systemDate.ToString("dddd, MMMM d yyyy", CultureInfo.InvariantCulture) },
                { "PrintTime", systemDate.ToString("hh:mm tt", CultureInfo.InvariantCulture) + " (GMT +7)" },
                { "CompanyName", incentiveMarketingData.CompanyName ?? "-" },
                { "PeriodeFrom", incentiveMarketingData.IncentivePeriode ?? "-" },
                { "PeriodeTo", incentiveMarketingData.IncentivePeriode ?? "-" }
            };

        foreach (var parameter in parameters)
        {
          string placeholder = $"{{{{{parameter.Key}}}}}";
          htmlContent = htmlContent.Replace(placeholder, parameter.Value);
        }

        string tableRows = string.Empty;
        foreach (var item in dataAgreement)
        {
          tableRows += $@"<tr>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.AgreementNo ?? "-"} / {item.ClientName ?? "-"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.ApprovedDate?.ToString("dd-MMM-yyyy") ?? "-"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.DisbursementDate?.ToString("dd-MMM-yyyy") ?? "-"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.IncentivePeriod.ToString() ?? "-"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.PaymentMethod ?? "-"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.CurrencyCode ?? "-"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.NetFinance ?? 0}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.InterestRate ?? 0}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.CostRate ?? 0}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.InterestMargin ?? 0}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{(item.BPERatio * 100)?.ToString("N2") ?? "0.00"}%</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{(item.InsurancePremiumUsageRatio * 100)?.ToString("N2") ?? "0.00"}%</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{(item.BPEEffect * 100)?.ToString("N2") ?? "0.00"}%</td>
                       <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.BPEIncomeIncentiveExpense?.ToString("N2") ?? "0.00"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.NonInterestEffect?.ToString("N2") ?? "0.00"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.ProfitBeforeMarketingIncentive?.ToString("N2") ?? "0.00"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.MarketingIncentiveRatio?.ToString("N2") ?? "0.00"}</td>
                        <td style=""padding: 8px; border: 1px solid #000000; color: #333; display: table-cell;"">{item.NetInterestMarginAfterCost?.ToString("N2") ?? "0.00"}</td>
                    </tr>";
        }

        htmlContent = htmlContent.Replace("{{table_rows}}", tableRows);

        MemoryStream memoryStream = new();
        string mimeTypetoReturn = string.Empty;

        if (mimeType == null) throw new ArgumentNullException("Mime Type is null");

        if (mimeType == "Docx")
        {
          memoryStream = Report.ConvertHtmlToDocx(htmlContent);
          mimeTypetoReturn = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        }
        if (mimeType == "Xlsx")
        {
          memoryStream = Report.ConvertHTMLtoExcel(htmlContent);
          mimeTypetoReturn = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
        if (mimeType == "PDF")
        {
          mimeTypetoReturn = "application/pdf";
          memoryStream = Report.ConvertHtmlToDocx(htmlContent);

          // Konversi memoryStream (DOCX) ke byte array
          byte[] docxBytes;
          using (var ms = new MemoryStream())
          {
            await memoryStream.CopyToAsync(ms);
            docxBytes = ms.ToArray();
          }

          // Convert DOCX ke PDF
          byte[] pdfBytes = await ConvertDocxToPDF(docxBytes);
          memoryStream = new MemoryStream(pdfBytes);
        }

        FileDoc fileDoc = new()
        {
          Content = memoryStream.ToArray(),
          Name = $"INQUIRY_CLIENT_AGREEMENT.{mimeType}",
          MimeType = mimeTypetoReturn,
        };

        transaction.Commit();
        return fileDoc;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

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
  }
}