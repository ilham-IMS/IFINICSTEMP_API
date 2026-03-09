
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class IncentiveCollectionRepository : BaseRepository, IIncentiveCollectionRepository
  {
    private readonly string tableBase = "incentive_collection";

    public async Task<IncentiveCollection> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                      AS ID
                                ,client_id			        AS	ClientID
                                ,client_no			        AS	ClientNo
                                ,client_name		        AS	ClientName
                                ,incentive_periode	    AS	IncentivePeriode
                                ,total_incentive_amount AS TotalIncentiveAmount
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<IncentiveCollection>(transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveCollection>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,client_id			        AS	ClientID
                              ,client_no			        AS	ClientNo
                              ,client_name		        AS	ClientName
                              ,incentive_periode	    AS	IncentivePeriode
                              ,total_incentive_amount AS TotalIncentiveAmount
                        from 
                            {tableBase}
                        where 
                        (
                            
                            lower(client_name)                                                 like lower({p}Keyword)
                            or lower(incentive_periode)                                           like lower({p}Keyword)
                            or lower({FormatNumeric("total_incentive_amount")})                           like lower({p}Keyword)
                        )
                        order by
                        cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<IncentiveCollection>
      (transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveCollection>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string? PeriodeFrom = null, string? PeriodeTo = null)
    {
      var p = db.Symbol();
      
      string periodCondition = "";
      if (!string.IsNullOrEmpty(PeriodeFrom) && !string.IsNullOrEmpty(PeriodeTo))
      {
        periodCondition = $@"CAST(incentive_periode AS INT) BETWEEN CAST({p}PeriodeFrom AS INT) AND CAST({p}PeriodeTo AS INT) AND";
      }
      
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,client_id			        AS	ClientID
                              ,client_no			        AS	ClientNo
                              ,client_name		        AS	ClientName
                              ,incentive_periode	    AS	IncentivePeriode
                              ,total_incentive_amount AS TotalIncentiveAmount
                        from 
                            {tableBase}
                        where 
                        {periodCondition}
                        (
                            lower(client_name)                                                 like lower({p}Keyword)
                            or lower(incentive_periode)                                        like lower({p}Keyword)
                            or lower({FormatNumeric("total_incentive_amount")})                like lower({p}Keyword)
                        )
                        order by
                        cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        PeriodeFrom,
        PeriodeTo
      };

      var result = await _command.GetRows<IncentiveCollection>(transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, IncentiveCollection model)
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
                                ,client_id
                                ,client_no
                                ,client_name
                                ,incentive_periode
                                ,total_incentive_amount
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
                                ,{p}ClientID
                                ,{p}ClientNo
                                ,{p}ClientName
                                ,{p}IncentivePeriode
                                ,{p}TotalIncentiveAmount
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, IncentiveCollection model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,client_id             =  {p}ClientID
                                ,client_no             =  {p}ClientNo
                                ,client_name           =  {p}ClientName
                                ,incentive_periode     =  {p}IncentivePeriode
                                ,total_incentive_amount =  {p}TotalIncentiveAmount
                            where
                              id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
    
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      return await _command.DeleteByID(transaction, query, id.ToString());
    }

  }
}