using System.Data;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.DAL.Helper;

namespace DAL
{
  public class DynamicReportRepository : BaseRepository, IDynamicReportRepository
  {
    private readonly string tableBase = "dynamic_report";
    private readonly string tableDynamicReportUser = "dynamic_report_user";

    #region GetRows
    public Task<List<DynamicReport>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
        select
          id as ID
          ,title as Title
          ,remarks as Remarks
          ,is_published as IsPublished
        from
          {tableBase}
        where
          (
            title like {p}Keyword
            or remarks like {p}Keyword
          )
        order by
          mod_date desc ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };
      return _command.GetRows<DynamicReport>(transaction, query, parameters);
    }
    #endregion

    #region GetRowsPublished
    public Task<List<DynamicReport>> GetRowsPublished(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query = $@"
        select
          id as ID
          ,title as Title
          ,remarks as Remarks
        from
          {tableBase}
        where
          is_published = 1
          and
          (
            title like {p}Keyword
          )
        order by
          mod_date desc ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };
      return _command.GetRows<DynamicReport>(transaction, query, parameters);
    }
    public Task<List<DynamicReport>> GetRowsPublishedByUser(IDbTransaction transaction, string? keyword, int offset, int limit, string userCode)
    {
      var p = db.Symbol();
      string query = $@"
        select
          dr.id as ID
          ,dr.title as Title
					,dr.remarks as Remarks
        from
          {tableBase} dr
        inner join
					{tableDynamicReportUser} dru on dru.dynamic_report_id = dr.id and dru.user_code = {p}UserCode
				where
          dr.is_published = 1
          and
          (
            dr.title like {p}Keyword
						or dr.remarks like {p}Keyword
          )
        order by
          dr.mod_date desc ";

      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit,
        UserCode = userCode
      };
      return _command.GetRows<DynamicReport>(transaction, query, parameters);
    }
    #endregion

    #region GetRowByID
    public Task<DynamicReport> GetRowByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          id            as ID
          ,title        as Title
          ,is_published as IsPublished
          ,remarks      as Remarks
        from
          {tableBase}
        where
          id = {p}ID";

      var parameters = new
      {
        ID = ID
      };
      return _command.GetRow<DynamicReport>(transaction, query, parameters);
    }
    #endregion

    #region GetQuery
    public Task<string> GetQuery(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query = $@"
        select
          query         as Query
        from
          {tableBase}
        where
          id = {p}ID";

      var parameters = new
      {
        ID = ID
      };
      return _command.GetRow<string>(transaction, query, parameters);
    }
    #endregion

    #region GetDataFromQuery
    public async Task<List<IDictionary<string, object?>>> GetDataFromQuery(IDbTransaction transaction, string query, IDictionary<string, object> parameters)
    {
      var p = db.Symbol();

      if (transaction.Connection is null) throw new Exception("Transaction is not connected");
      // var res = await transaction.Connection.QueryAsync(query);

      // Build parameter dump for debugging
      if (parameters != null)
      {
      var paramDump = string.Join(", ", parameters.Select(kv => $"{kv.Key}={kv.Value ?? "NULL"}"));

      Console.WriteLine($"Executing SQL:\n{query}\nWith Parameters: {paramDump}");
      }

      var res = await _command.GetRows<dynamic>(transaction, query, parameters);

      var dict = res.Select(x => (IDictionary<string, object?>)x).ToList();

      return dict;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, DynamicReport model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    insert into {tableBase} (
                        id
                        ,cre_date
                        ,cre_by
                        ,cre_ip_address
                        ,mod_date
                        ,mod_by
                        ,mod_ip_address
                        --
                        ,title  
                        ,is_published
                        ,remarks
                    ) values (
                        {p}ID
                        ,{p}CreDate
                        ,{p}CreBy
                        ,{p}CreIPAddress
                        ,{p}ModDate
                        ,{p}ModBy
                        ,{p}ModIPAddress
                        --
                        ,{p}Title
                        ,{p}IsPublished
                        ,{p}Remarks
                    )";
      var result = await _command.Insert(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, DynamicReport model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        title = {p}Title
                        ,remarks = {p}Remarks
                        --
                        ,mod_date         = {p}ModDate
                        ,mod_by           = {p}ModBy
                        ,mod_ip_address   = {p}ModIPAddress
                    where
                        id = {p}ID
                    ";
      var result = await _command.Update(transaction, query, model);
      return result;
    }
    #endregion

    #region UpdateQuery
    public async Task<int> UpdateQuery(IDbTransaction transaction, DynamicReport model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        query = {p}Query
                        --
                        ,mod_date         = {p}ModDate
                        ,mod_by           = {p}ModBy
                        ,mod_ip_address   = {p}ModIPAddress
                    where
                        id = {p}ID
                    ";
      var result = await _command.Update(transaction, query, model);
      return result;
    }
    #endregion

    #region DeleteByID
    public async Task<int> DeleteByID(IDbTransaction transaction, string ID)
    {
      var p = db.Symbol();
      string query =
          $@"
                    delete from {tableBase}
                    where
                        id = {p}ID
                    ";
      var result = await _command.DeleteByID(transaction, query, ID);
      return result;
    }
    #endregion

    #region GetPublishedStatus
    public async Task<int> GetPublishedStatus(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query =
          $@"
                    select
                        is_published
                    from
                        {tableBase}
                    where
                        id = {p}ID
                    ";

      var parameters = new
      {
        ID = id
      };
      var result = await _command.GetRow<int>(transaction, query, parameters);
      return result;
    }
    #endregion

    #region ChangePublishedStatus
    public async Task<int> ChangePublishedStatus(IDbTransaction transaction, DynamicReport model)
    {
      var p = db.Symbol();
      string query =
          $@"
                    update {tableBase}
                    set
                        is_published      = is_published * -1 
                        --
                        ,mod_date         = {p}ModDate
                        ,mod_by           = {p}ModBy
                        ,mod_ip_address   = {p}ModIPAddress
                    where
                        id = {p}ID
                    ";
      var result = await _command.Update(transaction, query, model);
      return result;
    }
    #endregion

    public async Task<List<DynamicReport>> GetReportData(IDbTransaction transaction)
    {
      string query =
                      $@"
                      select
												title as Title
											from
												{tableBase}
                        order by
                            mod_date desc ";
      var result = await _command.GetRows<DynamicReport>(
          transaction,
          query);
      return result;
    }
  }
}