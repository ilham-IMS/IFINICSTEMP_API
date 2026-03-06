
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;

namespace Service
{
  public class JournalGlLinkService : BaseService, IJournalGlLinkService
  {
    private readonly IJournalGlLinkRepository _repo;
    public JournalGlLinkService(IJournalGlLinkRepository repo)
    {
      _repo = repo;
    }

    public async Task<List<JournalGlLink>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<JournalGlLink> GetRowByID(string id)
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
    public async Task<int> Insert(JournalGlLink model)
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

    public async Task<int> UpdateByID(JournalGlLink model)
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
          int countResult = 0;
          foreach (string id in idList)
          {
            var result = await _repo.DeleteByID(transaction, id);
            if (result > 0)
            {
              countResult += result;
            }
          }
          transaction.Commit();

          return countResult;
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }

      }
    }
    public async Task<int> ChangeIsActive(JournalGlLink model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int result = await _repo.ChangeIsActive(transaction, model);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }

    }
    public async Task<FileDoc> GetReportData()
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                string title = "Journal Gl Link Report List";
                List<string> headers = new() { "Code" , "GlLinkName" , "IsActive"}; // Header disesuaikan dengan jumalh kolom yang ditampilkan dan menggunakan PascalCase

                var reportData = await _repo.GetReportData(transaction);
                // Jika data pada database ingin dirubah misalnya 1 menjadi Yes dapat menggunakan cara mapping seperti dibawah ini
                var formattedData = reportData.Select(tempData => new
                {
                  Code = tempData.Code,
                  GlLinkName = tempData.GlLinkName,
                  IsActive = tempData.IsActive == 1 ? "Yes" : "No"
                }).ToList();

                MemoryStream ms = new MemoryStream();
                ms = await GenerateExcelTemplateWithData(title, headers, formattedData);

                transaction.Commit();
                return new FileDoc
                {
                  Content = ms.ToArray(),
                  Name = $"journal_gl_link_report_{Static.FileTimeStamp}.xlsx",
                  MimeType = FileMimeType.Xlsx
                };

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
  }
}