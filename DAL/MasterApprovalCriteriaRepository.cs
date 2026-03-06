using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class MasterApprovalCriteriaRepository : BaseRepository, IMasterApprovalCriteriaRepository
  {
    private readonly string tableBase = "master_approval_criteria";
    private readonly string tableMasterApproval = "master_approval";
    public async Task<List<MasterApprovalCriteria>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit, string ApprovalID)
    {
      var p = db.Symbol();

      string query =
      $@"
                select
									mac.id                              as ID
									,mac.approval_id                   as ApprovalID
									,mac.reff_criteria_id              as ReffCriteriaID
									,mac.reff_criteria_code            as ReffCriteriaCode
									,mac.reff_criteria_name            as ReffCriteriaName
									,mac.criteria_id                   as CriteriaID
									,mac.criteria_code                 as CriteriaCode
									,mac.criteria_description          as CriteriaDescription
                from
									{tableBase} mac
                where
                        approval_id = {p}ApprovalID
                and
                        (
                            lower(mac.reff_criteria_code)     like    lower({p}Keyword)
                            or lower(mac.reff_criteria_name)   like    lower({p}Keyword)
                            or lower(mac.criteria_description)        like    lower({p}Keyword)
                        )
                order by
                        mac.mod_date desc

            ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        ApprovalID = ApprovalID
      };

      var result = await _command.GetRows<MasterApprovalCriteria>(transaction, query, parameters);
      return result;
    }
    public async Task<List<MasterApprovalCriteria>> GetRowsByApprovalCode(IDbTransaction transaction, string approvalCode)
    {
      var p = db.Symbol();

      string query =
      $@"
                select
									mac.id                              as ID
									,mac.criteria_id                   as CriteriaID
									,mac.criteria_code                 as CriteriaCode
									,mac.criteria_description          as CriteriaDescription
                  ,mac.reff_criteria_id              as ReffCriteriaID
									,mac.reff_criteria_code            as ReffCriteriaCode
									,mac.reff_criteria_name            as ReffCriteriaName
                from
									{tableBase} mac
								inner join
									{tableMasterApproval} ma on ma.id = mac.approval_id and ma.code = {p}ApprovalCode
                where
											criteria_id is not null
                order by
                        mac.mod_date desc
            ";

      var parameters = new
      {
        ApprovalCode = approvalCode
      };

      var result = await _command.GetRows<MasterApprovalCriteria>(transaction, query, parameters);
      return result;
    }
    public Task<List<MasterApprovalCriteria>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      throw new NotImplementedException();
    }
    public async Task<MasterApprovalCriteria> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                    select
                            {tableBase}.id                              as ID
                            ,{tableBase}.approval_id                    as ApprovalID
                            ,{tableBase}.reff_criteria_id               as ReffCriteriaID
                            ,{tableBase}.reff_criteria_code             as ReffCriteriaCode
                            ,{tableBase}.reff_criteria_name             as ReffCriteriaName
                            ,{tableBase}.Criteria_id                    as CriteriaID
                            ,{tableBase}.Criteria_code                   as CriteriaCode
                            ,{tableBase}.Criteria_description            as CriteriaDescription
                    from
                            {tableBase}
                    where
                            {tableBase}.id = {p}ID
            ";
      var parameters = new
      {
        ID = id
      };

      var result = await _command.GetRow<MasterApprovalCriteria>(transaction, query, parameters);
      return result;
    }
    public async Task<int> Insert(IDbTransaction transaction, MasterApprovalCriteria model)
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
                    ,approval_id
                    ,reff_criteria_id
                    ,reff_criteria_code
                    ,reff_criteria_name
                    ,criteria_id 
                    ,criteria_code 
                    ,criteria_description 
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
                    ,{p}ApprovalID
                    ,{p}ReffCriteriaID
                    ,{p}ReffCriteriaCode
                    ,{p}ReffCriteriaName
                    ,{p}CriteriaID
                    ,{p}CriteriaCode
                    ,{p}CriteriaDescription
                )";
      return await _command.Insert(transaction, query, model);
    }
    public async Task<int> UpdateByID(IDbTransaction transaction, MasterApprovalCriteria model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date                    =   {p}ModDate
                                ,mod_by                     =   {p}ModBy
                                ,mod_ip_address             =   {p}ModIPAddress
                                ,criteria_id                =   {p}CriteriaID
                                ,criteria_code              =   {p}CriteriaCode
                                ,criteria_description       =   {p}CriteriaDescription
                            where
                                id                  = {p}ID";
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

    public async Task<int> UpdateReferenceByID(IDbTransaction transaction, MasterApprovalCriteria model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
																reff_criteria_id          =   {p}ReffCriteriaID
																,reff_criteria_code        =   {p}ReffCriteriaCode
                                ,reff_criteria_name        =   {p}ReffCriteriaName
                                --
                                ,mod_date                    =  {p}ModDate
                                ,mod_by                     =   {p}ModBy
                                ,mod_ip_address             =   {p}ModIPAddress
                            where
                                id                  = {p}ID";
      return await _command.Update(transaction, query, model);
    }

    public async Task<int> DeleteByNotInReffCriteria(IDbTransaction transaction, string[] reffCriteriaID, string ApprovalID)
    {
      var p = db.Symbol();

      if (reffCriteriaID.Length == 0) return 0;

      string query = $@"
                            delete from {tableBase}
                            where
                            approval_id = {p}ApprovalID
                            --and
                                --reff_criteria_id not in ({string.Join(",", reffCriteriaID.Select((x, i) => $"{p}CriteriaID"))})
														
																";

      var parameters = new Dictionary<string, object>(){
        {"ApprovalID", ApprovalID}
      };

      for (int i = 0; i < reffCriteriaID.Length; i++)
      {
        parameters.Add($"CriteriaID", reffCriteriaID[i]);
      }
      return await _command.Delete(transaction, query, parameters);

    }

    public async Task<int> DeleteAllByApprovalID(IDbTransaction transaction, string ApprovalID)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                            approval_id = {p}ApprovalID
																";
      return await _command.Delete(transaction, query, new { ApprovalID = ApprovalID });

    }


    public async Task<List<MasterApprovalCriteria>> GetRowsByReffCriteria(IDbTransaction transaction, string[] reffCriteriaID, string ApprovalID)
    {
      var p = db.Symbol();

      if (reffCriteriaID.Length == 0) return [];

      string query =
      $@"
                select
									mac.id                              as ID
                from
									{tableBase} mac
                where
                    approval_id = {p}ApprovalID
                    and
                        reff_criteria_id not in ({string.Join(",", reffCriteriaID.Select((x, i) => $"{p}CriteriaID{i}"))})

            ";

      var parameters = new Dictionary<string, object>(){
        {"ApprovalID", ApprovalID}
      };

      for (int i = 0; i < reffCriteriaID.Length; i++)
      {
        parameters.Add($"CriteriaID{i}", reffCriteriaID[i]);
      }

      var result = await _command.GetRows<MasterApprovalCriteria>(transaction, query, parameters);
      return result;
    }
  }
}