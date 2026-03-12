using DotNetEnv;
using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Messaging.MessageBrokerConfiguration;

public static class KafkaConfiguration
{
    private readonly static string _ifinapvMasterApprovalSync = "ifinapv-master-approval-sync";
    private readonly static string _ifinicsAgreementIncentiveMarketingInsert = "ifinics-agreement-incentive-marketing-insert";

    private readonly static string _bootstrapServer = Env.GetString("KAFKA_BOOTSTRAP_SERVER") ?? throw new ArgumentException("KAFKA_BOOTSTRAP_SERVER required");
    private readonly static string _groupID = Env.GetString("KAFKA_GROUP_ID") ?? throw new ArgumentException("KAFKA_GROUP_ID is required");
    public static async Task<IServiceCollection> AddKafkaServices(this IServiceCollection services)
    {
        services.AddKafka(
        kafka => kafka
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .WithBrokers(new[] { _bootstrapServer })
                

                .AddConsumer(consumer => consumer
                    .Topic(_ifinicsAgreementIncentiveMarketingInsert) 
                    .WithGroupId(_groupID)
                    .WithBufferSize(100)
                    .WithWorkersCount(3)
                    .AddMiddlewares(middleware => middleware
                        .Add<AgreementIncentiveMarketingConsumer>()
                    )
                )
            )
    );



        var serviceProvider = services.BuildServiceProvider();
        var bus = serviceProvider.CreateKafkaBus();
        await bus.StartAsync();
        return services;
    }
}
