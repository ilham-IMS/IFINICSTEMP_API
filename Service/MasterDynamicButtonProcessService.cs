using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class MasterDynamicButtonProcessService(
    IMasterDynamicButtonProcessRepository repo,
    IMasterDynamicButtonProcessExtRepository repoExt
    ) : BaseService, IMasterDynamicButtonProcessService
  {
    private readonly IMasterDynamicButtonProcessRepository _repo = repo;
    private readonly IMasterDynamicButtonProcessExtRepository _repoExt = repoExt;

    #region GetRows
    public async Task<List<MasterDynamicButtonProcess>> GetRows(string? keyword, int offset, int limit)
    {

      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        List<MasterDynamicButtonProcess> result = await _repo.GetRows(transaction, keyword, offset, limit);

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
    public async Task<MasterDynamicButtonProcess> GetRowByID(string id)
    {

      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        MasterDynamicButtonProcess result = await _repo.GetRowByID(transaction, id);

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
    public async Task<int> Insert(MasterDynamicButtonProcess model)
    {

      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        List<string> errors = []; // Tampungan Error

        var existByShortDesc = await _repo.CountByShortDesc(transaction, model.ShortDescription!, model.ID!);

        if (existByShortDesc > 0) errors.Add($"Short Description {model.ShortDescription} already exist");

        var existByDescription = await _repo.CountByDescription(transaction, model.Description!, model.ID!);

        model.Description = model.Description?.ToUpper();
        if (existByDescription > 0) errors.Add($"Description {model.Description} already exist");

        model.DllName = model.DllName + ";";
        model.NamespaceName = model.NamespaceName + ".";
        model.ClassName = model.ClassName + ";";

        if (errors.Count > 0) throw new Exception(string.Join(";\n", errors));  // Jika ada error, throw exception semua error
        int result = await _repo.Insert(transaction, model);
        // Insert Dynamic Form data
        var tempProperties = model.Properties;

        model.Properties = null;

        await InsertExt(transaction, tempProperties, model);

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
    public async Task<int> UpdateByID(MasterDynamicButtonProcess model)
    {

      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {

        List<string> errors = []; // Tampungan Error

        var existByShortDesc = await _repo.CountByShortDesc(transaction, model.ShortDescription!, model.ID!);

        if (existByShortDesc > 0) errors.Add($"Short Description {model.ShortDescription} already exist");

        var existByDescription = await _repo.CountByDescription(transaction, model.Description!, model.ID!);

        model.Description = model.Description?.ToUpper();
        if (existByDescription > 0) errors.Add($"Description {model.Description} already exist");


        if (errors.Count > 0) throw new Exception(string.Join(";\n", errors));  // Jika ada error, throw exception semua error

        int result = await _repo.UpdateByID(transaction, model);
        // Update Dynamic Form data
        var extProperties = await _repoExt.GetRowForParent(transaction, model.ID!);

        var tempProperties = model.Properties;
        model.Properties = null;

        await UpdateExt(transaction, tempProperties, extProperties, model);

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

    public async Task<List<MasterDynamicButtonProcess>> GetRowsForLookup(string? keyword, int offset, int limit)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        List<MasterDynamicButtonProcess> results = await _repo.GetRowsForLookup(transaction, keyword, offset, limit);
        foreach (var result in results)
        {
          result.MethodFullName = result.DllName + result.NamespaceName + result.ClassName + result.MethodName;
        }

        transaction.Commit();
        return results;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region IsDLLExist
    public async Task<bool> IsDLLExist(string assemblyName)
    {
      return IsDllExist(assemblyName);
    }
    #endregion

    #region InsertExt
    private async Task<int> InsertExt(IDbTransaction transaction, dynamic tempProperties, MasterDynamicButtonProcess model)
    {
      var properties = await DeserializeProperties(tempProperties);

      if (properties == null) return 0;

      int resultExt = 0;

      foreach (var property in properties)
      {
        var extendModel = new ExtendModel
        {
          ID = Guid.NewGuid().ToString("N").ToLower(),
          CreDate = model.CreDate,
          CreBy = model.CreBy,
          CreIPAddress = model.CreIPAddress,
          ModDate = model.ModDate,
          ModBy = model.ModBy,
          ModIPAddress = model.ModIPAddress,
          ParentID = model.ID,
          Keyy = property.Key,
          Value = property.Value,
          Properties = null
        };

        resultExt += await _repoExt.Insert(transaction, extendModel);
      }

      return resultExt;
    }
    #endregion

    #region UpdateExt
    private async Task<int> UpdateExt<T>(IDbTransaction transaction, dynamic tempProperties, List<ExtendModel> extProperties, T model) where T : ExtendModel
    {
      int result = 0;

      var properties = await DeserializeProperties(tempProperties);

      foreach (var property in properties)
      {
        var keyy = property.Key;
        var value = property.Value;

        if (value is not string)
        {
          value = value.ToString();
        }

        if (extProperties.Any(e => e.Keyy == keyy))
        {
          var extendModel = new ExtendModel
          {
            ParentID = model.ID,
            ModDate = model.ModDate,
            ModBy = model.ModBy,
            ModIPAddress = model.ModIPAddress,
            Keyy = keyy,
            Value = value,
            Properties = null
          };

          result += await _repoExt.UpdateByID(transaction, extendModel);
        }
        else
        {
          var extendModel = new ExtendModel
          {
            ID = Guid.NewGuid().ToString("N").ToLower(),
            CreDate = model.CreDate,
            CreBy = model.CreBy,
            CreIPAddress = model.CreIPAddress,
            ModDate = model.ModDate,
            ModBy = model.ModBy,
            ModIPAddress = model.ModIPAddress,
            ParentID = model.ID,
            Keyy = keyy,
            Value = value,
            Properties = null
          };

          result += await _repoExt.Insert(transaction, extendModel);
        }
      }

      return result;
    }
    #endregion

    #region GetRowForParent
    public async Task<List<ExtendModel>> GetRowForParent(string ParentID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        // khusus client personal/corporate info
        // karena yg dikirim adalah client id, maka perlu kita ambil id terlebih dahulu
        MasterDynamicButtonProcess model = await _repo.GetRowByID(transaction, ParentID);

        var result = await _repoExt.GetRowForParent(transaction, model.ID);

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
  }
}
