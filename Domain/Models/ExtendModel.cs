using System.Dynamic;

namespace Domain.Models
{
   public class ExtendModel : BaseModel
   {
      public string? Keyy { get; set; }
      public string? Value { get; set; }
      public string? ParentID { get; set; }
      public dynamic? Properties { get; set; } = new ExpandoObject();
      public void AddProperty(string propertyName, object value)
      {
         ((IDictionary<string, object>)Properties)[propertyName] = value;
      }

      public object GetProperty(string propertyName)
      {
         if (((IDictionary<string, object>)Properties).ContainsKey(propertyName))
         {
            return ((IDictionary<string, object>)Properties)[propertyName];
         }
         else
         {
            throw new Exception($"Property {propertyName} does not exist");
         }
      }

   }
}