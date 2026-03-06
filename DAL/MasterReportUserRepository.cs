using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class MasterReportUserRepository : BaseRepository, IMasterReportUserRepository
  {
    private readonly string tableBase = "master_report_user";
    private readonly string tableSysReport = "sys_report";
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      return await _command.DeleteByID(transaction, query, id.ToString());
    }

    public Task<MasterReportUser> GetRowByID(IDbTransaction transaction, string id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<MasterReportUser>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();

      string query =
                  $@"
                            select
                                    mru.id              AS ID
                                    ,employee_id				          AS	EmployeeID	
                                    ,report_id			              AS	ReportID
                                    ,sr.name				              AS	ReportName	
                                    ,sr.report_type			          AS	ReportType
                            from 
                                    {tableBase} mru
                            inner join {tableSysReport} sr on mru.report_id = sr.id
                            where
                                    (
                                        lower(employee_id)           like    lower({p}Keyword)
                                        or lower(report_id)             like    lower({p}Keyword)
                                        or lower(name)                  like    lower({p}Keyword)
                                        or lower(report_type)           like    lower({p}Keyword)
                                    )
                            order by
                                    mru.mod_date desc

                        ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<MasterReportUser>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, MasterReportUser model)
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
              ,employee_id				          			  
              ,report_id				          			  
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
              ,{p}EmployeeID
              ,{p}ReportID
            )";
      return await _command.Insert(transaction, query, model);
    }

    public Task<int> UpdateByID(IDbTransaction transaction, MasterReportUser model)
    {
      throw new NotImplementedException();
    }
  }
}