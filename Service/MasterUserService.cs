
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;

namespace Service
{
  public class MasterUserService : BaseService, IMasterUserService
  {

    private readonly IMasterUserRepository _repo;
    public MasterUserService(IMasterUserRepository repo)
    {
      _repo = repo;
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

    public async Task<MasterUser> GetRowByID(string id)
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

    public async Task<List<MasterUser>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<int> Insert(MasterUser model)
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

    public async Task<int> UpdateByID(MasterUser model)
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
    public async Task<FileDoc> GetReportData()
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                string title = "Master User List";
                List<string> headers = new() { "EmployeeCode" , "EmployeeName"}; // Header disesuaikan dengan jumalh kolom yang ditampilkan dan menggunakan PascalCase

                var reportData = await _repo.GetReportData(transaction);
                // Jika data pada database ingin dirubah misalnya 1 menjadi Yes dapat menggunakan cara mapping seperti dibawah ini
                var formattedData = reportData.Select(tempData => new
                {
                  EmployeeCode = tempData.EmployeeCode,
                  EmployeeName = tempData.EmployeeName,
                }).ToList();

                MemoryStream ms = new MemoryStream();
                ms = await GenerateExcelTemplateWithData(title, headers, formattedData);

                transaction.Commit();
                return new FileDoc
                {
                  Content = ms.ToArray(),
                  Name = $"master_user_{Static.FileTimeStamp}.xlsx",
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