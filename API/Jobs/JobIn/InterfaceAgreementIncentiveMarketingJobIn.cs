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
    public class InterfaceAgreementIncentiveMarketingJobIn : BaseService
    {
      private readonly ISysJobTaskService _sysJobTaskService;
      private readonly ISysJobTaskLogService _sysJobTaskLogService;
      private readonly IInterfaceAgreementIncentiveMarketingRepository _interfaceAgreementIncentiveMarketingRepo;
      private readonly IInterfaceAgreementIncentiveMarketingService _interfaceAgreementIncentiveMarketingService;
      private readonly IInterfaceAgreementFeeService _interfaceAgreementFeeService;
      private readonly IInterfaceAgreementRefundService _interfaceAgreementRefundService;
      private readonly IAgreementIncentiveMarketingService _agreementIncentiveMarketingService;

    public InterfaceAgreementIncentiveMarketingJobIn(ISysJobTaskService sysJobTaskService, ISysJobTaskLogService sysJobTaskLogService, IInterfaceAgreementIncentiveMarketingService interfaceAgreementIncentiveMarketingService, IInterfaceAgreementFeeService interfaceAgreementFeeService, IInterfaceAgreementIncentiveMarketingRepository interfaceAgreementIncentiveMarketingRepo, IAgreementIncentiveMarketingService agreementIncentiveMarketingService, IInterfaceAgreementRefundService interfaceAgreementRefundService)
    {
      _sysJobTaskService = sysJobTaskService;
      _sysJobTaskLogService = sysJobTaskLogService;
      _interfaceAgreementIncentiveMarketingService = interfaceAgreementIncentiveMarketingService;
      _interfaceAgreementFeeService = interfaceAgreementFeeService;
      _interfaceAgreementIncentiveMarketingRepo = interfaceAgreementIncentiveMarketingRepo;
      _agreementIncentiveMarketingService = agreementIncentiveMarketingService;
      _interfaceAgreementRefundService = interfaceAgreementRefundService;
    }

    public async Task StartAsync()
      {
        SysJobTask jobTask = await _sysJobTaskService.GetRowByCode("IMAC");
        var interfaceModel = await _interfaceAgreementIncentiveMarketingService.GetRowsForJobIn(jobTask.RowToProcess ?? 100);

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

      private async Task HandleInterfaceToTransaction(InterfaceAgreementIncentiveMarketing interfaceModel)
      {
        using var connection = _interfaceAgreementIncentiveMarketingRepo.GetDbConnection();
        using var transaction = connection.BeginTransaction();
        try
        {
          var fees = await _interfaceAgreementFeeService.GetRows(transaction, interfaceModel.ID);
          var refunds = await _interfaceAgreementRefundService.GetRows(transaction, interfaceModel.ID);

          interfaceModel.AgreementFees ??= new List<InterfaceAgreementFee>();
          interfaceModel.AgreementRefunds ??= new List<InterfaceAgreementRefund>();

          interfaceModel.AgreementFees.AddRange(fees);
          interfaceModel.AgreementRefunds.AddRange(refunds);

          await _agreementIncentiveMarketingService.ProcessSync(transaction, interfaceModel);


          transaction.Commit();
        }
        catch (Exception)
        {
          transaction.Rollback();
          throw;
        }
      }

      protected async Task HandleJobIn(SysJobTask jobTask, InterfaceAgreementIncentiveMarketing model, Exception? ex = null)
      {
        // Update status interface
        InterfaceAgreementIncentiveMarketing iAgreementIncentiveMarketing = new()
        {
          ID = model.ID,
          JobStatus = ex == null ? "SUCESS" : "FAILED",
          ErrorMessage = ex == null ? " " : ex?.Message,
          StackTrace = ex == null ? " " : ex?.StackTrace,
        };

        await _interfaceAgreementIncentiveMarketingService.UpdateAfterJobIn(iAgreementIncentiveMarketing);
        
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