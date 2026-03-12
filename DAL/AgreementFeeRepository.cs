
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class AgreementFeeRepository : BaseRepository, IAgreementFeeRepository
  {
    private readonly string tableBase = "agreement_fee";

    public async Task<AgreementFee> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                         AS ID
                                ,agreement_incentive_id     AS AgreementIncentiveID
                                ,fee_id                    AS FeeID
                                ,fee_code                  AS FeeCode
                                ,fee_name                  AS FeeName
                                ,fee_amount                AS FeeAmount
                                ,fee_payment_type          AS FeePaymentType
                                ,fee_paid_amount           AS FeePaidAmount
                                ,fee_reduce_disburse_amount AS FeeReduceDisburseAmount
                                ,fee_capitalize_amount     AS FeeCapitalizeAmount
                                ,insurance_year            AS InsuranceYear
                                ,remarks                   AS Remarks
                                ,fee_rate                  AS FeeRate
                                ,is_internal_income         AS IsInternalIncome
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<AgreementFee>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementFee>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,agreement_incentive_id   AS AgreementIncentiveID
                              ,fee_id                    AS FeeID
                              ,fee_code                  AS FeeCode
                              ,fee_name                  AS FeeName
                              ,fee_amount                AS FeeAmount
                              ,fee_payment_type          AS FeePaymentType
                              ,fee_paid_amount           AS FeePaidAmount
                              ,fee_reduce_disburse_amount AS FeeReduceDisburseAmount
                              ,fee_capitalize_amount     AS FeeCapitalizeAmount
                              ,insurance_year            AS InsuranceYear
                              ,remarks                   AS Remarks
                              ,fee_rate                  AS FeeRate
                              ,is_internal_income         AS IsInternalIncome
                        from 
                            {tableBase}
                        where 
                        (
                            lower(fee_name)                                               like lower({p}Keyword)
                            or lower({FormatNumeric("fee_amount")})                       like lower({p}Keyword)
                            or lower(fee_payment_type)                                           like lower({p}Keyword)
                            or lower({FormatNumeric("fee_paid_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("fee_reduce_disburse_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("fee_capitalize_amount")})                       like lower({p}Keyword)
                            or lower(insurance_year)                                           like lower({p}Keyword)
                            or lower(remarks)                                           like lower({p}Keyword)
                            or lower({FormatNumeric("fee_rate")})                       like lower({p}Keyword)
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

      var result = await _command.GetRows<AgreementFee>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementFee>> GetRowsByAgreementID(IDbTransaction transaction, string? keyword, int offset, int limit, string agreementID, int isInternalIncome)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,agreement_incentive_id   AS AgreementIncentiveID
                              ,fee_id                    AS FeeID
                              ,fee_code                  AS FeeCode
                              ,fee_name                  AS FeeName
                              ,fee_amount                AS FeeAmount
                              ,fee_payment_type          AS FeePaymentType
                              ,fee_paid_amount           AS FeePaidAmount
                              ,fee_reduce_disburse_amount AS FeeReduceDisburseAmount
                              ,fee_capitalize_amount     AS FeeCapitalizeAmount
                              ,insurance_year            AS InsuranceYear
                              ,remarks                   AS Remarks
                              ,fee_rate                  AS FeeRate
                              ,is_internal_income         AS IsInternalIncome
                        from 
                            {tableBase}
                        where 
                            agreement_incentive_id = {p}AgreementID and
                            is_internal_income = {p}IsInternalIncome and
                        (
                            lower(fee_name)                                               like lower({p}Keyword)
                            or lower({FormatNumeric("fee_amount")})                       like lower({p}Keyword)
                            or lower(fee_payment_type)                                           like lower({p}Keyword)
                            or lower({FormatNumeric("fee_paid_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("fee_reduce_disburse_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("fee_capitalize_amount")})                       like lower({p}Keyword)
                            or lower(insurance_year)                                           like lower({p}Keyword)
                            or lower(remarks)                                           like lower({p}Keyword)
                            or lower({FormatNumeric("fee_rate")})                       like lower({p}Keyword)
                        )
                        order by
                        cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        AgreementID = agreementID,
        IsInternalIncome = isInternalIncome
      };

      var result = await _command.GetRows<AgreementFee>(transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, AgreementFee model)
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
                                ,agreement_incentive_id   
                                ,fee_id                    
                                ,fee_code                  
                                ,fee_name                  
                                ,fee_amount                
                                ,fee_payment_type          
                                ,fee_paid_amount           
                                ,fee_reduce_disburse_amount 
                                ,fee_capitalize_amount     
                                ,insurance_year            
                                ,remarks                   
                                ,fee_rate
                                ,is_internal_income                           
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
                                ,{p}AgreementIncentiveID
                                ,{p}FeeID
                                ,{p}FeeCode
                                ,{p}FeeName
                                ,{p}FeeAmount
                                ,{p}FeePaymentType
                                ,{p}FeePaidAmount
                                ,{p}FeeReduceDisburseAmount
                                ,{p}FeeCapitalizeAmount
                                ,{p}InsuranceYear
                                ,{p}Remarks
                                ,{p}FeeRate
                                ,{p}IsInternalIncome
                                
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, AgreementFee model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,agreement_incentive_id =  {p}AgreementIncentiveID
                                ,fee_id                =  {p}FeeID
                                ,fee_code              =  {p}FeeCode
                                ,fee_name              =  {p}FeeName
                                ,fee_amount            =  {p}FeeAmount
                                ,fee_payment_type      =  {p}FeePaymentType
                                ,fee_paid_amount       =  {p}FeePaidAmount
                                ,fee_reduce_disburse_amount =  {p}FeeReduceDisburseAmount
                                ,fee_capitalize_amount =  {p}FeeCapitalizeAmount
                                ,insurance_year        =  {p}InsuranceYear
                                ,remarks               =  {p}Remarks
                                ,fee_rate             =  {p}FeeRate
                                ,is_internal_income     =  {p}IsInternalIncome
                                
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