
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class IncentiveGroupCriteriaRepository : BaseRepository, IIncentiveGroupCriteriaRepository
  {
    private readonly string tableBase = "incentive_group_criteria";

    public async Task<IncentiveGroupCriteria> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                    AS ID
                                ,incentive_group_id		AS	IncentiveGroupID
                                ,criteria_id			    AS	CriteriaID
                                ,criteria_code			  AS	CriteriaCode
                                ,criteria_description	AS	CriteriaDescription
                                ,criteria_operator		AS	CriteriaOperator
                                ,value_from				AS	ValueFrom
                                ,value_to				AS	ValueTo
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<IncentiveGroupCriteria>(transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveGroupCriteria>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_group_id		AS	IncentiveGroupID
                              ,criteria_id			    AS	CriteriaID
                              ,criteria_code			  AS	CriteriaCode
                              ,criteria_description	AS	CriteriaDescription
                              ,criteria_operator		AS	CriteriaOperator
                              ,value_from				AS	ValueFrom
                              ,value_to				AS	ValueTo
                              
                        from 
                            {tableBase}
                        where 
                        (
                            lower(criteria_description)                                                 like lower({p}Keyword)
                            or lower(criteria_operator)                                                 like lower({p}Keyword)
                            or lower(value_from)                                                        like lower({p}Keyword)
                            or lower(value_to)                                                          like lower({p}Keyword)
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

      var result = await _command.GetRows<IncentiveGroupCriteria>
      (transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveGroupCriteria>> GetRowsByGroupID(IDbTransaction transaction, string? keyword, int offset, int limit, string groupID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_group_id		AS	IncentiveGroupID
                              ,criteria_id			    AS	CriteriaID
                              ,criteria_code			  AS	CriteriaCode
                              ,criteria_description	AS	CriteriaDescription
                              ,criteria_operator		AS	CriteriaOperator
                              ,value_from				AS	ValueFrom
                              ,value_to				AS	ValueTo
                              
                        from 
                            {tableBase}
                        where 
                            incentive_group_id = {p}IncentiveGroupID";
      
      if (!string.IsNullOrEmpty(keyword))
      {
          query += $@"
                        and
                        (
                            lower(criteria_description) like lower({p}Keyword)
                            or lower(criteria_operator) like lower({p}Keyword)
                            or lower(value_from) like lower({p}Keyword)
                            or lower(value_to) like lower({p}Keyword)
                        )
                        ";
      }

      query += " order by cre_date asc";
                        
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = string.IsNullOrEmpty(keyword) ? "%" : $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        IncentiveGroupID = groupID
      };

      var result = await _command.GetRows<IncentiveGroupCriteria>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, IncentiveGroupCriteria model)
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
                                ,incentive_group_id
                                ,criteria_id
                                ,criteria_code
                                ,criteria_description
                                ,criteria_operator
                                ,value_from
                                ,value_to
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
                                ,{p}IncentiveGroupID
                                ,{p}CriteriaID
                                ,{p}CriteriaCode
                                ,{p}CriteriaDescription
                                ,{p}CriteriaOperator
                                ,{p}ValueFrom
                                ,{p}ValueTo
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, IncentiveGroupCriteria model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_group_id		 =	{p}IncentiveGroupID
                                ,criteria_id			     =	{p}CriteriaID
                                ,criteria_code			   =	{p}CriteriaCode
                                ,criteria_description	 =	{p}CriteriaDescription
                                ,criteria_operator		 =	{p}CriteriaOperator
                                ,value_from				     =	{p}ValueFrom
                                ,value_to				       =	{p}ValueTo
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

    #region CheckDuplicateCriteria
    public async Task<int> CheckDuplicateCriteria(IDbTransaction transaction, string incentiveGroupID, string criteriaID, string exceptID)
    {
      string p = db.Symbol();
      string query = $@"
              select 
                count(igc.criteria_id) 
              from 
                {tableBase} igc
              where
                igc.incentive_group_id = {p}IncentiveGroupID
                and igc.criteria_id = {p}CriteriaID
                and igc.id != {p}ExceptID
              ";

      var parameters = new 
      { 
        IncentiveGroupID = incentiveGroupID,
        CriteriaID = criteriaID,
        ExceptID = exceptID
      };
      
      var result = await _command.GetRow<int?>(transaction, query, parameters);
      return result ?? 0;
    }
    #endregion
  }
}