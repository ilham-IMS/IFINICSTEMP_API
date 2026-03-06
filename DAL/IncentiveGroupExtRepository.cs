using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
    public class IncentiveGroupExtRepository : BaseRepository, IIncentiveGroupExtRepository
    {
        private string tableName = "incentive_group_ext";

        public async Task<List<ExtendModel>> GetRowForParent(IDbTransaction transaction, string parentID)
        {
            var p = db.Symbol();
            string query =
                $@"
                        select
                                 id                                as ID
                                ,incentive_group_id               as ParentID
                                ,keyy                              as Keyy
                                ,value                             as Value
                        from
                        {tableName}
                    where
                        incentive_group_id = {p}ID";
            var result = await _command.GetRows<ExtendModel>(
                transaction,
                query,
                new { ID = parentID }
            );
            return result;
        }

        public async Task<int> Insert(IDbTransaction transaction, ExtendModel module)
        {
            var p = db.Symbol();
            string query =
                $@"
                            insert into {tableName} (
                                 id
                                ,cre_date
                                ,cre_by
                                ,cre_ip_address
                                ,mod_date
                                ,mod_by
                                ,mod_ip_address
                                ,incentive_group_id
                                ,keyy
                                ,value
                            ) values (
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
            var result = await _command.Insert(transaction, query, module);
            return result;
        }

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
                                incentive_group_id = {p}ParentID
                            and
                                keyy = {p}Keyy";
            var result = await _command.Update(transaction, query, model);
            return result;
        }

    }
}
