using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
  public class MasterApprovalService(IMasterApprovalRepository repo, IMasterApprovalCriteriaRepository repoMasterApprovalCriteria) : BaseService, IMasterApprovalService
  {
    private readonly IMasterApprovalRepository _repo = repo;
    private readonly IMasterApprovalCriteriaRepository _repoMasterApprovalCriteria = repoMasterApprovalCriteria;

    public async Task<List<MasterApproval>> GetRows(string? keyword, int offset, int limit)
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
    public async Task<MasterApproval> GetRowByID(string id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
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
    public async Task<int> Insert(MasterApproval model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      //untuk cek apakah ada code yang sama
      var CodeCheck = await _repo.CheckCode(transaction, model.Code);
      if (CodeCheck.CountCode > 0)
      {
        throw new Exception("Code already exist.");
      }

      //untuk cek apakah ada approval name yang sama

      var NameCheck = await _repo.CheckName(transaction, model.ApprovalName);
      if (NameCheck.CountName > 0)
      {
        throw new Exception("Approval name already exist.");
      }

      try
      {
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
    public async Task<int> UpdateByID(MasterApproval model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      //untuk cek apakah ada approval name yang sama
      var NameCheck = await _repo.CheckNameForUpdate(transaction, model.ApprovalName, model.ID);
      if (NameCheck.CountNameForUpdate > 0)
      {
        throw new Exception("Approval name already exist.");
      }

      try
      {
        var result = await _repo.UpdateByID(transaction, model);
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
    public async Task<int> ChangeIsActive(MasterApproval model)
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

    #region ProcessSync
    public async Task<int> ProcessSync(IDbTransaction transaction, InterfaceMasterApproval model)
    {
      try
      {
        int result = 0;

        var masterApv = await _repo.CountData(transaction, model?.ApprovalCategoryID!);

        string? approvalID = masterApv?.ID ?? null;

        if (masterApv == null)
        {
          var masterApproval = new MasterApproval()
          {
            ID = GUID(),
            CreDate = model?.CreDate,
            CreBy = model?.CreBy,
            CreIPAddress = model?.CreIPAddress,
            ModDate = model?.ModDate,
            ModBy = model?.ModBy,
            ModIPAddress = model?.ModIPAddress,
            Code = model?.ApprovalCategoryCode,
            ApprovalName = model?.ApprovalCategoryName,
            ReffApprovalCategoryID = model?.ApprovalCategoryID,
            ReffApprovalCategoryCode = model?.ApprovalCategoryCode,
            ReffApprovalCategoryName = model?.ApprovalCategoryName,
            IsActive = 1
          };

          result += await _repo.Insert(transaction, masterApproval);

          approvalID = masterApproval.ID; // Ubah ID untuk insert detail
        }

        result += await _repoMasterApprovalCriteria.DeleteAllByApprovalID(transaction, masterApv?.ID!);

        foreach (var criteria in model?.Criterias!)
        {
          var approvalCriteria = new MasterApprovalCriteria()
          {
            ID = GUID(),
            CreDate = model?.CreDate,
            CreBy = model?.CreBy,
            CreIPAddress = model?.CreIPAddress,
            ModDate = model?.ModDate,
            ModBy = model?.ModBy,
            ModIPAddress = model?.ModIPAddress,
            ApprovalID = approvalID,
            ReffCriteriaID = criteria.CriteriaID,
            ReffCriteriaCode = criteria.CriteriaCode,
            ReffCriteriaName = criteria.CriteriaDescription,
            CriteriaID = criteria.CriteriaID,
            CriteriaCode = criteria.CriteriaCode,
            CriteriaDescription = criteria.CriteriaDescription,
          };

          result += await _repoMasterApprovalCriteria.Insert(transaction, approvalCriteria);
        }

        return result;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion
  }
}