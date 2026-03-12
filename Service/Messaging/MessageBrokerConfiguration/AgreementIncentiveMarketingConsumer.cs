using Domain.Models;
using Domain.Models.Message;
using iFinancing360.Service.Helper;
using KafkaFlow;
using Service.Messaging.Abstraction.IConsumerService;

public class AgreementIncentiveMarketingConsumer : BaseMessagingService, IMessageMiddleware, IAgreementIncentiveMarketingConsumer
{
  private readonly IAgreementIncentiveMarketingConsumerMessage _agreementIncentiveMarketingConsumerMessage;

  public AgreementIncentiveMarketingConsumer(IAgreementIncentiveMarketingConsumerMessage agreementIncentiveMarketingConsumerMessage)
  {
    _agreementIncentiveMarketingConsumerMessage =  agreementIncentiveMarketingConsumerMessage;
  }

  public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
  {

    try
    {
      var message = ConvertMessage<MessageAgreementIncentiveMarketing>(context);
      await _agreementIncentiveMarketingConsumerMessage.ConsumeRequest(message);

      await ProduceSucces(context);
    }
    catch (Exception ex)
    {
      await ProduceError(ex, context);
    }

  }

}

public interface IAgreementIncentiveMarketingConsumer { }