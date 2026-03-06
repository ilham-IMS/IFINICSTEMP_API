using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface IInformationSchemaColumnRepository : IBaseRepository<InformationSchemaColumn>
  {
    Task<List<InformationSchemaColumn>> GetRows(IDbTransaction transaction, string tableName);
    Task<InformationSchemaColumn> GetRowByName(IDbTransaction transaction, string name);

  }
}