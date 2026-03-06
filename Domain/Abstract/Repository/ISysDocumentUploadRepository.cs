using System.Data;
using Domain.Models;

namespace Domain.Abstract.Repository
{
  public interface ISysDocumentUploadRepository : IBaseRepository<SysDocumentUpload>
  {
    Task<SysDocumentUpload> GetRowByTrxAndName(IDbTransaction transaction, string trxCode, string fileName);
    Task<int> DeleteByTrxAndFileName(IDbTransaction transaction, string trxCode, string fileName);
  }
}
