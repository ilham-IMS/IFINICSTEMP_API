
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class MasterUserRepository : BaseRepository, IMasterUserRepository
  {
    private readonly string tableBase = "master_user";
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      return await _command.DeleteByID(transaction, query, id.ToString());
    }

    public async Task<MasterUser> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                    AS ID
                                ,employee_code				AS	EmployeeCode	
                                ,employee_name			  AS	EmployeeName
                                ,employee_id				  AS	EmployeeID
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<MasterUser>(transaction, query, parameters);
      return result;
    }

    public async Task<List<MasterUser>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                          select 
                                id                    AS ID
                                ,employee_id			    AS	EmployeeID	
                                ,employee_code				AS	EmployeeCode	
                                ,employee_name			  AS	EmployeeName
                          from 
                              {tableBase}
                          where 
                          (
                            lower(employee_code)              like    lower({p}Keyword)
                            or lower(employee_name)           like    lower({p}Keyword)
                        )
                        order by
                        mod_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<MasterUser>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, MasterUser model)
    {
      var p = db.Symbol();
      string query = $@"
                      insert into {tableBase}
                            (
                                id
                                ,cre_date
                                ,cre_by
                                ,cre_ip_address
                                ,mod_date
                                ,mod_by
                                ,mod_ip_address
                                ,employee_code          
                                ,employee_name
                                ,employee_id 
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
                                ,{p}EmployeeCode
                                ,{p}EmployeeName
                                ,{p}EmployeeID
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, MasterUser model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,employee_code			   =	{p}EmployeeCode
                                ,employee_name		     = 	{p}EmployeeName
                            where
                              id = {p}ID";
      return await _command.Update(transaction, query, model);
    }

    public async Task<List<MasterUser>> GetReportData(IDbTransaction transaction)
    {
      string query =
                                      $@"
                                select
                                         id                         AS ID
                                        ,employee_code				AS	EmployeeCode	
                                        ,employee_name			  AS	EmployeeName
                                from
                                    {tableBase}
                                order by
                                    mod_date desc";
      var result = await _command.GetRows<MasterUser>(
              transaction,
              query);
      return result;
    }
  }
}