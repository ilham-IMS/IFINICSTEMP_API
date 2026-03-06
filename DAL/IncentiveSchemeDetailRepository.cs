
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class IncentiveSchemeDetailRepository : BaseRepository, IIncentiveSchemeDetailRepository
  {
    private readonly string tableBase = "incentive_scheme_detail";

    public async Task<IncentiveSchemeDetail> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                    AS ID
                                ,incentive_scheme_id   AS IncentiveSchemeID
                                ,from_rate            AS FromRate
                                ,to_rate              AS ToRate
                                ,incentive_rate       AS IncentiveRate
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<IncentiveSchemeDetail>(transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveSchemeDetail>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_scheme_id   AS IncentiveSchemeID
                              ,from_rate            AS FromRate
                              ,to_rate              AS ToRate
                              ,incentive_rate       AS IncentiveRate
                        from 
                            {tableBase}
                        where 
                        (
                            
                            cast({FormatNumeric("from_rate", 6)} as char(50))            like lower({p}Keyword)
							              or cast({FormatNumeric("to_rate", 6)} as char(50))              like lower({p}Keyword)
                            or cast({FormatNumeric("incentive_rate", 6)} as char(50))       like lower({p}Keyword)
                            
                        )
                        order by
                        cre_date asc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<IncentiveSchemeDetail>
      (transaction, query, parameters);
      return result;
    }

   public async Task<List<IncentiveSchemeDetail>> GetRowsBySchemeID(IDbTransaction transaction, string? keyword, int offset, int limit, string schemeID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_scheme_id   AS IncentiveSchemeID
                              ,from_rate            AS FromRate
                              ,to_rate              AS ToRate
                              ,incentive_rate       AS IncentiveRate
                        from 
                            {tableBase}
                        where 
                            incentive_scheme_id = {p}IncentiveSchemeID
                        ";
      
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        query += $@"
                        and
                        (
                            cast({FormatNumeric("from_rate", 6)} as char(50))      like {p}Keyword
                            or cast({FormatNumeric("to_rate", 6)} as char(50))     like {p}Keyword
                            or cast({FormatNumeric("incentive_rate", 6)} as char(50)) like {p}Keyword
                        )
                        ";
      }
      
      query += $@"
                  order by
                  cre_date asc
                  ";
      
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = string.IsNullOrWhiteSpace(keyword) ? "%" : $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        IncentiveSchemeID = schemeID
      };

      var result = await _command.GetRows<IncentiveSchemeDetail>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, IncentiveSchemeDetail model)
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
                                ,incentive_scheme_id
                                ,from_rate
                                ,to_rate
                                ,incentive_rate
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
                                ,{p}IncentiveSchemeID
                                ,{p}FromRate
                                ,{p}ToRate
                                ,{p}IncentiveRate
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, IncentiveSchemeDetail model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_scheme_id	 =	{p}IncentiveSchemeID
                                ,from_rate		           = 	{p}FromRate
                                ,to_rate		           = 	{p}ToRate
                                ,incentive_rate		   = 	{p}IncentiveRate
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