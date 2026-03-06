using Domain.Models;
using Domain.Models.Message;
using iFinancing360.Service.Helper;
using KafkaFlow;
using Service.Messaging.Abstraction.IConsumerService;

public class MasterApprovalConsumer : BaseMessagingService, IMessageMiddleware, IMasterApprovalConsumer
{
  private readonly IMasterApprovalConsumerMessage _masterApprovalConsumerMessage;

  public MasterApprovalConsumer(IMasterApprovalConsumerMessage masterApprovalConsumerMessage)
  {
    _masterApprovalConsumerMessage =  masterApprovalConsumerMessage;
  }

  public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
  {

    try
    {
      var message = ConvertMessage<MessageMasterApproval>(context);
      await _masterApprovalConsumerMessage.ConsumeRequest(message);

      await ProduceSucces(context);
    }
    catch (Exception ex)
    {
      await ProduceError(ex, context);
    }

  }

}

public interface IMasterApprovalConsumer { }