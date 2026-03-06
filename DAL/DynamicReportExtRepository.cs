using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class DynamicReportExtRepository : BaseRepository, IDynamicReportExtRepository
  {
    private string tableName = "dynamic_report_ext";

    #region GetRowForParent
    public async Task<List<ExtendModel>> GetRowForParent(IDbTransaction transaction, string parentID)
    {
      var p = db.Symbol();

      string query = $@"
        select
          id                        as ID
          ,dynamic_report_id  as ParentID
          ,keyy                     as Keyy
          ,value                    as Value
        from
        {tableName}
        where
          dynamic_report_id = {p}ID";

      var result = await _command.GetRows<ExtendModel>(
          transaction,
          query,
          new { ID = parentID }
      );

      return result;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, ExtendModel module)
    {
      var p = db.Symbol();

      string query = $@"
        insert into {tableName}
        (
          id
          ,cre_date
          ,cre_by
          ,cre_ip_address
          ,mod_date
          ,mod_by
          ,mod_ip_address
          ,dynamic_report_id
          ,keyy
          ,value
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
          ,{p}ParentID
          ,{p}Keyy
          ,{p}Value
        )";

      return await _command.Insert(transaction, query, module);
    }
    #endregion

    #region UpdateByID
        public async Task<int> UpdateByID(IDbTransaction transaction, ExtendModel model)
        {
            var p = db.Symbol();

            string query =
                $@"
                            update {tableName}
                            set
                                 value              = {p}Value
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                dynamic_report_id  = {p}ParentID
                            and
                                keyy = {p}Keyy";
            var result = await _command.Update(transaction, query, model);
            return result;
        }
        #endregion
  }
}
