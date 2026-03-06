
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;
using DotNetEnv;
using iFinancing360.Helper;
using System.Data;

namespace Service
{
  public class IncentiveSchemeService : BaseService, IIncentiveSchemeService
  {

    private readonly IIncentiveSchemeRepository _repo;
    private readonly IIncentiveSchemeExtRepository _repoExt;
    public IncentiveSchemeService(IIncentiveSchemeRepository repo, IIncentiveSchemeExtRepository repoExt)
    {
      _repo = repo;
      _repoExt = repoExt;
    }


    public async Task<IncentiveScheme> GetRowByID(string id)
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

    public async Task<List<IncentiveScheme>> GetRows(string? keyword, int offset, int limit)
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

    public async Task<int> Insert(IncentiveScheme model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
        try
        {
          List<string> ErrorList = new List<string>();

          if (model.EffDate < DateTime.Now) ErrorList.Add("Effective Date cannot be less than current date");

          if (model.IncentiveType == "COLLECTION")
          {
            if (model.OverdueDaysFrom < 0) ErrorList.Add("Overdue Days From cannot be less than 0");
            if (model.OverdueDaysTo < 0) ErrorList.Add("Overdue Days To cannot be less than 0");
            if (model.MinimumAmount < 0) ErrorList.Add("Minimum Amount cannot be less than 0");
            if (model.MaximumAmount < 0) ErrorList.Add("Maximum Amount cannot be less than 0");
          }
          else
          {
            if (model.IncentiveRatio < 0) ErrorList.Add("Incentive Ratio cannot be less than 0");
            if (model.RatePenalty < 0) ErrorList.Add("Rate Penalty cannot be less than 0");
          }

          if (ErrorList.Count > 0) throw new Exception(string.Join(";\n ", ErrorList));

          int result = await _repo.Insert(transaction, model);
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
    }

    public async Task<int> UpdateByID(IncentiveScheme model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      {
          try
          {
            List<string> ErrorList = new List<string>();

            if (model.EffDate < DateTime.Now) ErrorList.Add("Effective Date cannot be less than current date");

            if (model.IncentiveType == "MARKETING")
            {
              model.OverdueDaysFrom = null;
              model.OverdueDaysTo = null;
              model.MinimumAmount = null;
              model.MaximumAmount = null;

              if (model.IncentiveRatio < 0) ErrorList.Add("Incentive Ratio cannot be less than 0");
              if (model.RatePenalty < 0) ErrorList.Add("Rate Penalty cannot be less than 0");
            }
            else
            {
              model.IncentiveRatio = null;
              model.RatePenalty = null;

              if (model.OverdueDaysFrom < 0) ErrorList.Add("Overdue Days From cannot be less than 0");
              if (model.OverdueDaysTo < 0) ErrorList.Add("Overdue Days To cannot be less than 0");
              if (model.MinimumAmount < 0) ErrorList.Add("Minimum Amount cannot be less than 0");
              if (model.MaximumAmount < 0) ErrorList.Add("Maximum Amount cannot be less than 0");
            }
          if (ErrorList.Count > 0) throw new Exception(string.Join(";\n ", ErrorList));

          int result = await _repo.UpdateByID(transaction, model);
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
    }

    public async Task<int> ChangeStatus(IncentiveScheme model)
		{
			using var connection = _repo.GetDbConnection();
			using var transaction = connection.BeginTransaction();

			try
			{
				int result = await _repo.ChangeStatus(transaction, model);
				transaction.Commit();
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}

		}

    
private async Task<int> InsertExt(IDbTransaction transaction, dynamic tempProperties, IncentiveScheme model)
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


    public async Task<List<ExtendModel>> GetRowForParent(string ID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {

        IncentiveScheme model = await _repo.GetRowByID(transaction, ID);
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
  }
}