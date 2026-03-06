using System.Text.Json;

namespace Domain.Models
{
  public class DynamicReport : ExtendModel
  {
    public string? Title { get; set; }
    public int? IsPublished { get; set; }
    public string? Remarks { get; set; }
    public string? Query { get; set; }
    private IDictionary<string, object?>? _parameters;
    public IDictionary<string, object?>? Parameters
    {
      get => _parameters; set
      {
        if (value != null)
        {
          _parameters = new Dictionary<string, object?>();
          foreach (var item in value)
          {

            if (item.Value?.GetType() == typeof(JsonElement))
            {
              var json = (JsonElement)item.Value;
              _parameters[item.Key] = JsonElementConverter(json);
            }

            else
            {
              _parameters[item.Key] = item.Value;
            }

          }
        }
        else
        {
          _parameters = null;
        }
      }
    }

    private static object? JsonElementConverter(JsonElement json)
    {
      switch (json.ValueKind)
      {
        case JsonValueKind.String:
          if (json.TryGetDateTime(out DateTime dt))
          {
            return dt;
          }
          else
          {
            return json.GetString();
          }

        case JsonValueKind.Number:
          if (json.TryGetInt32(out int i))
          {
            return i;
          }
          else if (json.TryGetDecimal(out decimal d))
          {
            return d;
          }
          break;
      }

      return json.ToString();
    }
  }
}


