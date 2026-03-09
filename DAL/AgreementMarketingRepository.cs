
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class AgreementMarketingRepository : BaseRepository, IAgreementMarketingRepository
  {
    private readonly string tableBase = "agreement_marketing";

    public async Task<AgreementMarketing> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                         AS ID
                                ,incentive_marketing_id    AS IncentiveMarketingID
                                ,agreement_id              AS AgreementID
                                ,agreement_no              AS AgreementNo
                                ,client_id			           AS	ClientID
                                ,client_no			           AS	ClientNo
                                ,client_name		           AS	ClientName
                                ,approved_date             AS ApprovedDate
                                ,disbursement_date         AS DisbursementDate
                                ,incentive_period	       AS IncentivePeriod
                                ,payment_method            AS PaymentMethod
                                ,currency_id               AS CurrencyID
                                ,currency_code             AS CurrencyCode
                                ,currency_desc             AS CurrencyDesc
                                ,net_finance               AS NetFinance
                                ,interest_rate             AS InterestRate
                                ,cost_rate                 AS CostRate
                                ,interest_margin           AS InterestMargin
                                ,bpe_ratio                 AS BPERatio
                                ,insurance_premium_usage_ratio AS InsurancePremiumUsageRatio
                                ,bpe_effect                AS BPEEffect
                                ,bpe_income                AS BPEIncome
                                ,incentive_expense         AS IncentiveExpense
                                ,non_interest_effect       AS NonInterestEffect
                                ,profit_before_marketing_incentive AS ProfitBeforeMarketingIncentive
                                ,incentive_amount          AS IncentiveAmount
                                ,marketing_incentive_ratio AS MarketingIncentiveRatio
                                ,finance_amount            AS FinanceAmount
                                ,insurance_rate             AS InsuranceRate
                                ,interest_amount            AS InterestAmount
                                ,cost_amount                AS CostAmount
                                ,interest_margin_amount     AS InterestMarginAmount
                                ,ccy_rate                   AS CCYRate
                                ,bpe_total                  AS BPETotal
                                ,bpe_total_amount           AS BPETotalAmount
                                ,non_interest_name          AS NonInterestName
                                ,non_interest_expense       AS NonInterestExpense
                                ,non_interest_effect_amount AS NonInterestEffectAmount
                                 -- Calculated Fields
                                ,CASE WHEN profit_before_marketing_incentive <> 0 THEN interest_margin / profit_before_marketing_incentive ELSE 0 END AS InterestMarginProfitBeforeMarketingIncentive
                                ,CASE WHEN incentive_amount <> 0 THEN marketing_incentive_ratio / incentive_amount ELSE 0 END AS MarketingIncentiveRatioIncentiveAmount
                                ,CASE WHEN finance_amount <> 0 THEN marketing_incentive_ratio / finance_amount ELSE 0 END AS MarketingIncentiveRatioFinanceAmount
                                ,CASE WHEN incentive_expense <> 0 THEN bpe_income / incentive_expense ELSE 0 END AS BPEIncomeIncentiveExpense
                                ,(interest_margin - cost_rate)   AS NetInterestMarginAfterCost
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<AgreementMarketing>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementMarketing>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,incentive_marketing_id    AS IncentiveMarketingID
                              ,agreement_id              AS AgreementID
                              ,agreement_no              AS AgreementNo
                              ,client_id			           AS	ClientID
                              ,client_no			           AS	ClientNo
                              ,client_name		           AS	ClientName
                              ,approved_date             AS ApprovedDate
                              ,disbursement_date         AS DisbursementDate
                              ,incentive_period	         AS IncentivePeriod
                              ,payment_method            AS PaymentMethod
                              ,currency_id               AS CurrencyID
                              ,currency_code             AS CurrencyCode
                              ,currency_desc             AS CurrencyDesc
                              ,net_finance               AS NetFinance
                              ,interest_rate             AS InterestRate
                              ,cost_rate                 AS CostRate
                              ,interest_margin           AS InterestMargin
                              ,bpe_ratio                 AS BPERatio
                              ,insurance_premium_usage_ratio AS InsurancePremiumUsageRatio
                              ,bpe_effect                AS BPEEffect
                              ,bpe_income                AS BPEIncome
                              ,incentive_expense         AS IncentiveExpense
                              ,non_interest_effect       AS NonInterestEffect
                              ,profit_before_marketing_incentive AS ProfitBeforeMarketingIncentive
                              ,incentive_amount          AS IncentiveAmount
                              ,marketing_incentive_ratio AS MarketingIncentiveRatio
                              ,finance_amount            AS FinanceAmount
                              ,insurance_rate             AS InsuranceRate
                              ,interest_amount            AS InterestAmount
                              ,cost_amount                AS CostAmount
                              ,interest_margin_amount     AS InterestMarginAmount
                              ,ccy_rate                   AS CCYRate
                              ,bpe_total                  AS BPETotal
                              ,bpe_total_amount           AS BPETotalAmount
                              ,non_interest_name          AS NonInterestName
                              ,non_interest_expense       AS NonInterestExpense
                              ,non_interest_effect_amount AS NonInterestEffectAmount
                              -- Calculated Fields
                              ,CASE WHEN profit_before_marketing_incentive <> 0 THEN interest_margin / profit_before_marketing_incentive ELSE 0 END AS InterestMarginProfitBeforeMarketingIncentive
                              ,CASE WHEN incentive_amount <> 0 THEN marketing_incentive_ratio / incentive_amount ELSE 0 END AS MarketingIncentiveRatioIncentiveAmount
                              ,CASE WHEN finance_amount <> 0 THEN marketing_incentive_ratio / finance_amount ELSE 0 END AS MarketingIncentiveRatioFinanceAmount
                              ,CASE WHEN incentive_expense <> 0 THEN bpe_income / incentive_expense ELSE 0 END AS BPEIncomeIncentiveExpense
                              ,(interest_margin - cost_rate)   AS NetInterestMarginAfterCost
                        from 
                            {tableBase}
                        where 
                        (
                            lower(agreement_no)                                               like lower({p}Keyword)
                            or lower(client_name)                                             like lower({p}Keyword)
                            or cast({FormatDateTime($"approved_date")} as char(50))           like lower({p}Keyword)
                            or cast({FormatDateTime($"disbursement_date")} as char(50))        like lower({p}Keyword)
                            or lower({FormatNumeric("incentive_period")})                      like lower({p}Keyword)
                            or lower(payment_method)                                           like lower({p}Keyword)
                            or lower(currency_code)                                           like lower({p}Keyword)
                            or lower({FormatNumeric("net_finance")})                           like lower({p}Keyword)
                            or lower({FormatNumeric("interest_rate")})                         like lower({p}Keyword)
                            or lower({FormatNumeric("cost_rate")})                             like lower({p}Keyword)
                            or lower({FormatNumeric("interest_margin")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("bpe_ratio")})                             like lower({p}Keyword)
                            or lower({FormatNumeric("insurance_premium_usage_ratio")})         like lower({p}Keyword)
                            or lower({FormatNumeric("bpe_effect")})                             like lower({p}Keyword)
                            or lower({FormatNumeric("bpe_income")})                             like lower({p}Keyword)
                            or lower({FormatNumeric("incentive_expense")})                     like lower({p}Keyword)
                            or lower({FormatNumeric("non_interest_effect")})                   like lower({p}Keyword)
                            or lower({FormatNumeric("profit_before_marketing_incentive")})     like lower({p}Keyword)
                            or lower({FormatNumeric("incentive_amount")})                      like lower({p}Keyword)
                            or lower({FormatNumeric("marketing_incentive_ratio")})             like lower({p}Keyword)
                            or lower({FormatNumeric("finance_amount")})                        like lower({p}Keyword)
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

      var result = await _command.GetRows<AgreementMarketing>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementMarketing>> GetRowsByIncentiveID(IDbTransaction transaction, string? keyword, int offset, int limit, string incentiveMarketingID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,incentive_marketing_id    AS IncentiveMarketingID
                              ,agreement_id              AS AgreementID
                              ,agreement_no              AS AgreementNo
                              ,client_id			           AS	ClientID
                              ,client_no			           AS	ClientNo
                              ,client_name		           AS	ClientName
                              ,approved_date             AS ApprovedDate
                              ,disbursement_date         AS DisbursementDate
                              ,incentive_period	         AS IncentivePeriod
                              ,payment_method            AS PaymentMethod
                              ,currency_id               AS CurrencyID
                              ,currency_code             AS CurrencyCode
                              ,currency_desc             AS CurrencyDesc
                              ,net_finance               AS NetFinance
                              ,interest_rate             AS InterestRate
                              ,cost_rate                 AS CostRate
                              ,interest_margin           AS InterestMargin
                              ,bpe_ratio                 AS BPERatio
                              ,insurance_premium_usage_ratio AS InsurancePremiumUsageRatio
                              ,bpe_effect                AS BPEEffect
                              ,bpe_income                AS BPEIncome
                              ,incentive_expense         AS IncentiveExpense
                              ,non_interest_effect       AS NonInterestEffect
                              ,profit_before_marketing_incentive AS ProfitBeforeMarketingIncentive
                              ,incentive_amount          AS IncentiveAmount
                              ,marketing_incentive_ratio AS MarketingIncentiveRatio
                              ,finance_amount            AS FinanceAmount
                              ,insurance_rate             AS InsuranceRate
                              ,interest_amount            AS InterestAmount
                              ,cost_amount                AS CostAmount
                              ,interest_margin_amount     AS InterestMarginAmount
                              ,ccy_rate                   AS CCYRate
                              ,bpe_total                  AS BPETotal
                              ,bpe_total_amount           AS BPETotalAmount
                              ,non_interest_name          AS NonInterestName
                              ,non_interest_expense       AS NonInterestExpense
                              ,non_interest_effect_amount AS NonInterestEffectAmount
                              -- Calculated Fields
                              ,CASE WHEN profit_before_marketing_incentive <> 0 THEN interest_margin / profit_before_marketing_incentive ELSE 0 END AS InterestMarginProfitBeforeMarketingIncentive
                              ,CASE WHEN incentive_amount <> 0 THEN marketing_incentive_ratio / incentive_amount ELSE 0 END AS MarketingIncentiveRatioIncentiveAmount
                              ,CASE WHEN finance_amount <> 0 THEN marketing_incentive_ratio / finance_amount ELSE 0 END AS MarketingIncentiveRatioFinanceAmount
                              ,CASE WHEN incentive_expense <> 0 THEN bpe_income / incentive_expense ELSE 0 END AS BPEIncomeIncentiveExpense
                              ,(interest_margin - cost_rate)   AS NetInterestMarginAfterCost
                        from 
                            {tableBase}
                        where 
                        incentive_marketing_id = {p}IncentiveMarketingID 
                        order by
                        cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        IncentiveMarketingID = incentiveMarketingID
      };

      var result = await _command.GetRows<AgreementMarketing>(transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, AgreementMarketing model)
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
                                ,agreement_id
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
                                ,bpe_ratio
                                ,insurance_premium_usage_ratio
                                ,bpe_effect
                                ,bpe_income
                                ,incentive_expense
                                ,non_interest_effect
                                ,profit_before_marketing_incentive
                                ,incentive_amount
                                ,marketing_incentive_ratio
                                ,finance_amount
                                ,insurance_rate             
                                ,interest_amount
                                ,cost_amount            
                                ,interest_margin_amount     
                                ,ccy_rate                   
                                ,bpe_total                  
                                ,bpe_total_amount           
                                ,non_interest_name          
                                ,non_interest_expense       
                                ,non_interest_effect_amount 
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
                                ,{p}AgreementID
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
                                ,{p}BPERatio
                                ,{p}InsurancePremiumUsageRatio
                                ,{p}BPEEffect
                                ,{p}BPEIncome
                                ,{p}IncentiveExpense
                                ,{p}NonInterestEffect
                                ,{p}ProfitBeforeMarketingIncentive
                                ,{p}IncentiveAmount
                                ,{p}MarketingIncentiveRatio
                                ,{p}FinanceAmount
                                ,{p}InsuranceRate
                                ,{p}InterestAmount
                                ,{p}CostAmount
                                ,{p}InterestMarginAmount
                                ,{p}CCYRate
                                ,{p}BPETotal
                                ,{p}BPETotalAmount
                                ,{p}NonInterestName
                                ,{p}NonInterestExpense
                                ,{p}NonInterestEffectAmount
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, AgreementMarketing model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_marketing_id = {p}IncentiveMarketingID
                                ,agreement_id          = {p}AgreementID
                                ,agreement_no          = {p}AgreementNo
                                ,client_id			           = {p}ClientID
                                ,client_no			           = {p}ClientNo
                                ,client_name		           = {p}ClientName
                                ,approved_date         = {p}ApprovedDate
                                ,disbursement_date     = {p}DisbursementDate
                                ,incentive_period	   = {p}IncentivePeriod
                                ,payment_method        = {p}PaymentMethod
                                ,currency_id           = {p}CurrencyID
                                ,currency_code         = {p}CurrencyCode
                                ,currency_desc         = {p}CurrencyDesc
                                ,net_finance           = {p}NetFinance
                                ,interest_rate         = {p}InterestRate
                                ,cost_rate             = {p}CostRate
                                ,interest_margin       = {p}InterestMargin
                                ,bpe_ratio             = {p}BPERatio
                                ,insurance_premium_usage_ratio = {p}InsurancePremiumUsageRatio
                                ,bpe_effect            = {p}BPEEffect
                                ,bpe_income            = {p}BPEIncome
                                ,incentive_expense     = {p}IncentiveExpense
                                ,non_interest_effect   = {p}NonInterestEffect
                                ,profit_before_marketing_incentive = {p}ProfitBeforeMarketingIncentive
                                ,incentive_amount      = {p}IncentiveAmount
                                ,marketing_incentive_ratio = {p}MarketingIncentiveRatio
                                ,finance_amount        = {p}FinanceAmount
                                ,insurance_rate             ={p}InsuranceRate
                                ,cost_amount                ={p}CostAmount
                                ,interest_amount            ={p}InterestAmount
                                ,interest_margin_amount     ={p}InterestMarginAmount
                                ,ccy_rate                   ={p}CCYRate
                                ,bpe_total                  ={p}BPETotal
                                ,bpe_total_amount           ={p}BPETotalAmount
                                ,non_interest_name          ={p}NonInterestName
                                ,non_interest_expense       ={p}NonInterestExpense
                                ,non_interest_effect_amount ={p}NonInterestEffectAmount
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