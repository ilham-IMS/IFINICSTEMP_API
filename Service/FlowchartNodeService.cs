using System.Reflection;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using DotNetEnv;
using iFinancing360.Service.Helper;

namespace Service
{
  public class FlowchartNodeService : BaseService, IFlowchartNodeService
  {
    private readonly IFlowchartNodeRepository _repo;

    public FlowchartNodeService(
         IFlowchartNodeRepository repo
    )
    {
      _repo = repo;
    }

    public async Task<List<FlowchartNode>> GetRows(string masterFlowchartID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.GetRows(transaction, masterFlowchartID);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
      finally
      {
        connection.Close();
      }
    }

    public async Task<List<FlowchartNode>> GetRowsByCode(string roleCode)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.GetRowsByCode(transaction, roleCode);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
      finally
      {
        connection.Close();
      }
    }

    public async Task<FlowchartNode> GetRowByID(string ID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        var result = await _repo.GetRowByID(transaction, ID);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<int> Insert(FlowchartNode model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {

        bool isStartNodeExist = await _repo.IsStartNodeExist(transaction, model.DynamicButtonProcessRoleID!);

        List<FlowchartNode> nodes = await _repo.GetRowsByDynamicButtonProcessRoleID(transaction, model.DynamicButtonProcessRoleID!);

        if(nodes.Any(n => n.NodeName == model.NodeName)) throw new ArgumentException("Node Name Alredy Exist");

        if (isStartNodeExist == false)
        {
          FlowchartNode startNode = new FlowchartNode()
          {
            ID = GUID(),
            CreDate = model.CreDate,
            CreBy = model.CreBy,
            CreIPAddress = model.CreIPAddress,
            ModDate = model.ModDate,
            ModBy = model.ModBy,
            ModIPAddress = model.ModIPAddress,
            NodeName = "Start",
            DynamicButtonProcessRoleID = model.DynamicButtonProcessRoleID,
            XCoordinat = 100,
            YCoordinat = 25,
            ShortDescription = "Start",
            SourceLink = "Start",
            TargetLink = model.NodeName,
            MethodName = "Start"
          };
          await _repo.Insert(transaction, startNode);
        }


        model.XCoordinat = nodes.LastOrDefault()?.XCoordinat ?? 100;
        model.YCoordinat = (nodes.LastOrDefault()?.YCoordinat ?? 100) + 100 ;
        model.SourceLink = model.NodeName;
        var result = await _repo.Insert(transaction, model);
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<int> Update(List<FlowchartNode> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int result = 0;
        foreach (var model in models)
        {
          result += await _repo.UpdateByID(transaction, model);
        }
        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public async Task<int> DeleteByID(List<FlowchartNode> models)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int count = 0;
        foreach (var model in models)
        {
          var node = await _repo.GetRowByID(transaction, model.ID!);

          // Update Flowchart yang memiliki TargetLink terhadap node yang dihapus

          await _repo.RemoveLinkedNode(transaction, node);
          var result = await _repo.DeleteByID(transaction, model.ID!);
          if (result > 0)
            count += result;

        }
        transaction.Commit();
        return count;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    public async Task<int> DeleteByID(string[] listID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        int count = 0;
        foreach (var ID in listID)
        {
          var result = await _repo.DeleteByID(transaction, ID);
          if (result > 0)
            count += result;

        }
        transaction.Commit();
        return count;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    public Task<List<FlowchartNode>> GetRows(string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }

    public Task<int> UpdateByID(FlowchartNode model)
    {
      throw new NotImplementedException();
    }

    public async Task DynamicExecution()
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();
      try
      {
        List<FlowchartNode> nodes = await _repo.GetRows(transaction, "5ec40bdb52554ede8efeeeca682f9603");
        var orderedNodes = GetOrderedNodes(nodes);
        var parameters = new Dictionary<string, string>
        {
            { "empName", "Rahmat" },
            { "uPass", "123456" },
            { "companyName", "IMS" },
            { "companyAddress", "Jl. Example No. 123" },
            { "companyPhone", "021-12345678" },
            { "companyFax", "021-87654321" }
        };

        var ccEmails = new List<string> { "farrel@ims-tec.com", "fadhil@ims-tec.com" };
        var attachments = new List<(byte[] Content, string FileName)>
        {
            (File.ReadAllBytes("../API/DokumentasiStandarisasipesancommit.pdf"), "DokumentasiStandarisasipesancommit.pdf")
        };

        //   Email.SendEmail("rahmatf.9f@gmail.com", "Reset Password", "../API/ResetPassword.html", parameters, ccEmails, attachments);
        string masterFlowchartID = "5ec40bdb52554ede8efeeeca682f9603";
        string ID = masterFlowchartID;
        foreach (var classWithNameSpace in orderedNodes)
        {
          List<object> param = [transaction];
          var paramTemp = GetParameterMethod(classWithNameSpace?.MethodName);
          param.AddRange(paramTemp);
          var result = await InvokeMethod(classWithNameSpace?.MethodName, param.ToArray());
          param = [];
        }

        transaction.Commit();
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    private async Task<object> InvokeMethod(string assemblyClassMethod, params object[] parameters)
    {
      try
      {
        var (assemblyName, className, methodName) = ParseAssemblyClassAndMethod(assemblyClassMethod);
        var type = GetTypeFromAssembly(assemblyName, className);
        var obj = Activator.CreateInstance(type);
        int paremeterCount = parameters.Count();
        var method = FindMethod(type, methodName, paremeterCount);

        if (method.GetParameters().Length == 0)
        {
          parameters = new object[0];
        }

        var result = method.Invoke(obj, parameters);

        return await HandleTaskResult(result);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
        throw;
      }
    }

    private (string assemblyName, string className, string methodName) ParseAssemblyClassAndMethod(string assemblyClassMethod)
    {
      string[] parts = assemblyClassMethod.Split(';');
      string[] methodParts = parts[2].Split(':');
      string methodName = methodParts[0];
      return (parts[0], parts[1], methodName);
    }

    private object[] GetParameterMethod(string assemblyClassMethod)
    {
      string[] parts = assemblyClassMethod.Split(';');
      string[] methodParts = parts[2].Split(':');
      if (methodParts.Length > 1)
      {
        return methodParts[1].Split(',').Select(p => (object)p).ToArray();
      }
      return new object[0];
    }

    private Type GetTypeFromAssembly(string assemblyName, string className)
    {
      // Load the assembly containing the class
      string dllPath = Env.GetString("DLL_PATH");
      Assembly assembly = Assembly.LoadFrom($"{dllPath}{assemblyName}");
      var type = assembly.GetType(className);
      if (type == null)
      {
        throw new Exception($"Class {className} not found in assembly {assemblyName}.");
      }
      return type;
    }

    private System.Reflection.MethodInfo FindMethod(Type type, string methodName, int parameterCount)
    {
      var methods = type.GetMethods().Where(m => m.Name == methodName).ToArray();

      if (methods.Length == 0)
      {
        throw new Exception($"Method {methodName} not found in class {type.FullName}");
      }

      var method = methods.FirstOrDefault(m => m.GetParameters().Length == parameterCount) ??
                   methods.FirstOrDefault(m => m.GetParameters().Length == 0);

      if (method == null)
      {
        throw new Exception($"No matching method found for {methodName} with {parameterCount} parameters.");
      }

      return method;
    }

    private async Task<object> HandleTaskResult(object result)
    {
      if (result is Task task)
      {
        await task;

        if (task.GetType().IsGenericType)
        {
          var taskResult = task.GetType().GetProperty("Result")?.GetValue(task);
          return taskResult;
        }
      }
      return result;
    }


    private List<FlowchartNode> GetOrderedNodes(List<FlowchartNode> nodes)
    {
      var orderedNodes = new List<FlowchartNode>();
      var nodeDictionary = nodes
          .Where(n => n.SourceLink != null)
          .ToDictionary(n => n.SourceLink);

      var currentNode = nodes.FirstOrDefault(n => n.NodeName == "Start");

      while (currentNode != null)
      {
        orderedNodes.Add(currentNode);
        nodeDictionary.Remove(currentNode.SourceLink);

        if (currentNode.TargetLink == null || !nodeDictionary.TryGetValue(currentNode.TargetLink, out currentNode))
        {
          currentNode = null;
        }
      }

      return orderedNodes;
    }


    private async Task ExecuteNodes(List<FlowchartNode> orderedNodes)
    {
      foreach (var node in orderedNodes)
      {
        var methodName = node.MethodName;
        var method = _repo.GetType().GetMethod(methodName);

        if (method == null) continue;

        var returnType = method.ReturnType;

        if (!typeof(Task).IsAssignableFrom(returnType))
        {
          method.Invoke(_repo, null);
          continue;
        }

        var task = (Task)method.Invoke(_repo, null);
        await task;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
          var resultProperty = returnType.GetProperty("Result");
          resultProperty?.GetValue(task);
        }
      }
    }

    private record Book(int ID, string Title, string Author, decimal Price);

  }
}