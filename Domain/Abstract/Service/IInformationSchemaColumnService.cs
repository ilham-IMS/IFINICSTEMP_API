using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IInformationSchemaColumnService : IBaseService<InformationSchemaColumn>
  {
    Task<List<InformationSchemaColumn>> GetRows(string tableName);
  }
}