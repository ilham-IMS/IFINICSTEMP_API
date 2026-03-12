using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
    public class InterfaceAgreementRefundRepository : BaseRepository, IInterfaceAgreementRefundRepository
    {
        private readonly string tableBase = "interface_agreement_refund";
  
        #region GetRows
        public async Task<List<InterfaceAgreementRefund>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
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
                    agreement_incentive_id as AgreementIncentiveID,
                    refund_id as RefundID,
                    refund_code as RefundCode,
                    refund_desc as RefundDesc,
                    refund_amount as RefundAmount,
                    refund_rate as RefundRate,
                    calculate_by as CalculateBy,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    ({p}Keyword is null 
                     or lower(refund_code) like lower({p}Keyword) 
                     or lower(refund_desc) like lower({p}Keyword))
                order by mod_date desc
            ";

            query = QueryLimitOffset(query);

            var parameters = new
            {
                Keyword = $"%{keyword}%",
                Offset = offset,
                Limit = limit
            };

            return await _command.GetRows<InterfaceAgreementRefund>(transaction, query, parameters);
        }

        public async Task<List<InterfaceAgreementRefund>> GetRows(IDbTransaction transaction, string agreementIncentiveID)
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
                    agreement_incentive_id as AgreementIncentiveID,
                    refund_id as RefundID,
                    refund_code as RefundCode,
                    refund_desc as RefundDesc,
                    refund_amount as RefundAmount,
                    refund_rate as RefundRate,
                    calculate_by as CalculateBy,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    agreement_incentive_id = {p}AgreementIncentiveID
                order by mod_date desc
            ";

            var parameters = new
            {
                AgreementIncentiveID = agreementIncentiveID
            };

            return await _command.GetRows<InterfaceAgreementRefund>(transaction, query, parameters);
        }
        #endregion

        #region GetRowByID
        public async Task<InterfaceAgreementRefund> GetRowByID(IDbTransaction transaction, string id)
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
                    agreement_incentive_id as AgreementIncentiveID,
                    refund_id as RefundID,
                    refund_code as RefundCode,
                    refund_desc as RefundDesc,
                    refund_amount as RefundAmount,
                    refund_rate as RefundRate,
                    calculate_by as CalculateBy,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where id = {p}ID
            ";

            var parameters = new { ID = id };
            return await _command.GetRow<InterfaceAgreementRefund>(transaction, query, parameters);
        }
        #endregion

        #region Insert
        public async Task<int> Insert(IDbTransaction transaction, InterfaceAgreementRefund model)
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
                    agreement_incentive_id,
                    refund_id,
                    refund_code,
                    refund_desc,
                    refund_amount,
                    refund_rate,
                    calculate_by,
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
                    {p}AgreementIncentiveID,
                    {p}RefundID,
                    {p}RefundCode,
                    {p}RefundDesc,
                    {p}RefundAmount,
                    {p}RefundRate,
                    {p}CalculateBy,
                    {p}JobStatus,
                    {p}ErrorMessage,
                    {p}StackTrace
                )
            ";

            return await _command.Insert(transaction, query, model);
        }
        #endregion

        #region UpdateByID
        public async Task<int> UpdateByID(IDbTransaction transaction, InterfaceAgreementRefund model)
        {
            var p = db.Symbol();

            string query = $@"
                update {tableBase}
                set
                    mod_date = {p}ModDate,
                    mod_by = {p}ModBy,
                    mod_ip_address = {p}ModIPAddress,
                    agreement_incentive_id = {p}AgreementIncentiveID,
                    refund_id = {p}RefundID,
                    refund_code = {p}RefundCode,
                    refund_desc = {p}RefundDesc,
                    refund_amount = {p}RefundAmount,
                    refund_rate = {p}RefundRate,
                    calculate_by = {p}CalculateBy,
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
