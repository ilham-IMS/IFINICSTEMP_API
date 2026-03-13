using Domain.Models;
using iFinancing360.Helper;

namespace Domain.Abstract.Service
{
  public interface IAgreementCollectionService : IBaseService<AgreementCollection>
  {
    Task<List<AgreementCollection>> GetRowsByIncentiveID(string? keyword, int offset, int limit, string incentiveID);
  }
}