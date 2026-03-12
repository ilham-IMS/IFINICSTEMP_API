
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class AgreementRefundRepository : BaseRepository, IAgreementRefundRepository
  {
    private readonly string tableBase = "agreement_refund";

    public async Task<AgreementRefund> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
  
      string query =
                    $@"  select
                                id                         AS ID
                                ,agreement_incentive_id     AS AgreementIncentiveID
                                ,refund_id                 AS RefundID
                                ,refund_code               AS RefundCode
                                ,refund_desc               AS RefundDesc
                                ,refund_amount             AS RefundAmount
                                ,refund_rate               AS RefundRate
                                ,calculate_by              AS CalculateBy
                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<AgreementRefund>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementRefund>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,agreement_incentive_id   AS AgreementIncentiveID
                              ,refund_id                 AS RefundID
                              ,refund_code               AS RefundCode
                              ,refund_desc               AS RefundDesc
                              ,refund_amount             AS RefundAmount
                              ,refund_rate               AS RefundRate
                              ,calculate_by              AS CalculateBy
                              
                        from 
                            {tableBase}
                        where 
                        (
                            lower(refund_desc)                                               like lower({p}Keyword)
                            or lower({FormatNumeric("refund_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("refund_rate")})                       like lower({p}Keyword)
                            or lower(calculate_by)                                           like lower({p}Keyword)
                           
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

      var result = await _command.GetRows<AgreementRefund>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementRefund>> GetRowsByAgreementID(IDbTransaction transaction, string? keyword, int offset, int limit, string agreementID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              ar.id                      AS ID
                              ,ar.agreement_incentive_id   AS AgreementIncentiveID
                              ,ar.refund_id                 AS RefundID
                              ,ar.refund_code               AS RefundCode
                              ,ar.refund_desc               AS RefundDesc
                              ,ar.refund_amount             AS RefundAmount
                              ,ar.refund_rate               AS RefundRate
                              ,ar.calculate_by              AS CalculateBy
                              ,aim.vendor_name              AS VendorName
                              ,aim.agent_name               AS AgentName
                              
                        from 
                            {tableBase} ar
                        left join agreement_incentive_marketing aim on ar.agreement_incentive_id = aim.id 
                        where 
                            ar.agreement_incentive_id = {p}AgreementID and
                        (
                            lower(ar.refund_desc)                                               like lower({p}Keyword)
                            or lower({FormatNumeric("ar.refund_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("ar.refund_rate")})                       like lower({p}Keyword)
                            or lower(ar.calculate_by)                                           like lower({p}Keyword)
                           
                        )
                        order by
                        ar.cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        AgreementID = agreementID
      };

      var result = await _command.GetRows<AgreementRefund>(transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, AgreementRefund model)
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
                                ,refund_id                 
                                ,refund_code               
                                ,refund_desc               
                                ,refund_amount             
                                ,refund_rate               
                                ,calculate_by                                      
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
                                ,{p}RefundID
                                ,{p}RefundCode
                                ,{p}RefundDesc
                                ,{p}RefundAmount
                                ,{p}RefundRate
                                ,{p}CalculateBy
                                
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, AgreementRefund model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,agreement_incentive_id =  {p}AgreementIncentiveID
                                ,refund_id                =  {p}RefundID
                                ,refund_code              =  {p}RefundCode
                                ,refund_desc              =  {p}RefundDesc
                                ,refund_amount            =  {p}RefundAmount
                                ,refund_rate              =  {p}RefundRate
                                ,calculate_by             =  {p}CalculateBy
                                
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