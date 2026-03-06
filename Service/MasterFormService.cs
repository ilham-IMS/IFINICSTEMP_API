using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class MasterFormService : BaseService, IMasterFormService
  {
    private readonly IMasterFormRepository _repo;
    private readonly IFormControlsRepository _repoFormControls;

    public MasterFormService(IMasterFormRepository repo, IFormControlsRepository repoFormControls)
    {
      _repo = repo;
      _repoFormControls = repoFormControls;
    }

    public async Task<List<MasterForm>> GetRows(string? keyword, int offset, int limit)
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
      finally
      {
        connection.Close();
      }
    }

    public async Task<MasterForm> GetRowByID(string ID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        //await _repo.LockRow(transaction, "master_Form", ID);
        var result = await _repo.GetRowByID(transaction, ID);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<MasterForm> GetRowByCode(string code)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        //await _repo.LockRow(transaction, "master_Form", ID);
        var result = await _repo.GetRowByCode(transaction, code);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<int> Insert(MasterForm model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        List<string> errors = []; // Tampungan Error

        var existByCode = await _repo.CountByCode(transaction, model.Code!);

        if (existByCode > 0) errors.Add($"Code {model.Code} already exist");

        var existByName = await _repo.CountByName(transaction, model.Name!, model.ID);

        if (existByName > 0) errors.Add($"Name {model.Name} already exist");

        // var countLabel = await _repo.CountByLabel(transaction, model.Label, model.ID);
        // if (countLabel > 0) errors.Add($"Label {model.Label} already exist");


        if (errors.Count > 0) throw new Exception(string.Join(";\n", errors));  // Jika ada error, throw exception semua error
        var result = await _repo.Insert(transaction, model);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<int> UpdateByID(MasterForm model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        List<string> errors = []; // Tampungan Error

        // var countLabel = await _repo.CountByLabel(transaction, model.Label, model.ID);
        // if (countLabel > 0) errors.Add($"Label {model.Label} already exist");
        int result = 0;
        if (errors.Count > 0) throw new Exception(string.Join(";\n", errors));  // Jika ada error, throw exception semua error

        result += await _repo.UpdateByID(transaction, model);
        if (model.IsActive == -1)
        {
          FormControls formControls = new()
          {
            MasterFormID = model.ID,
            ModDate = model.ModDate,
            ModBy = model.ModBy,
            ModIPAddress = model.ModIPAddress
          };
          result += await _repoFormControls.ChangeStatusNonActive(transaction, formControls);
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

    public async Task<int> DeleteByID(string[] listID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int count = 0;
        foreach (var ID in listID)
        {
          var result = await _repo.DeleteByID(transaction, ID);
          if (result > 0)
            count += result;

        }
        transaction.Commit();
        return count;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<int> ChangeStatus(MasterForm model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.ChangeStatus(transaction, model);
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
}
