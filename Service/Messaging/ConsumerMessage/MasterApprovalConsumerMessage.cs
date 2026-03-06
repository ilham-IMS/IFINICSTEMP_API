using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using Domain.Models.Message;
using iFinancing360.Service.Helper;
using Microsoft.Extensions.Logging;
using Service.Messaging.Abstraction.IConsumerService;
using System.Text.Json;


namespace Service.Messaging.ConsumerMessage;
public class MasterApprovalConsumerMessage : BaseMessagingService, IMasterApprovalConsumerMessage
{
  private readonly ILogger<MasterApprovalConsumerMessage> _logger;
  private readonly IInterfaceMasterApprovalRepository _repoInterfaceMasterApproval;
  private readonly IInterfaceMasterApprovalCriteriaRepository _repoInterfaceMasterApprovalCriteria;

  public MasterApprovalConsumerMessage(ILogger<MasterApprovalConsumerMessage> logger, IInterfaceMasterApprovalRepository repoInterfaceMasterApproval, IInterfaceMasterApprovalCriteriaRepository repoInterfaceMasterApprovalCriteria)
  {
    _logger = logger;
    _repoInterfaceMasterApproval = repoInterfaceMasterApproval;
    _repoInterfaceMasterApprovalCriteria = repoInterfaceMasterApprovalCriteria;
  }

  public async Task ConsumeRequest(MessageMasterApproval message)
  {
    try
    {
      if (message.ReffModuleCode != "IFINICS") return;

      await ConsumeMasterApproval(message);
    }
    catch (Exception e)
    {
      _logger.LogWarning("Kafka consumer Error: {Message}", e.Message);
      throw;
    }

  }

  private async Task ConsumeMasterApproval(MessageMasterApproval message)
  {
    using var connection = await GetDbConnection();
    using var transaction = connection.BeginTransaction();
    try
    {
      int result = 0;

      InterfaceMasterApproval masterApproval = new()
      {
        ID = GUID(),
        CreDate = message.CreDate,
        CreBy = message.CreBy,
        CreIPAddress = message.CreIPAddress,
        ModDate = message.ModDate,
        ModBy = message.ModBy,
        ModIPAddress = message.ModIPAddress,

        ApprovalCategoryID = message.ApprovalCategoryID,
        ApprovalCategoryCode = message.ApprovalCategoryCode,
        ApprovalCategoryName = message.ApprovalCategoryName,
        ReffModuleCode = message.ReffModuleCode,
        JobStatus = "HOLD",
      };
      result += await _repoInterfaceMasterApproval.Insert(transaction, masterApproval);

      if (message.Criterias != null && message.Criterias.Any())
      {
        foreach (var item in message.Criterias)
        {
          InterfaceMasterApprovalCriteria masterApprovalCriteria = new()
          {
            ID = GUID(),
            CreDate = message.CreDate,
            CreBy = message.CreBy,
            CreIPAddress = message.CreIPAddress,
            ModDate = message.ModDate,
            ModBy = message.ModBy,
            ModIPAddress = message.ModIPAddress,
            InterfaceApprovalID = masterApproval.ID,
            CriteriaID = item.CriteriaID,
            CriteriaCode = item.CriteriaCode,
            CriteriaDescription = item.CriteriaDescription,
            JobStatus = "HOLD",
          };
          result += await _repoInterfaceMasterApprovalCriteria.Insert(transaction, masterApprovalCriteria);
        }
      }

      transaction.Commit();
    }
    catch (Exception)
    {
      transaction.Rollback();
      throw;
    }
  }
}