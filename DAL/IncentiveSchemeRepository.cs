
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class IncentiveSchemeRepository : BaseRepository, IIncentiveSchemeRepository
  {
    private readonly string tableBase = "incentive_scheme";

    public async Task<IncentiveScheme> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                    AS ID
                                ,incentive_type			  AS	IncentiveType
                                ,eff_date				      AS	EffDate
                                ,is_active				    AS	IsActive
                                ,incentive_ratio		  AS	IncentiveRatio
                                ,rate_penalty			    AS	RatePenalty
                                ,overdue_days_from	  AS	OverdueDaysFrom
                                ,overdue_days_to		  AS	OverdueDaysTo
                                ,minimum_amount			  AS	MinimumAmount
                                ,maximum_amount			  AS	MaximumAmount
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<IncentiveScheme>(transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveScheme>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_type			  AS	IncentiveType
                              ,eff_date				      AS	EffDate
                              ,is_active				    AS	IsActive
                              ,incentive_ratio		  AS	IncentiveRatio
                              ,rate_penalty			    AS	RatePenalty
                              ,overdue_days_from	  AS	OverdueDaysFrom
                              ,overdue_days_to		  AS	OverdueDaysTo
                              ,minimum_amount			  AS	MinimumAmount
                              ,maximum_amount			  AS	MaximumAmount
                        from 
                            {tableBase}
                        where 
                        (
                            lower(incentive_type)                                                 like lower({p}Keyword)
                            or cast({FormatNumeric("incentive_ratio", 6)} as char(50))            like lower({p}Keyword)
							              or cast({FormatDateTime($"eff_date")} as char(50))                    like lower({p}Keyword)
                            or case is_active when 1 then 'yes' else 'no' end                     like lower({p}Keyword)
                            
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

      var result = await _command.GetRows<IncentiveScheme>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, IncentiveScheme model)
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
                                ,incentive_type
                                ,eff_date
                                ,is_active
                                ,incentive_ratio
                                ,rate_penalty
                                ,overdue_days_from
                                ,overdue_days_to
                                ,minimum_amount
                                ,maximum_amount
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
                                ,{p}IncentiveType
                                ,{p}EffDate
                                ,{p}IsActive
                                ,{p}IncentiveRatio
                                ,{p}RatePenalty
                                ,{p}OverdueDaysFrom
                                ,{p}OverdueDaysTo
                                ,{p}MinimumAmount
                                ,{p}MaximumAmount
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, IncentiveScheme model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_type			   =	{p}IncentiveType
                                ,eff_date		           = 	{p}EffDate
                                ,is_active		         = 	{p}IsActive
                                ,incentive_ratio		   = 	{p}IncentiveRatio
                                ,rate_penalty		       = 	{p}RatePenalty
                                ,overdue_days_from		 = 	{p}OverdueDaysFrom
                                ,overdue_days_to		   = 	{p}OverdueDaysTo
                                ,minimum_amount		     = 	{p}MinimumAmount
                                ,maximum_amount		     = 	{p}MaximumAmount
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

    public async Task<int> ChangeStatus(IDbTransaction transaction, IncentiveScheme model)
    {
      var p = db.Symbol();

      string query = $@"
												update {tableBase} 
												set
														is_active          = is_active * -1
														--
														,mod_date           = {p}ModDate
														,mod_by             = {p}ModBy
														,mod_ip_address     = {p}ModIPAddress
												where
														id = {p}ID";
      return await _command.Update(transaction, query, model);
    }

    public async Task<int> CountExistingScheme(IDbTransaction transaction, string incentiveType, DateTime? effDate, string? excludeID = null)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                count(id)
                          from
                              {tableBase}
                          where
                              lower(incentive_type) = lower({p}IncentiveType)
                              and eff_date = {p}EffDate
                  ";
                  if (!string.IsNullOrEmpty(excludeID))
                  {
                    query += $" and id != {p}ExcludeID";
                  }
      var parameters = new { IncentiveType = incentiveType, EffDate = effDate, ExcludeID = excludeID };

      var result = await _command.GetRow<int>(transaction, query, parameters);
      return result;
    }

    public async Task<DateTime> GetExistingSchemeEffDate(IDbTransaction transaction, string incentiveType, string? excludeID = null)
    {
      var p = db.Symbol();

      string query =
                  $@"  select
                              isnull(max(eff_date), cast('1900-01-01' as datetime)) AS EffDate
                        from
                            {tableBase}
                        where
                            lower(incentive_type) = lower({p}IncentiveType)
                ";
      
      dynamic parameters;
      
      if (!string.IsNullOrEmpty(excludeID))
      {
        query += $" and id != {p}ExcludeID";
        parameters = new { IncentiveType = incentiveType, ExcludeID = excludeID };
      }
      else
      {
        parameters = new { IncentiveType = incentiveType };
      }

      var result = await _command.GetRow<DateTime>(transaction, query, parameters);
      return result;
    }
  }
}