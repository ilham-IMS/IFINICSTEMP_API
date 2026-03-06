using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
    public class InterfaceMasterApprovalCriteriaRepository : BaseRepository, IInterfaceMasterApprovalCriteriaRepository
    {
        private readonly string tableBase = "interface_master_approval_criteria";

        #region GetRows
        public async Task<List<InterfaceMasterApprovalCriteria>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
        {
            var p = db.Symbol();

            string query = $@"
                select
                    id as ID,
                    cre_date as CreDate,
                    cre_by as CreBy,
                    cre_ip_address as CreIPAddress,
                    mod_date as ModDate,
                    mod_by as ModBy,
                    mod_ip_address as ModIPAddress,
                    criteria_id as CriteriaID,
                    criteria_code as CriteriaCode,
                    criteria_description as CriteriaDescription,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    ({p}Keyword is null 
                     or lower(criteria_code) like lower({p}Keyword) 
                     or lower(criteria_description) like lower({p}Keyword))
                order by mod_date desc
            ";

            query = QueryLimitOffset(query);

            var parameters = new
            {
                Keyword = $"%{keyword}%",
                Offset = offset,
                Limit = limit
            };

            return await _command.GetRows<InterfaceMasterApprovalCriteria>(transaction, query, parameters);
        }

        public async Task<List<InterfaceMasterApprovalCriteria>> GetRows(IDbTransaction transaction, string iApprovalID)
        {
            var p = db.Symbol();

            string query = $@"
                select
                    id as ID,
                    cre_date as CreDate,
                    cre_by as CreBy,
                    cre_ip_address as CreIPAddress,
                    mod_date as ModDate,
                    mod_by as ModBy,
                    mod_ip_address as ModIPAddress,
                    criteria_id as CriteriaID,
                    criteria_code as CriteriaCode,
                    criteria_description as CriteriaDescription,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    interface_approval_id = {p}IApprovalID
                order by mod_date desc
            ";

            var parameters = new
            {
                IApprovalID = iApprovalID
            };

            return await _command.GetRows<InterfaceMasterApprovalCriteria>(transaction, query, parameters);
        }
        #endregion

        #region GetRowByID
        public async Task<InterfaceMasterApprovalCriteria> GetRowByID(IDbTransaction transaction, string id)
        {
            var p = db.Symbol();

            string query = $@"
                select
                    id as ID,
                    cre_date as CreDate,
                    cre_by as CreBy,
                    cre_ip_address as CreIPAddress,
                    mod_date as ModDate,
                    mod_by as ModBy,
                    mod_ip_address as ModIPAddress,
                    criteria_id as CriteriaID,
                    criteria_code as CriteriaCode,
                    criteria_description as CriteriaDescription,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where id = {p}ID
            ";

            var parameters = new { ID = id };
            return await _command.GetRow<InterfaceMasterApprovalCriteria>(transaction, query, parameters);
        }
        #endregion

        #region Insert
        public async Task<int> Insert(IDbTransaction transaction, InterfaceMasterApprovalCriteria model)
        {
            var p = db.Symbol();

            string query = $@"
                insert into {tableBase}
                (
                    id,
                    cre_date,
                    cre_by,
                    cre_ip_address,
                    mod_date,
                    mod_by,
                    mod_ip_address,
                    criteria_id,
                    criteria_code,
                    criteria_description,
                    job_status,
                    error_message,
                    stack_trace,
                    interface_approval_id
                )
                values
                (
                    {p}ID,
                    {p}CreDate,
                    {p}CreBy,
                    {p}CreIPAddress,
                    {p}ModDate,
                    {p}ModBy,
                    {p}ModIPAddress,
                    {p}CriteriaID,
                    {p}CriteriaCode,
                    {p}CriteriaDescription,
                    {p}JobStatus,
                    {p}ErrorMessage,
                    {p}StackTrace,
                    {p}InterfaceApprovalID
                )
            ";

            return await _command.Insert(transaction, query, model);
        }
        #endregion

        #region UpdateByID
        public async Task<int> UpdateByID(IDbTransaction transaction, InterfaceMasterApprovalCriteria model)
        {
            var p = db.Symbol();

            string query = $@"
                update {tableBase}
                set
                    mod_date = {p}ModDate,
                    mod_by = {p}ModBy,
                    mod_ip_address = {p}ModIPAddress,
                    criteria_id = {p}CriteriaID,
                    criteria_code = {p}CriteriaCode,
                    criteria_description = {p}CriteriaDescription,
                    job_status = {p}JobStatus,
                    error_message = {p}ErrorMessage,
                    stack_trace = {p}StackTrace
                where id = {p}ID
            ";

            return await _command.Update(transaction, query, model);
        }
        #endregion

        #region DeleteByID
        public async Task<int> DeleteByID(IDbTransaction transaction, string id)
        {
            var p = db.Symbol();
            string query = $@"delete from {tableBase} where id = {p}ID";
            return await _command.DeleteByID(transaction, query, id);
        }
        #endregion
    }
}
