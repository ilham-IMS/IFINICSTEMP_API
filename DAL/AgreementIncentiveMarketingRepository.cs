
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class AgreementIncentiveMarketingRepository : BaseRepository, IAgreementIncentiveMarketingRepository
  {
    private readonly string tableBase = "agreement_incentive_marketing";
    
    public async Task<AgreementIncentiveMarketing> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
    
      string query =
            $@"  select
                    aim.id                            AS ID
                    ,aim.incentive_marketing_id       AS IncentiveMarketingID
                    ,aim.application_main_id          AS ApplicationMainID
                    ,aim.agreement_no                 AS AgreementNo
                    ,aim.client_id                    AS ClientID
                    ,aim.client_no                    AS ClientNo
                    ,aim.client_name                  AS ClientName
                    ,aim.approved_date                AS ApprovedDate
                    ,aim.disbursement_date            AS DisbursementDate
                    ,aim.incentive_period             AS IncentivePeriod
                    ,aim.payment_method               AS PaymentMethod
                    ,aim.currency_id                  AS CurrencyID
                    ,aim.currency_code                AS CurrencyCode
                    ,aim.currency_desc                AS CurrencyDesc
                    ,aim.net_finance                  AS NetFinance
                    ,aim.interest_rate                AS InterestRate
                    ,aim.cost_rate                    AS CostRate
                    ,aim.interest_margin              AS InterestMargin
                    ,aim.insurance_rate               AS InsuranceRate
                    ,aim.interest_amount              AS InterestAmount
                    ,aim.cost_amount                  AS CostAmount
                    ,aim.interest_margin_amount       AS InterestMarginAmount
                    ,aim.total_insurance_premi_amount AS TotalInsurancePremiAmount
                    ,aim.ccy_rate                     AS CCYRate
                    ,aim.commission_rate              AS CommissionRate
                    ,aim.vendor_id                    AS VendorID
                    ,aim.vendor_code                  AS VendorCode
                    ,aim.vendor_name                  AS VendorName
                    ,aim.agent_id                     AS AgentID
                    ,aim.agent_code                   AS AgentCode
                    ,aim.agent_name                   AS AgentName
                    ,(select sum(ar.refund_amount) from agreement_refund ar where ar.agreement_incentive_id = aim.id) AS TotalRefundAmount
              from
                  {tableBase} aim
              where
                  aim.id = {p}ID
            ";
      var parameters = new { id };

      var result = await _command.GetRow<AgreementIncentiveMarketing>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementIncentiveMarketing>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                            AS ID
                              ,incentive_marketing_id       AS IncentiveMarketingID
                              ,application_main_id          AS ApplicationMainID
                              ,agreement_no                 AS AgreementNo
                              ,client_id                    AS ClientID
                              ,client_no                    AS ClientNo
                              ,client_name                  AS ClientName
                              ,approved_date                AS ApprovedDate
                              ,disbursement_date            AS DisbursementDate
                              ,incentive_period             AS IncentivePeriod
                              ,payment_method               AS PaymentMethod
                              ,currency_id                  AS CurrencyID
                              ,currency_code                AS CurrencyCode
                              ,currency_desc                AS CurrencyDesc
                              ,net_finance                  AS NetFinance
                              ,interest_rate                AS InterestRate
                              ,cost_rate                    AS CostRate
                              ,interest_margin              AS InterestMargin
                              ,insurance_rate               AS InsuranceRate
                              ,interest_amount              AS InterestAmount
                              ,cost_amount                  AS CostAmount
                              ,interest_margin_amount       AS InterestMarginAmount
                              ,total_insurance_premi_amount AS TotalInsurancePremiAmount
                              ,ccy_rate                     AS CCYRate
                              ,commission_rate              AS CommissionRate
                              ,vendor_id                    AS VendorID
                              ,vendor_code                  AS VendorCode
                              ,vendor_name                  AS VendorName
                              ,agent_id                     AS AgentID
                              ,agent_code                   AS AgentCode
                              ,agent_name                   AS AgentName
                        from 
                            {tableBase}
                        where 
                        (
                            lower(client_name)                                               like lower({p}Keyword)
                            or lower(agreement_no)                                               like lower({p}Keyword)
                            or cast({FormatDateTime($"approved_date")} as char(50))           like lower({p}Keyword)
                            or cast({FormatDateTime($"disbursement_date")} as char(50))           like lower({p}Keyword)
                            or lower({FormatNumeric("incentive_period")})                       like lower({p}Keyword)
                            or lower(payment_method)                                            like lower({p}Keyword)
                            or lower(currency_code)                                             like lower({p}Keyword)
                            or lower({FormatNumeric("net_finance")})                          like lower({p}Keyword)
                            or lower({FormatNumeric("interest_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("cost_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("interest_margin")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("insurance_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("interest_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("cost_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("interest_margin_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("total_insurance_premi_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("ccy_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("commission_rate")})                       like lower({p}Keyword)

                        )
                        order by
                        cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<AgreementIncentiveMarketing>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementIncentiveMarketing>> GetRowsByIncentiveID(IDbTransaction transaction, string? keyword, int offset, int limit, string incentiveID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              aim.id                            AS ID
                              ,aim.incentive_marketing_id       AS IncentiveMarketingID
                              ,aim.application_main_id          AS ApplicationMainID
                              ,aim.agreement_no                 AS AgreementNo
                              ,aim.client_id                    AS ClientID
                              ,aim.client_no                    AS ClientNo
                              ,aim.client_name                  AS ClientName
                              ,aim.approved_date                AS ApprovedDate
                              ,aim.disbursement_date            AS DisbursementDate
                              ,aim.incentive_period             AS IncentivePeriod
                              ,aim.payment_method               AS PaymentMethod
                              ,aim.currency_id                  AS CurrencyID
                              ,aim.currency_code                AS CurrencyCode
                              ,aim.currency_desc                AS CurrencyDesc
                              ,aim.net_finance                  AS NetFinance
                              ,aim.interest_rate                AS InterestRate
                              ,aim.cost_rate                    AS CostRate
                              ,aim.interest_margin              AS InterestMargin
                              ,aim.insurance_rate               AS InsuranceRate
                              ,aim.interest_amount              AS InterestAmount
                              ,aim.cost_amount                  AS CostAmount
                              ,aim.interest_margin_amount       AS InterestMarginAmount
                              ,aim.total_insurance_premi_amount AS TotalInsurancePremiAmount
                              ,aim.ccy_rate                     AS CCYRate
                              ,aim.commission_rate              AS CommissionRate
                              ,aim.vendor_id                    AS VendorID
                              ,aim.vendor_code                  AS VendorCode
                              ,aim.vendor_name                  AS VendorName
                              ,aim.agent_id                     AS AgentID
                              ,aim.agent_code                   AS AgentCode
                              ,aim.agent_name                   AS AgentName
                              ,(select sum(ar.refund_amount) from agreement_refund ar where ar.agreement_incentive_id = aim.id) AS TotalRefundAmount
                        from 
                            {tableBase} aim
                        where 
                            aim.incentive_marketing_id = {p}IncentiveID and
                        (
                            lower(aim.client_name)                                               like lower({p}Keyword)
                            or lower(aim.agreement_no)                                               like lower({p}Keyword)
                            or cast({FormatDateTime($"aim.approved_date")} as char(50))           like lower({p}Keyword)
                            or cast({FormatDateTime($"aim.disbursement_date")} as char(50))           like lower({p}Keyword)
                            or lower({FormatNumeric("aim.incentive_period")})                       like lower({p}Keyword)
                            or lower(aim.payment_method)                                            like lower({p}Keyword)
                            or lower(aim.currency_code)                                             like lower({p}Keyword)
                            or lower({FormatNumeric("aim.net_finance")})                          like lower({p}Keyword)
                            or lower({FormatNumeric("aim.interest_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.cost_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.interest_margin")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.insurance_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.interest_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.cost_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.interest_margin_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.total_insurance_premi_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.ccy_rate")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("aim.commission_rate")})                       like lower({p}Keyword)

                        )
                        order by
                        aim.cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        IncentiveID = incentiveID
      };

      var result = await _command.GetRows<AgreementIncentiveMarketing>(transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, AgreementIncentiveMarketing model)
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
                                ,incentive_marketing_id       
                                ,application_main_id          
                                ,agreement_no                 
                                ,client_id                    
                                ,client_no                    
                                ,client_name                  
                                ,approved_date                
                                ,disbursement_date            
                                ,incentive_period             
                                ,payment_method               
                                ,currency_id                  
                                ,currency_code                
                                ,currency_desc                
                                ,net_finance                  
                                ,interest_rate                
                                ,cost_rate                    
                                ,interest_margin              
                                ,insurance_rate               
                                ,interest_amount              
                                ,cost_amount                  
                                ,interest_margin_amount       
                                ,total_insurance_premi_amount 
                                ,ccy_rate                     
                                ,commission_rate
                                ,vendor_id
                                ,vendor_code
                                ,vendor_name
                                ,agent_id
                                ,agent_code
                                ,agent_name                                      
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
                                ,{p}IncentiveMarketingID
                                ,{p}ApplicationMainID
                                ,{p}AgreementNo
                                ,{p}ClientID
                                ,{p}ClientNo
                                ,{p}ClientName
                                ,{p}ApprovedDate
                                ,{p}DisbursementDate
                                ,{p}IncentivePeriod
                                ,{p}PaymentMethod
                                ,{p}CurrencyID
                                ,{p}CurrencyCode
                                ,{p}CurrencyDesc
                                ,{p}NetFinance
                                ,{p}InterestRate
                                ,{p}CostRate
                                ,{p}InterestMargin
                                ,{p}InsuranceRate
                                ,{p}InterestAmount
                                ,{p}CostAmount
                                ,{p}InterestMarginAmount
                                ,{p}TotalInsurancePremiAmount
                                ,{p}CCYRate
                                ,{p}CommissionRate
                                ,{p}VendorID
                                ,{p}VendorCode
                                ,{p}VendorName
                                ,{p}AgentID
                                ,{p}AgentCode
                                ,{p}AgentName
                                
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, AgreementIncentiveMarketing model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date                      ={p}ModDate
                                ,mod_by                       ={p}ModBy
                                ,mod_ip_address               ={p}ModIPAddress
                                ,incentive_marketing_id       ={p}IncentiveMarketingID
                                ,application_main_id          ={p}ApplicationMainID
                                ,agreement_no                 ={p}AgreementNo
                                ,client_id                    ={p}ClientID
                                ,client_no                    ={p}ClientNo
                                ,client_name                  ={p}ClientName
                                ,approved_date                ={p}ApprovedDate
                                ,disbursement_date            ={p}DisbursementDate
                                ,incentive_period             ={p}IncentivePeriod
                                ,payment_method               ={p}PaymentMethod
                                ,currency_id                  ={p}CurrencyID
                                ,currency_code                ={p}CurrencyCode
                                ,currency_desc                ={p}CurrencyDesc
                                ,net_finance                  ={p}NetFinance
                                ,interest_rate                ={p}InterestRate
                                ,cost_rate                    ={p}CostRate
                                ,interest_margin              ={p}InterestMargin
                                ,insurance_rate               ={p}InsuranceRate
                                ,interest_amount              ={p}InterestAmount
                                ,cost_amount                  ={p}CostAmount
                                ,interest_margin_amount       ={p}InterestMarginAmount
                                ,total_insurance_premi_amount ={p}TotalInsurancePremiAmount
                                ,ccy_rate                     ={p}CCYRate
                                ,commission_rate              ={p}CommissionRate
                                ,vendor_id                    ={p}VendorID
                                ,vendor_code                  ={p}VendorCode
                                ,vendor_name                  ={p}VendorName
                                ,agent_id                     ={p}AgentID
                                ,agent_code                   ={p}AgentCode
                                ,agent_name                   ={p}AgentName
                                
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

  }
}