using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class JournalGlLinkRepository : BaseRepository, IJournalGlLinkRepository
  {

    private readonly string tableBase = "journal_gl_link";

    #region getrows 
    public async Task<List<JournalGlLink>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();
      string query =
                  $@"
                          select 
                              id              AS ID
                              ,code           AS Code
                              ,gl_link_name   AS GlLinkName
                              ,is_active      AS  IsActive
                          from 
                              {tableBase}
                          where 
                          (
                            lower(code)                 like    lower({p}Keyword)
                            or lower(gl_link_name)      like    lower({p}Keyword)
                            or case is_active
                                when 1 then 'yes'
                                else		'no'
                            end					                like	  lower({p}Keyword)
                        )
                        order by
                        mod_date desc
                        ";
      query = QueryLimitOffset(query);

      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      };

      var result = await _command.GetRows<JournalGlLink>
      (transaction, query, parameters);
      return result;
    }
    #endregion

    #region getrow
    public async Task<JournalGlLink> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                    select
                        id                AS ID
                        ,code             AS Code
                        ,gl_link_name     AS GlLinkName
                        ,is_active        AS IsActive
                    from
                        {tableBase}
                    where
                        id = {p}ID
            ";
      var parameters = new { id };

      var result = await _command.GetRow<JournalGlLink>(transaction, query, parameters);
      return result;
    }
    #endregion

    #region insert
    public async Task<int> Insert(IDbTransaction transaction, JournalGlLink model)
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
                    ,code          
                    ,gl_link_name
                    ,is_active
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
                    ,{p}Code
                    ,{p}GlLinkName
                    ,{p}IsActive

                )";
      return await _command.Insert(transaction, query, model);
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, JournalGlLink model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                mod_date            =  {p}ModDate
                                ,mod_by             =  {p}ModBy
                                ,mod_ip_address     =  {p}ModIPAddress
                                ,gl_link_name       =  {p}GlLinkName
                                ,is_active          =  {p}IsActive
                            where
                                id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
    #endregion

    #region delete
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
                            delete from {tableBase}
                            where
                                id = {p}ID";
      return await _command.DeleteByID(transaction, query, id.ToString());
    }
    #endregion

    #region change active
    public async Task<int> ChangeIsActive(IDbTransaction transaction, JournalGlLink model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableBase}
                            set
                                is_active           =  is_active * -1
                                ,mod_date           =  {p}ModDate
                                ,mod_by             =  {p}ModBy
                                ,mod_ip_address     =  {p}ModIPAddress
                            where
                                id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
    #endregion

    public async Task<List<JournalGlLink>> GetReportData(IDbTransaction transaction)
    {
      string query =
                                      $@"
                                select
                                         id                         AS ID
                                        ,code                       AS Code
                                        ,gl_link_name               AS GlLinkName
                                        ,is_active                  AS IsActive
                                from
                                    {tableBase}
                                order by
                                    mod_date desc";
      var result = await _command.GetRows<JournalGlLink>(
              transaction,
              query);
      return result;
    }
  }
}