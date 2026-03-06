using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;


namespace DAL
{
    public class FormDropdownRepository : BaseRepository, IFormDropdownRepository
    {
        private string tableName = "form_drop_down";

        public async Task<List<FormDropdown>> GetRows(
            IDbTransaction transaction,
            string formControlsID
        )
        {
            var p = db.Symbol();
            string query =
                $@"
                            select
                                 id               as ID
                                ,form_controls_id as FormControlsID
                                ,key              as Key
                                ,value            as Value
                            from       
                                {tableName}    
                            where
                                form_controls_id = {p}FormControlsID
                            ";
            var result = await _command.GetRows<FormDropdown>(
                transaction,
                query,
                new
                {
                    FormControlsID = formControlsID
                }
            );
            return result;

        }

        public async Task<FormDropdown> GetRowByID(IDbTransaction transaction, string id)
        {
            var p = db.Symbol();
            string query =
                $@"
                        select
                                 id               as ID
                                ,form_controls_id as FormControlsID
                                ,key              as Key
                                ,value            as Value
                        from
                        {tableName}
                    where
                        id = {p}ID";
            var result = await _command.GetRow<FormDropdown>(
                transaction,
                query,
                new { ID = id }
            );
            return result;
        }

        public async Task<int> Insert(IDbTransaction transaction, FormDropdown module)
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
                                ,form_controls_id
                                ,key
                                ,value         
                            ) values (
                                 {p}ID
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIPAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIPAddress
                                ,{p}FormControlsID
                                ,{p}Key
                                ,{p}Value
                            )";
            var result = await _command.Insert(transaction, query, module);
            return result;
        }

        public async Task<int> UpdateByID(IDbTransaction transaction, FormDropdown module)
        {
            var p = db.Symbol();

            string query =
                $@"
                            update {tableName} 
                            set
                                 key                = {p}Key
                                ,value              = {p}Value
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                form_controls_id = {p}FormControlsID";
            var result = await _command.Update(transaction, query, module);
            return result;
        }

        public async Task<int> DeleteByID(IDbTransaction transaction, string id)
        {
            var p = db.Symbol();

            string query =
                $@"
                            delete from {tableName}
                            where
                                form_controls_id = {p}FormControlsID";
            var result = await _command.Delete(transaction, query, new { FormControlsID = id });
            return result;
        }

        public Task<List<FormDropdown>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
