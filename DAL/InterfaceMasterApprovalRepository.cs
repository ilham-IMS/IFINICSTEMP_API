using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
    public class InterfaceMasterApprovalRepository : BaseRepository, IInterfaceMasterApprovalRepository
    {
        private readonly string tableBase = "interface_master_approval";

        #region GetRows
        public async Task<List<InterfaceMasterApproval>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
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
                    approval_category_id as ApprovalCategoryID,
                    approval_category_code as ApprovalCategoryCode,
                    approval_category_name as ApprovalCategoryName,
                    reff_module_code as ReffModuleCode,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    ({p}Keyword is null or lower(code) like lower({p}Keyword) 
                     or lower(approval_name) like lower({p}Keyword))
                order by mod_date desc
            ";

            query = QueryLimitOffset(query);

            var parameters = new
            {
                Keyword = $"%{keyword}%",
                Offset = offset,
                Limit = limit
            };

            return await _command.GetRows<InterfaceMasterApproval>(transaction, query, parameters);
        }
        #endregion

        #region GetRowByID
        public async Task<InterfaceMasterApproval> GetRowByID(IDbTransaction transaction, string id)
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
                    approval_category_id as ApprovalCategoryID,
                    approval_category_code as ApprovalCategoryCode,
                    approval_category_name as ApprovalCategoryName,
                    reff_module_code as ReffModuleCode,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where id = {p}ID
            ";

            var parameters = new { ID = id };
            return await _command.GetRow<InterfaceMasterApproval>(transaction, query, parameters);
        }
        #endregion

        #region Insert
        public async Task<int> Insert(IDbTransaction transaction, InterfaceMasterApproval model)
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
                    approval_category_id,
                    approval_category_code,
                    approval_category_name,
                    reff_module_code,
                    job_status,
                    error_message,
                    stack_trace
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
                    {p}ApprovalCategoryID,
                    {p}ApprovalCategoryCode,
                    {p}ApprovalCategoryName,
                    {p}ReffModuleCode,
                    {p}JobStatus,
                    {p}ErrorMessage,
                    {p}StackTrace
                )
            ";

            return await _command.Insert(transaction, query, model);
        }
        #endregion

        #region UpdateByID
        public async Task<int> UpdateByID(IDbTransaction transaction, InterfaceMasterApproval model)
        {
            var p = db.Symbol();

            string query = $@"
                update {tableBase}
                set
                    mod_date = {p}ModDate,
                    mod_by = {p}ModBy,
                    mod_ip_address = {p}ModIPAddress,
                    approval_category_id = {p}ApprovalCategoryID,
                    approval_category_code = {p}ApprovalCategoryCode,
                    approval_category_name = {p}ApprovalCategoryName,
                    reff_module_code = {p}ReffModuleCode,
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

        #region GetRowsForJobIn
        public async Task<List<InterfaceMasterApproval>> GetRowsForJobIn(IDbTransaction transaction, int limit)
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
                    approval_category_id as ApprovalCategoryID,
                    approval_category_code as ApprovalCategoryCode,
                    approval_category_name as ApprovalCategoryName,
                    reff_module_code as ReffModuleCode,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where job_status = 'HOLD'
                order by mod_date desc
            ";

            query = QueryLimit(query);

            var parameters = new { Limit = limit };
            return await _command.GetRows<InterfaceMasterApproval>(transaction, query, parameters);
        }
        #endregion

        #region UpdateAfterJobIn
        public async Task<int> UpdateAfterJobIn(IDbTransaction transaction, InterfaceMasterApproval model)
        {
            var p = db.Symbol();

            string query = $@"
                update {tableBase}
                set
                    job_status = {p}JobStatus,
                    error_message = {p}ErrorMessage,
                    stack_trace = {p}StackTrace
                where id = {p}ID
            ";

            return await _command.Update(transaction, query, model);
        }
        #endregion

        #region CountDataByCode
        public async Task<int> CountDataByCode(IDbTransaction transaction, string code)
        {
            var p = db.Symbol();

            string query = $@"
                select count(1)
                from {tableBase}
                where code = {p}Code
            ";

            var parameters = new { Code = code };
            return await _command.GetRow<int?>(transaction, query, parameters) ?? 0;
        }
        #endregion
    }
}
