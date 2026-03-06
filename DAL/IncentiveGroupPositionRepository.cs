
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class IncentiveGroupPositionRepository : BaseRepository, IIncentiveGroupPositionRepository
  {
    private readonly string tableBase = "incentive_group_position";

    public async Task<IncentiveGroupPosition> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                    AS ID
                                ,incentive_group_id		AS	IncentiveGroupID
                                ,position_id			    AS	PositionID
                                ,position_code			  AS	PositionCode
                                ,position_description	AS	PositionDescription
                                ,position_ratio			  AS	PositionRatio
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<IncentiveGroupPosition>(transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveGroupPosition>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_group_id		AS	IncentiveGroupID
                              ,position_id			    AS	PositionID
                              ,position_code			  AS	PositionCode
                              ,position_description	AS	PositionDescription
                              ,position_ratio			  AS	PositionRatio
                        from 
                            {tableBase}
                        where 
                        (
                            lower(position_description)                                                 like lower({p}Keyword)
                            or lower(position_code)                                                     like lower({p}Keyword)
                            or lower(position_ratio)                                                    like lower({p}Keyword)
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

      var result = await _command.GetRows<IncentiveGroupPosition>
      (transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveGroupPosition>> GetRowsByGroupID(IDbTransaction transaction, string? keyword, int offset, int limit, string groupID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_group_id		AS	IncentiveGroupID
                              ,position_id			    AS	PositionID
                              ,position_code			  AS	PositionCode
                              ,position_description	AS	PositionDescription
                              ,position_ratio			  AS	PositionRatio
                        from 
                            {tableBase}
                        where 
                            incentive_group_id = {p}IncentiveGroupID";
      
      if (!string.IsNullOrEmpty(keyword))
      {
          query += $@"
                        and
                        (
                            lower(position_description) like lower({p}Keyword)
                            or lower(position_code) like lower({p}Keyword)
                            or lower(position_ratio) like lower({p}Keyword)
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

      var result = await _command.GetRows<IncentiveGroupPosition>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, IncentiveGroupPosition model)
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
                                ,position_id
                                ,position_code
                                ,position_description
                                ,position_ratio
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
                                ,{p}PositionID
                                ,{p}PositionCode
                                ,{p}PositionDescription
                                ,{p}PositionRatio
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, IncentiveGroupPosition model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_group_id		 =	{p}IncentiveGroupID
                                ,position_id			     =	{p}PositionID
                                ,position_code			   =	{p}PositionCode
                                ,position_description	 =	{p}PositionDescription
                                ,position_ratio		     =	{p}PositionRatio
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

    #region CheckDuplicatePosition
    public async Task<int> CheckDuplicatePosition(IDbTransaction transaction, string incentiveGroupID, string positionID, string exceptID)
    {
      string p = db.Symbol();
      string query = $@"
              select 
                count(igc.position_id) 
              from 
                {tableBase} igc
              where
                igc.incentive_group_id = {p}IncentiveGroupID
                and igc.position_id = {p}PositionID
                and igc.id != {p}ExceptID
              ";

      var parameters = new 
      { 
        IncentiveGroupID = incentiveGroupID,
        PositionID = positionID,
        ExceptID = exceptID
      };
      
      var result = await _command.GetRow<int?>(transaction, query, parameters);
      return result ?? 0;
    }
    #endregion
  }
}