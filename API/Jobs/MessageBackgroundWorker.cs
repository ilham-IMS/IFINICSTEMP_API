// using Service.Messaging;
// using Service.Messaging.Abstraction.IConsumerMessage;

// namespace API.Jobs;

// public class MessageBackgroundWorker : IHostedService
// {
//   private List<Timer?> _timerList = [];
//   readonly ILogger<MessageBackgroundWorker> _logger;

//   readonly ISurveyRequestConsumerMessage _SurveyRequestConsumerMessage;

//   public MessageBackgroundWorker(ILogger<MessageBackgroundWorker> logger, IServiceScopeFactory serviceScopeFactory)
//   {
//     _logger = logger;

//     // Inject Message Service
//     using var scope = serviceScopeFactory.CreateScope();
//     _SurveyRequestConsumerMessage = scope.ServiceProvider.GetRequiredService<ISurveyRequestConsumerMessage>();
//   }

//   public Task StartAsync(CancellationToken cancellationToken)
//   {
//     try
//     {
//       _timerList.Add(AddBackgroundTask((async (object state) => await _SurveyRequestConsumerMessage.ConsumeRequestInsert())!, 10));
//     }
//     catch (System.Exception ex)
//     {
//       _logger.LogError("Error on Background Worker: {Message}, {Trace}", ex.Message, ex.StackTrace);
//       throw;
//     }

//     return Task.CompletedTask;
//   }

//   private Timer AddBackgroundTask(TimerCallback callback, int second)
//   {
//       _logger.LogInformation("Add Background Task for {Second} seconds", second);
//       return new Timer(state => Task.Run(() => callback(state)), null, TimeSpan.Zero, TimeSpan.FromSeconds(second));
//   }


//   public Task StopAsync(CancellationToken cancellationToken)
//   {
//     _timerList.ForEach(t => t?.Change(Timeout.Infinite, 0));
//     return Task.CompletedTask;
//   }
// }

namespace API.Jobs;
public class MessageBackgroundWorker : IHostedService
{
    private readonly List<Timer?> _timerList = [];
    private readonly ILogger<MessageBackgroundWorker> _logger;


    private readonly SemaphoreSlim _interfaceMasterApprovalSemaphore = new(1, 1);

    #region Service
    private readonly InterfaceMasterApprovalJobIn _interfaceMasterApprovalJobIn;
    
    #endregion

    public MessageBackgroundWorker(ILogger<MessageBackgroundWorker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        using var scope = serviceScopeFactory.CreateScope();
        _interfaceMasterApprovalJobIn = scope.ServiceProvider.GetRequiredService<InterfaceMasterApprovalJobIn>();
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service running every {interval} seconds.", 5);

        _timerList.Add(AddBackgroundTask(async state => await SafeConsume(_interfaceMasterApprovalSemaphore, _interfaceMasterApprovalJobIn.StartAsync), 5));

        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timerList.ForEach(t => t?.Change(Timeout.Infinite, 0));
        return Task.CompletedTask;
    }
    private Timer AddBackgroundTask(TimerCallback callback, int second)
    {
        _logger.LogInformation("Add Background Task for {Second} seconds", second);
        return new Timer(state => Task.Run(() => callback(state)), null, TimeSpan.Zero, TimeSpan.FromSeconds(second));
    }

    private async Task SafeConsume(SemaphoreSlim semaphore, Func<Task> consumeMethod)
    {
        await semaphore.WaitAsync();

        try
        {
            await consumeMethod();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in consumer method");
        }
        finally
        {
            semaphore.Release();
        }
    }
}
