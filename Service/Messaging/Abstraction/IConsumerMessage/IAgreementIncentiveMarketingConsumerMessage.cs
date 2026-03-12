using System.Data;
using Domain.Models;
using Domain.Models.Message;

namespace Service.Messaging.Abstraction.IConsumerService
{
  public interface IAgreementIncentiveMarketingConsumerMessage : IBaseConsumerMessage
  {
    public Task ConsumeRequest(MessageAgreementIncentiveMarketing message);
  }
}