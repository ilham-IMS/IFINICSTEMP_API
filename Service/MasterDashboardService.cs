using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;

namespace Service
{
  public class MasterDashboardService : BaseService, IMasterDashboardService
  {
    private readonly IMasterDashboardRepository _repo;

    public MasterDashboardService(IMasterDashboardRepository repo)
    {
      _repo = repo;
    }

    #region GetRows
    public async Task<List<MasterDashboard>> GetRows(string? keyword, int offset, int limit)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
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
    #endregion

    #region GetRowsForLookupExcludeByID
    public async Task<List<MasterDashboard>> GetRowsForLookupExcludeByID(string? keyword, int offset, int limit, string[] id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        List<MasterDashboard> result = await _repo.GetRowsForLookupExcludeByID(transaction, keyword, offset, limit, id);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }

    }
    #endregion

    #region GetRowByID
    public async Task<MasterDashboard> GetRowByID(string id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        MasterDashboard result = await _repo.GetRowByID(transaction, id);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }

    }
    #endregion

    #region Insert
    public async Task<int> Insert(MasterDashboard model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
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
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(MasterDashboard model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
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
    #endregion

    #region DeleteByID
    public async Task<int> DeleteByID(string[] ids)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int result = 0;
        foreach (string id in ids)
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
    #endregion

    #region ChangeEditableStatus
    public async Task<int> ChangeEditableStatus(MasterDashboard model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int result = await _repo.ChangeEditableStatus(transaction, model);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }

    }
    #endregion

    public async Task<FileDoc> GetReportData()
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                string title = "Master Dashboard List";
                List<string> headers = new() { "Code" , "DashboardName","DashboardType", "DashboardGrid", "IsActive"}; // Header disesuaikan dengan jumalh kolom yang ditampilkan dan menggunakan PascalCase

                var reportData = await _repo.GetReportData(transaction);
                // Jika data pada database ingin dirubah misalnya 1 menjadi Yes dapat menggunakan cara mapping seperti dibawah ini
                var formattedData = reportData.Select(tempData => new
                {
                  Code = tempData.Code,
                  DashboardName = tempData.DashboardName,
                  DashboardType = tempData.DashboardType,
                  DashboardGrid = tempData.DashboardGrid,
                  IsActive = tempData.IsActive == 1 ? "Yes" : "No"
                }).ToList();

                MemoryStream ms = new MemoryStream();
                ms = await GenerateExcelTemplateWithData(title, headers, formattedData);

                transaction.Commit();
                return new FileDoc
                {
                  Content = ms.ToArray(),
                  Name = $"master_dashboard_{Static.FileTimeStamp}.xlsx",
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
