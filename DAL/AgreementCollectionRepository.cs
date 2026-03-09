
using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class AgreementCollectionRepository : BaseRepository, IAgreementCollectionRepository
  {
    private readonly string tableBase = "agreement_collection";

    public async Task<AgreementCollection> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query =
                    $@"  select
                                id                         AS ID
                                ,incentive_collection_id    AS IncentiveCollectionID
                                ,agreement_id              AS AgreementID
                                ,agreement_no              AS AgreementNo
                                ,client_id			           AS	ClientID
                                ,client_no			           AS	ClientNo
                                ,client_name		           AS	ClientName
                                ,collector_id             AS CollectorID
                                ,collector_code           AS CollectorCode
                                ,collector_name           AS CollectorName
                                ,marketing_id             AS MarketingID
                                ,marketing_code           AS MarketingCode
                                ,marketing_name           AS MarketingName
                                ,incentive_period	         AS IncentivePeriod
                                ,installment_no           AS InstallmentNo
                                ,unpaid_amount           AS UnpaidAmount
                                ,unpaid_date             AS UnpaidDate
                                ,starting_date_handling  AS StartingDateHandling
                                ,deadline_date_handling  AS DeadlineDateHandling
                                ,incentive_result        AS IncentiveResult
                                ,collected_amount        AS CollectedAmount
                                ,paid_case               AS PaidCase
                                ,collected_pct           AS CollectedPct
                                ,incentive_pct           AS IncentivePct
                                ,incentive_amount        AS IncentiveAmount
                                ,remarks                 AS Remarks

                          from
                              {tableBase}
                          where
                              id = {p}ID
                  ";
      var parameters = new { id };

      var result = await _command.GetRow<AgreementCollection>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementCollection>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,incentive_collection_id    AS IncentiveCollectionID
                              ,agreement_id              AS AgreementID
                              ,agreement_no              AS AgreementNo
                              ,client_id			           AS	ClientID
                              ,client_no			           AS	ClientNo
                              ,client_name		           AS	ClientName
                              ,collector_id             AS CollectorID
                              ,collector_code           AS CollectorCode
                              ,collector_name           AS CollectorName
                              ,marketing_id             AS MarketingID
                              ,marketing_code           AS MarketingCode
                              ,marketing_name           AS MarketingName
                              ,incentive_period	         AS IncentivePeriod
                              ,installment_no           AS InstallmentNo
                              ,unpaid_amount           AS UnpaidAmount
                              ,unpaid_date             AS UnpaidDate
                              ,starting_date_handling  AS StartingDateHandling
                              ,deadline_date_handling  AS DeadlineDateHandling
                              ,incentive_result        AS IncentiveResult
                              ,collected_amount        AS CollectedAmount
                              ,paid_case               AS PaidCase
                              ,collected_pct           AS CollectedPct
                              ,incentive_pct           AS IncentivePct
                              ,incentive_amount        AS IncentiveAmount
                              ,remarks                 AS Remarks
                        from 
                            {tableBase}
                        where 
                        (
                            lower(agreement_no)                                               like lower({p}Keyword)
                            or lower({FormatNumeric("installment_no")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("unpaid_amount")})                       like lower({p}Keyword)
                            or cast({FormatDateTime($"unpaid_date")} as char(50))           like lower({p}Keyword)
                            or cast({FormatDateTime($"starting_date_handling")} as char(50)) like lower({p}Keyword)
                            or cast({FormatDateTime($"deadline_date_handling")} as char(50)) like lower({p}Keyword)
                            or lower(incentive_result)                                               like lower({p}Keyword)
                            or lower({FormatNumeric("collected_amount")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("collected_pct")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("incentive_pct")})                       like lower({p}Keyword)
                            or lower({FormatNumeric("incentive_amount")})                       like lower({p}Keyword)
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

      var result = await _command.GetRows<AgreementCollection>(transaction, query, parameters);
      return result;
    }

    public async Task<List<AgreementCollection>> GetRowsByIncentiveID(IDbTransaction transaction, string? keyword, int offset, int limit, string incentiveCollectionID)
    {
      var p = db.Symbol();
      string query =
                  $@"
                        select 
                              id                      AS ID
                              ,incentive_collection_id    AS IncentiveCollectionID
                              ,agreement_id              AS AgreementID
                              ,agreement_no              AS AgreementNo
                              ,client_id			           AS	ClientID
                              ,client_no			           AS	ClientNo
                              ,client_name		           AS	ClientName
                              ,collector_id             AS CollectorID
                              ,collector_code           AS CollectorCode
                              ,collector_name           AS CollectorName
                              ,marketing_id             AS MarketingID
                              ,marketing_code           AS MarketingCode
                              ,marketing_name           AS MarketingName
                              ,incentive_period	         AS IncentivePeriod
                              ,installment_no           AS InstallmentNo
                              ,unpaid_amount           AS UnpaidAmount
                              ,unpaid_date             AS UnpaidDate
                              ,starting_date_handling  AS StartingDateHandling
                              ,deadline_date_handling  AS DeadlineDateHandling
                              ,incentive_result        AS IncentiveResult
                              ,collected_amount        AS CollectedAmount
                              ,paid_case               AS PaidCase
                              ,collected_pct           AS CollectedPct
                              ,incentive_pct           AS IncentivePct
                              ,incentive_amount        AS IncentiveAmount
                              ,remarks                 AS Remarks
                        from 
                            {tableBase}
                        where 
                        incentive_collection_id = {p}IncentiveCollectionID 
                        order by
                        cre_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        IncentiveCollectionID = incentiveCollectionID
      };

      var result = await _command.GetRows<AgreementCollection>(transaction, query, parameters);
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, AgreementCollection model)
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
                                ,incentive_collection_id   
                                ,agreement_id             
                                ,agreement_no             
                                ,client_id			          
                                ,client_no			          
                                ,client_name		          
                                ,collector_id            
                                ,collector_code          
                                ,collector_name          
                                ,marketing_id            
                                ,marketing_code          
                                ,marketing_name          
                                ,incentive_period	        
                                ,installment_no          
                                ,unpaid_amount          
                                ,unpaid_date            
                                ,starting_date_handling 
                                ,deadline_date_handling 
                                ,incentive_result       
                                ,collected_amount       
                                ,paid_case              
                                ,collected_pct          
                                ,incentive_pct          
                                ,incentive_amount       
                                ,remarks                
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
                                ,{p}IncentiveCollectionID
                                ,{p}AgreementID
                                ,{p}AgreementNo
                                ,{p}ClientID
                                ,{p}ClientNo
                                ,{p}ClientName
                                ,{p}CollectorID
                                ,{p}CollectorCode
                                ,{p}CollectorName
                                ,{p}MarketingID
                                ,{p}MarketingCode
                                ,{p}MarketingName
                                ,{p}IncentivePeriod
                                ,{p}InstallmentNo
                                ,{p}UnpaidAmount
                                ,{p}UnpaidDate
                                ,{p}StartingDateHandling
                                ,{p}DeadlineDateHandling
                                ,{p}IncentiveResult
                                ,{p}CollectedAmount
                                ,{p}PaidCase
                                ,{p}CollectedPct
                                ,{p}IncentivePct
                                ,{p}IncentiveAmount
                                ,{p}Remarks
                                
                            )";
      return await _command.Insert(transaction, query, model);
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, AgreementCollection model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date               =  {p}ModDate
                                ,mod_by                =  {p}ModBy
                                ,mod_ip_address        =  {p}ModIPAddress
                                ,incentive_collection_id   =  {p}IncentiveCollectionID
                                ,agreement_id             =  {p}AgreementID
                                ,agreement_no             =  {p}AgreementNo
                                ,client_id			          =  {p}ClientID
                                ,client_no		          =  {p}ClientNo
                                ,client_name		          =  {p}ClientName
                                ,collector_id            =  {p}CollectorID
                                ,collector_code          =  {p}CollectorCode
                                ,collector_name          =  {p}CollectorName
                                ,marketing_id            =  {p}MarketingID
                                ,marketing_code          =  {p}MarketingCode
                                ,marketing_name          =  {p}MarketingName
                                ,incentive_period	        =  {p}IncentivePeriod
                                ,installment_no          =  {p}InstallmentNo
                                ,unpaid_amount          =  {p}UnpaidAmount
                                ,unpaid_date            =  {p}UnpaidDate
                                ,starting_date_handling =  {p}StartingDateHandling
                                ,deadline_date_handling =  {p}DeadlineDateHandling
                                ,incentive_result       =  {p}IncentiveResult
                                ,collected_amount       =  {p}CollectedAmount
                                ,paid_case              =  {p}PaidCase
                                ,collected_pct          =  {p}CollectedPct
                                ,incentive_pct          =  {p}IncentivePct
                                ,incentive_amount       =  {p}IncentiveAmount
                                ,remarks                =  {p}Remarks
                                
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