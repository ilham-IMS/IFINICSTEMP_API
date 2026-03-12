using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
    public class InterfaceAgreementIncentiveMarketingRepository : BaseRepository, IInterfaceAgreementIncentiveMarketingRepository
    {
        private readonly string tableBase = "interface_agreement_incentive_marketing";

        #region GetRows
        public async Task<List<InterfaceAgreementIncentiveMarketing>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
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
                    application_main_id as ApplicationMainID,
                    agreement_no as AgreementNo,
                    client_id as ClientID,
                    client_no as ClientNo,
                    client_name as ClientName,
                    approved_date as ApprovedDate,
                    disbursement_date as DisbursementDate,
                    incentive_period as IncentivePeriod,
                    payment_method as PaymentMethod,
                    currency_id as CurrencyID,
                    currency_code as CurrencyCode,
                    currency_desc as CurrencyDesc,
                    net_finance as NetFinance,
                    interest_rate as InterestRate,
                    cost_rate as CostRate,
                    interest_margin as InterestMargin,
                    insurance_rate as InsuranceRate,
                    interest_amount as InterestAmount,
                    cost_amount as CostAmount,
                    interest_margin_amount as InterestMarginAmount,
                    total_insurance_premi_amount as TotalInsurancePremiAmount,
                    ccy_rate as CCYRate,
                    commission_rate as CommissionRate,
                    vendor_id as VendorID,
                    vendor_code as VendorCode,
                    vendor_name as VendorName,
                    agent_id as AgentID,
                    agent_code as AgentCode,
                    agent_name as AgentName,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where 
                    ({p}Keyword is null or lower(job_status) like lower({p}Keyword) 
                     or lower(error_message) like lower({p}Keyword))
                order by mod_date desc
            ";

            query = QueryLimitOffset(query);

            var parameters = new
            {
                Keyword = $"%{keyword}%",
                Offset = offset,
                Limit = limit
            };

            return await _command.GetRows<InterfaceAgreementIncentiveMarketing>(transaction, query, parameters);
        }
        #endregion

        #region GetRowByID
        public async Task<InterfaceAgreementIncentiveMarketing> GetRowByID(IDbTransaction transaction, string id)
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
                    application_main_id as ApplicationMainID,
                    agreement_no as AgreementNo,
                    client_id as ClientID,
                    client_no as ClientNo,
                    client_name as ClientName,
                    approved_date as ApprovedDate,
                    disbursement_date as DisbursementDate,
                    incentive_period as IncentivePeriod,
                    payment_method as PaymentMethod,
                    currency_id as CurrencyID,
                    currency_code as CurrencyCode,
                    currency_desc as CurrencyDesc,
                    net_finance as NetFinance,
                    interest_rate as InterestRate,
                    cost_rate as CostRate,
                    interest_margin as InterestMargin,
                    insurance_rate as InsuranceRate,
                    interest_amount as InterestAmount,
                    cost_amount as CostAmount,
                    interest_margin_amount as InterestMarginAmount,
                    total_insurance_premi_amount as TotalInsurancePremiAmount,
                    ccy_rate as CCYRate,
                    commission_rate as CommissionRate,
                    vendor_id as VendorID,
                    vendor_code as VendorCode,
                    vendor_name as VendorName,
                    agent_id as AgentID,
                    agent_code as AgentCode,
                    agent_name as AgentName,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where id = {p}ID
            ";

            var parameters = new { ID = id };
            return await _command.GetRow<InterfaceAgreementIncentiveMarketing>(transaction, query, parameters);
        }
        #endregion

        #region Insert
        public async Task<int> Insert(IDbTransaction transaction, InterfaceAgreementIncentiveMarketing model)
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
                    application_main_id,
                    agreement_no,
                    client_id,
                    client_no,
                    client_name,
                    approved_date,
                    disbursement_date,
                    incentive_period,
                    payment_method,
                    currency_id,
                    currency_code,
                    currency_desc,
                    net_finance,
                    interest_rate,
                    cost_rate,
                    interest_margin,
                    insurance_rate,
                    interest_amount,
                    cost_amount,
                    interest_margin_amount,
                    total_insurance_premi_amount,
                    ccy_rate,
                    commission_rate,
                    vendor_id,
                    vendor_code,
                    vendor_name,
                    agent_id,
                    agent_code,
                    agent_name,
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
                    {p}ApplicationMainID,
                    {p}AgreementNo,
                    {p}ClientID,
                    {p}ClientNo,
                    {p}ClientName,
                    {p}ApprovedDate,
                    {p}DisbursementDate,
                    {p}IncentivePeriod,
                    {p}PaymentMethod,
                    {p}CurrencyID,
                    {p}CurrencyCode,
                    {p}CurrencyDesc,
                    {p}NetFinance,
                    {p}InterestRate,
                    {p}CostRate,
                    {p}InterestMargin,
                    {p}InsuranceRate,
                    {p}InterestAmount,
                    {p}CostAmount,
                    {p}InterestMarginAmount,
                    {p}TotalInsurancePremiAmount,
                    {p}CCYRate,
                    {p}CommissionRate,
                    {p}VendorID,
                    {p}VendorCode,
                    {p}VendorName,
                    {p}AgentID,
                    {p}AgentCode,
                    {p}AgentName,
                    {p}JobStatus,
                    {p}ErrorMessage,
                    {p}StackTrace
                )
            ";

            return await _command.Insert(transaction, query, model);
        }
        #endregion

        #region UpdateByID
        public async Task<int> UpdateByID(IDbTransaction transaction, InterfaceAgreementIncentiveMarketing model)
        {
            var p = db.Symbol();

            string query = $@"
                update {tableBase}
                set
                    mod_date = {p}ModDate,
                    mod_by = {p}ModBy,
                    mod_ip_address = {p}ModIPAddress,
                    application_main_id = {p}ApplicationMainID,
                    agreement_no = {p}AgreementNo,
                    client_id = {p}ClientID,
                    client_no = {p}ClientNo,
                    client_name = {p}ClientName,
                    approved_date = {p}ApprovedDate,
                    disbursement_date = {p}DisbursementDate,
                    incentive_period = {p}IncentivePeriod,
                    payment_method = {p}PaymentMethod,
                    currency_id = {p}CurrencyID,
                    currency_code = {p}CurrencyCode,
                    currency_desc = {p}CurrencyDesc,
                    net_finance = {p}NetFinance,
                    interest_rate = {p}InterestRate,
                    cost_rate = {p}CostRate,
                    interest_margin = {p}InterestMargin,
                    insurance_rate = {p}InsuranceRate,
                    interest_amount = {p}InterestAmount,
                    cost_amount = {p}CostAmount,
                    interest_margin_amount = {p}InterestMarginAmount,
                    total_insurance_premi_amount = {p}TotalInsurancePremiAmount,
                    ccy_rate = {p}CCYRate,
                    commission_rate = {p}CommissionRate,
                    vendor_id = {p}VendorID,
                    vendor_code = {p}VendorCode,
                    vendor_name = {p}VendorName,
                    agent_id = {p}AgentID,
                    agent_code = {p}AgentCode,
                    agent_name = {p}AgentName,
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
        public async Task<List<InterfaceAgreementIncentiveMarketing>> GetRowsForJobIn(IDbTransaction transaction, int limit)
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
                    application_main_id as ApplicationMainID,
                    agreement_no as AgreementNo,
                    client_id as ClientID,
                    client_no as ClientNo,
                    client_name as ClientName,
                    approved_date as ApprovedDate,
                    disbursement_date as DisbursementDate,
                    incentive_period as IncentivePeriod,
                    payment_method as PaymentMethod,
                    currency_id as CurrencyID,
                    currency_code as CurrencyCode,
                    currency_desc as CurrencyDesc,
                    net_finance as NetFinance,
                    interest_rate as InterestRate,
                    cost_rate as CostRate,
                    interest_margin as InterestMargin,
                    insurance_rate as InsuranceRate,
                    interest_amount as InterestAmount,
                    cost_amount as CostAmount,
                    interest_margin_amount as InterestMarginAmount,
                    total_insurance_premi_amount as TotalInsurancePremiAmount,
                    ccy_rate as CCYRate,
                    commission_rate as CommissionRate,
                    vendor_id as VendorID,
                    vendor_code as VendorCode,
                    vendor_name as VendorName,
                    agent_id as AgentID,
                    agent_code as AgentCode,
                    agent_name as AgentName,
                    job_status as JobStatus,
                    error_message as ErrorMessage,
                    stack_trace as StackTrace
                from {tableBase}
                where job_status = 'HOLD'
                order by mod_date desc
            ";

            query = QueryLimit(query);

            var parameters = new { Limit = limit };
            return await _command.GetRows<InterfaceAgreementIncentiveMarketing>(transaction, query, parameters);
        }
        #endregion

        #region UpdateAfterJobIn
        public async Task<int> UpdateAfterJobIn(IDbTransaction transaction, InterfaceAgreementIncentiveMarketing model)
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
