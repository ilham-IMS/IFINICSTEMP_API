using System.Data;
using Domain.Models;
using Domain.Models.Message;

namespace Service.Messaging.Abstraction.IConsumerService
{
  public interface IMasterApprovalConsumerMessage : IBaseConsumerMessage
  {
    public Task ConsumeRequest(MessageMasterApproval message);

  }
}