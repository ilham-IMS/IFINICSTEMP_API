using System.Reflection;
using API.Jobs;
using iFinancing360.API.Helper;
using iFinancing360.DAL.Helper;
using iFinancing360.Service.Helper;
using Service.Messaging.ConsumerMessage;

namespace API.ServiceRegister
{
  public static class ServiceRegister
  {
    public static void AddService(this IServiceCollection services)
    {
      services.AddHostedService<MessageBackgroundWorker>();
      services.AddScoped<InterfaceMasterApprovalJobIn>();
      services.AddScoped<NanoDocClient>();
      // services.AddScoped<IMasterBookRepository, MasterBookRepository>();
      // services.AddScoped<IMasterBookService, MasterBookService>();
      services.AddSingleton<AesEncryptionService>();
      services.AddScoped<DecryptRequestBodyAttribute>();
      services.AddScoped<DecryptQueryStringAttribute>();
      services.AddHttpContextAccessor();
      // services.AddTransient<AppraisalRequestConsumerMessage>();
      List<Type> allRepositories = Assembly.Load("DAL")
      .GetTypes()
      .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseRepository)))
      .ToList();

      Console.WriteLine($"\nRegistering {allRepositories.Count} Repository");

      allRepositories.ForEach(repositoryClass =>
      {
        var repositoryInterface = repositoryClass.GetInterfaces().Where(i => !i.IsGenericType).First();

        Console.WriteLine($"Registering Repository {repositoryClass.Name} with interface {repositoryInterface.Name}");
        services.AddScoped(repositoryInterface, repositoryClass);
      });

      List<Type> allServices = Assembly.Load("Service")
      .GetTypes()
      .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseService)))
      .ToList();

      Console.WriteLine($"\nRegistering {allServices.Count} Service");
      allServices.ForEach(serviceClass =>
      {
        var serviceInterface = serviceClass.GetInterfaces().Where(i => !i.IsGenericType).First();

        Console.WriteLine($"Registering Service {serviceClass.Name} with interface {serviceInterface.Name}");
        services.AddScoped(serviceInterface, serviceClass);
      });

      List<Type> allMessages = Assembly.Load("Service")
      .GetTypes()
      .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseMessagingService)))
      .ToList();

      Console.WriteLine($"\nRegistering {allMessages.Count} Message");
      allMessages.ForEach(messageClass =>
      {
        var messageInterface = messageClass.GetInterfaces().Where(i => !i.IsGenericType && i.Name.Contains(messageClass.Name)).First();

        Console.WriteLine($"Registering Message {messageClass.Name} with interface {messageInterface.Name}");
        services.AddScoped(messageInterface, messageClass);
      });
    }
  }
}
