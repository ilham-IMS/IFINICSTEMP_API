
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class IncentiveGroupRepository : BaseRepository, IIncentiveGroupRepository
  {
    private readonly string tableBase = "incentive_group";

    public async Task<IncentiveGroup> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                    AS ID
                                ,incentive_type			  AS	IncentiveType
                                ,group_description		AS	GroupDescription
                                ,is_active				    AS	IsActive
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<IncentiveGroup>(transaction, query, parameters);
      return result;
    }

    public async Task<List<IncentiveGroup>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                    AS ID
                              ,incentive_type			  AS	IncentiveType
                              ,group_description		AS	GroupDescription
                              ,is_active				    AS	IsActive
                              
                        from 
                            {tableBase}
                        where 
                        (
                            lower(incentive_type)                                                 like lower({p}Keyword)
                            or lower(group_description)                                           like lower({p}Keyword)
                            or case is_active when 1 then 'yes' else 'no' end                     like lower({p}Keyword)
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

      var result = await _command.GetRows<IncentiveGroup>
      (transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, IncentiveGroup model)
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
                                ,group_description
                                ,is_active
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
                                ,{p}GroupDescription
                                ,{p}IsActive
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, IncentiveGroup model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_type			   =	{p}IncentiveType
                                ,group_description		 =	{p}GroupDescription
                                ,is_active		         = 	{p}IsActive
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

    public async Task<int> ChangeStatus(IDbTransaction transaction, IncentiveGroup model)
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

    public async Task<int> CountExistingGroup(IDbTransaction transaction, string incentiveType, string groupDescription)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              count(id)
                              
                        from 
                            {tableBase}
                        where 
                            lower(incentive_type) = lower({p}IncentiveType)
                            and lower(group_description) = lower({p}GroupDescription)
                        ";

      var parameters = new
      {
        IncentiveType = incentiveType,
        GroupDescription = groupDescription
      };

      var result = await _command.GetRow<int>
      (transaction, query, parameters);
      return result;
    }
  }
}