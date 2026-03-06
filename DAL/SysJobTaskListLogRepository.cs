using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class SysJobTaskListLogRepository : BaseRepository, ISysJobTaskListLogRepository
  {
    private readonly string tableBase = "sys_job_tasklist_log";
    public Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      throw new NotImplementedException();
    }

    public Task<SysJobTaskListLog> GetRowByID(IDbTransaction transaction, string id)
    {
      throw new NotImplementedException();
    }

    public Task<List<SysJobTaskListLog>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }

    public async Task<int> Insert(IDbTransaction transaction, SysJobTaskListLog model)
    {
      var p = db.Symbol();

      string query =
        $@"
            insert into {tableBase}
            (
              id
              ,cre_date
              ,cre_by
              ,cre_ip_address
              ,mod_date
              ,mod_by
              ,mod_ip_address
              ,job_tasklist_code
              ,status
              ,start_date
              ,end_date
              ,log_description
              ,run_by
              ,from_id
              ,to_id
              ,number_of_rows
            )
            values
            (
              {p}ID
              ,{p}CreDate
              ,{p}CreBy
              ,{p}CreIPAddress
              ,{p}ModDate
              ,{p}ModBy
              ,{p}ModIPAddress
              ,{p}JobTaskListCode
              ,{p}Status
              ,{p}StartDate
              ,{p}EndDate
              ,{p}LogDescription
              ,{p}RunBy
              ,{p}FromID
              ,{p}ToID
              ,{p}NumberOfRows
            )";
      return await _command.Insert(transaction, query, model);
    }

    public Task<int> UpdateByID(IDbTransaction transaction, SysJobTaskListLog model)
    {
      throw new NotImplementedException();
    }
  }
}