using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using Domain.Models.Message;
using iFinancing360.Service.Helper;

namespace API.Jobs
{
    public class InterfaceMasterApprovalJobIn : BaseService
    {
      private readonly ISysJobTaskService _sysJobTaskService;
      private readonly ISysJobTaskLogService _sysJobTaskLogService;
      private readonly IInterfaceMasterApprovalRepository _interfaceMasterApprovalRepo;
      private readonly IInterfaceMasterApprovalService _interfaceMasterApprovalService;
      private readonly IInterfaceMasterApprovalCriteriaService _interfaceMasterApprovalCriteriaService;
      private readonly IMasterApprovalService _masterApprovalService;

    public InterfaceMasterApprovalJobIn(ISysJobTaskService sysJobTaskService, ISysJobTaskLogService sysJobTaskLogService, IInterfaceMasterApprovalService interfaceMasterApprovalService, IInterfaceMasterApprovalCriteriaService interfaceMasterApprovalCriteriaService, IInterfaceMasterApprovalRepository interfaceMasterApprovalRepo, IMasterApprovalService masterApprovalService)
    {
      _sysJobTaskService = sysJobTaskService;
      _sysJobTaskLogService = sysJobTaskLogService;
      _interfaceMasterApprovalService = interfaceMasterApprovalService;
      _interfaceMasterApprovalCriteriaService = interfaceMasterApprovalCriteriaService;
      _interfaceMasterApprovalRepo = interfaceMasterApprovalRepo;
      _masterApprovalService = masterApprovalService;
    }

    public async Task StartAsync()
      {
        SysJobTask jobTask = await _sysJobTaskService.GetRowByCode("IMAC");
        var interfaceModel = await _interfaceMasterApprovalService.GetRowsForJobIn(jobTask.RowToProcess ?? 100);

        if (interfaceModel.Any())
        {
            foreach (var model in interfaceModel)
            {
              try
              {
                  await HandleInterfaceToTransaction(model);
                  await HandleJobIn(jobTask, model);
              }
              catch (Exception ex)
              {
                  await HandleJobIn(jobTask, model, ex);
                  throw;
              }
            }
        }
      }

      private async Task HandleInterfaceToTransaction(InterfaceMasterApproval interfaceModel)
      {
        using var connection = _interfaceMasterApprovalRepo.GetDbConnection();
        using var transaction = connection.BeginTransaction();
        try
        {
          var criterias = await _interfaceMasterApprovalCriteriaService.GetRows(transaction, interfaceModel.ID!);

          interfaceModel.Criterias ??= new List<InterfaceMasterApprovalCriteria>();

          interfaceModel.Criterias.AddRange(criterias);

          await _masterApprovalService.ProcessSync(transaction, interfaceModel);

          transaction.Commit();
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }
      }

      protected async Task HandleJobIn(SysJobTask jobTask, InterfaceMasterApproval model, Exception? ex = null)
      {
        // Update status interface
        InterfaceMasterApproval iMasterApproval = new()
        {
          ID = model.ID,
          JobStatus = ex == null ? "SUCESS" : "FAILED",
          ErrorMessage = ex == null ? " " : ex?.Message,
          StackTrace = ex == null ? " " : ex?.StackTrace,
        };

        await _interfaceMasterApprovalService.UpdateAfterJobIn(iMasterApproval);
        
        SysJobTask sysJobTask = new()
        {
          ID = jobTask.ID,
          JobStatus = ex == null ? "SUCCESS" : "FAILED"
        };
        await _sysJobTaskService.UpdateJobStatus(sysJobTask);
        
        if (ex != null)
        {
          SysJobTaskLog jobTaskLog = new()
          {
            ID = GUID(),
            CreDate = DateTime.Now,
            CreBy = "JOB",
            CreIPAddress = "127.0.0.1",
            ModDate = DateTime.Now,
            ModBy = "JOB",
            ModIPAddress = "127.0.0.1",
            SysJobTaskID = jobTask.ID,
            ErrorMessage = ex?.Message,
            StackTrace = ex?.StackTrace
          };
          await _sysJobTaskLogService.Insert(jobTaskLog);
        }
      }
    }
}