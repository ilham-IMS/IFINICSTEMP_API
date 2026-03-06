using System.Data;
using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using iFinancing360.Service.Helper;

namespace Service
{
    public class FormControlsService : BaseService, IFormControlsService
    {
        private readonly IFormControlsRepository _repo;
        private readonly IMasterFormRepository _repoMasterForm;
        private readonly IFormDropdownRepository _repoDropdown;


        public FormControlsService(
             IFormControlsRepository repo,
             IFormDropdownRepository repoDropdown
,
             IMasterFormRepository repoMasterForm)
        {
            _repo = repo;
            _repoDropdown = repoDropdown;
            _repoMasterForm = repoMasterForm;
        }

        public async Task<List<FormControls>> GetRows(string masterFormID)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.GetRows(transaction, masterFormID);

                foreach (var formControl in result)
                {
                    await LoadDropdownData(transaction, formControl);
                }

                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<List<FormControls>> GetRowsForDataTable(string masterFormID)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.GetRowsForDataTable(transaction, masterFormID);

                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<FormControls> GetRowByID(string ID)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.GetRowByID(transaction, ID);
                await LoadDropdownData(transaction, result);

                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> Insert(FormControls model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                Dictionary<string, string> Items = model.Items;
                model.Items = null;
                model.Visible = model.IsActive;
                var result = await _repo.Insert(transaction, model);
                await InsertDropdown(transaction, Items, model);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> UpdateByID(FormControls model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                MasterForm masterForm = await _repoMasterForm.GetRowByID(transaction, model.MasterFormID);
                if (masterForm.IsActive == -1 && model.IsActive == 1) throw new Exception("Dynamic form controls cannnot be active, please activated dynamic form first!");

                Dictionary<string, string> Items = model.Items;
                model.Items = null;
                var result = await _repo.UpdateByID(transaction, model);
                //await UpdateDropdown(transaction, Items, model);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> DeleteByID(string[] listID)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                int count = 0;
                foreach (var ID in listID)
                {
                    var result = await _repo.DeleteByID(transaction, ID);
                    if (result > 0)
                        count += result;

                }
                transaction.Commit();
                return count;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> ChangeStatus(FormControls model)
        {
            using var connection = _repo.GetDbConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = await _repo.ChangeStatus(transaction, model);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<List<FormControls>> GetRows(string? keyword, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        protected FormDropdown GetDropdown(KeyValuePair<string, string> item, FormControls model)
        {
            FormDropdown dropdown = new FormDropdown()
            {
                ID = Guid.NewGuid().ToString("N").ToLower(),
                FormControlsID = model.ID,
                CreDate = model.ModDate,
                CreBy = model.ModBy,
                CreIPAddress = model.ModIPAddress,
                ModDate = model.ModDate,
                ModBy = model.ModBy,
                ModIPAddress = model.ModIPAddress,
                Key = item.Key,
                Value = item.Value
            };

            return dropdown;
        }

        protected async Task LoadDropdownData(IDbTransaction transaction, FormControls formControl)
        {
            if (formControl.ComponentName == "FormFieldDropdown")
            {
                var dropdownData = await _repoDropdown.GetRows(transaction, formControl.ID);
                formControl.Items = dropdownData.ToDictionary(item => item.Key, item => item.Value);
            }
        }

        private async Task UpdateDropdown(IDbTransaction transaction, Dictionary<string, string> Items, FormControls model)
        {
            if (Items != null)
            {
                await _repoDropdown.DeleteByID(transaction, model.ID);
                foreach (var item in Items)
                {
                    var dropdown = GetDropdown(item, model);
                    await _repoDropdown.Insert(transaction, dropdown);
                }
            }
        }

        private async Task InsertDropdown(IDbTransaction transaction, Dictionary<string, string> Items, FormControls model)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    var dropdown = GetDropdown(item, model);
                    await _repoDropdown.Insert(transaction, dropdown);
                }
            }
        }

    }
}
