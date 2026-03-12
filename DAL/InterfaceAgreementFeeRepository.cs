using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
    public class InterfaceAgreementFeeRepository : BaseRepository, IInterfaceAgreementFeeRepository
    {
        private readonly string tableBase = "interface_agreement_fee";
  
        #region GetRows
        public async Task<List<InterfaceAgreementFee>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
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
                    fee_id as FeeID,
                    fee_code as FeeCode,
                    fee_name as FeeName,
                    fee_amount as FeeAmount,
                    fee_rate as FeeRate,
                    fee_payment_type as FeePaymentType,
                    fee_paid_amount as FeePaidAmount,
                    fee_reduce_disburse_amount as FeeReduceDisburseAmount,
                    fee_capitalize_amount as FeeCapitalizeAmount,
                    insurance_year as InsuranceYear,
                    remarks as Remarks,
                    is_internal_income as IsInternalIncome,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    ({p}Keyword is null 
                     or lower(fee_code) like lower({p}Keyword) 
                     or lower(fee_name) like lower({p}Keyword))
                order by mod_date desc
            ";

            query = QueryLimitOffset(query);

            var parameters = new
            {
                Keyword = $"%{keyword}%",
                Offset = offset,
                Limit = limit
            };

            return await _command.GetRows<InterfaceAgreementFee>(transaction, query, parameters);
        }

        public async Task<List<InterfaceAgreementFee>> GetRows(IDbTransaction transaction, string agreementIncentiveID)
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
                    fee_id as FeeID,
                    fee_code as FeeCode,
                    fee_name as FeeName,
                    fee_amount as FeeAmount,
                    fee_rate as FeeRate,
                    fee_payment_type as FeePaymentType,
                    fee_paid_amount as FeePaidAmount,
                    fee_reduce_disburse_amount as FeeReduceDisburseAmount,
                    fee_capitalize_amount as FeeCapitalizeAmount,
                    insurance_year as InsuranceYear,
                    remarks as Remarks,
                    is_internal_income as IsInternalIncome,
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

            return await _command.GetRows<InterfaceAgreementFee>(transaction, query, parameters);
        }
        #endregion

        #region GetRowByID
        public async Task<InterfaceAgreementFee> GetRowByID(IDbTransaction transaction, string id)
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
                    fee_id as FeeID,
                    fee_code as FeeCode,
                    fee_name as FeeName,
                    fee_amount as FeeAmount,
                    fee_rate as FeeRate,
                    fee_payment_type as FeePaymentType,
                    fee_paid_amount as FeePaidAmount,
                    fee_reduce_disburse_amount as FeeReduceDisburseAmount,
                    fee_capitalize_amount as FeeCapitalizeAmount,
                    insurance_year as InsuranceYear,
                    remarks as Remarks,
                    is_internal_income as IsInternalIncome,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where id = {p}ID
            ";

            var parameters = new { ID = id };
            return await _command.GetRow<InterfaceAgreementFee>(transaction, query, parameters);
        }
        #endregion

        #region Insert
        public async Task<int> Insert(IDbTransaction transaction, InterfaceAgreementFee model)
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
                    fee_id,
                    fee_code,
                    fee_name,
                    fee_amount,
                    fee_rate,
                    fee_payment_type,
                    fee_paid_amount,
                    fee_reduce_disburse_amount,
                    fee_capitalize_amount,
                    insurance_year,
                    remarks,
                    is_internal_income,
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
                    {p}FeeID,
                    {p}FeeCode,
                    {p}FeeName,
                    {p}FeeAmount,
                    {p}FeeRate,
                    {p}FeePaymentType,
                    {p}FeePaidAmount,
                    {p}FeeReduceDisburseAmount,
                    {p}FeeCapitalizeAmount,
                    {p}InsuranceYear,
                    {p}Remarks,
                    {p}IsInternalIncome,
                    {p}JobStatus,
                    {p}ErrorMessage,
                    {p}StackTrace
                )
            ";

            return await _command.Insert(transaction, query, model);
        }
        #endregion

        #region UpdateByID
        public async Task<int> UpdateByID(IDbTransaction transaction, InterfaceAgreementFee model)
        {
            var p = db.Symbol();

            string query = $@"
                update {tableBase}
                set
                    mod_date = {p}ModDate,
                    mod_by = {p}ModBy,
                    mod_ip_address = {p}ModIPAddress,
                    agreement_incentive_id = {p}AgreementIncentiveID,
                    fee_id = {p}FeeID,
                    fee_code = {p}FeeCode,
                    fee_name = {p}FeeName,
                    fee_amount = {p}FeeAmount,
                    fee_rate = {p}FeeRate,
                    fee_payment_type = {p}FeePaymentType,
                    fee_paid_amount = {p}FeePaidAmount,
                    fee_reduce_disburse_amount = {p}FeeReduceDisburseAmount,
                    fee_capitalize_amount = {p}FeeCapitalizeAmount,
                    insurance_year = {p}InsuranceYear,
                    remarks = {p}Remarks,
                    is_internal_income = {p}IsInternalIncome,
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
