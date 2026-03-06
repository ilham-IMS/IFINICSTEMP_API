using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;
using System.Reflection.Emit;

namespace DAL
{
  public class MasterFormRepository : BaseRepository, IMasterFormRepository
  {
    private string tableName = "master_form";

    public async Task<List<MasterForm>> GetRows(
        IDbTransaction transaction,
        string? keyword,
        int offset,
        int limit
    )
    {
      var p = db.Symbol();
      string query = QueryLimitOffset(
          $@"
                            select
                                 id                                as ID
                                ,code                              as Code
                                ,name                              as Name
                                ,label                             as Label
                                ,is_active                         as IsActive
                            from       
                                {tableName}    
                            where (    
                                    lower(name)                    like lower({p}Keyword)
                                    or lower(label)                like lower({p}Keyword)
                                    or case is_active
                                            when    1 then 'yes'
                                            else 'no'
                                        end                        like lower({p}Keyword)
                                  )
                            order by
                                    mod_date desc "
      );
      var result = await _command.GetRows<MasterForm>(
          transaction,
          query,
          new
          {
            Keyword = "%" + keyword + "%",
            Offset = offset,
            Limit = limit
          }
      );
      return result;

    }

    public async Task<MasterForm> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();
      string query =
          $@"
                        select
                                 id                                as ID
                                ,code                              as Code
                                ,name                              as Name
                                ,label                             as Label
                                ,is_active                         as IsActive
                        from
                        {tableName}
                    where
                        id = {p}ID";
      var result = await _command.GetRow<MasterForm>(
          transaction,
          query,
          new { ID = id }
      );
      return result;
    }

    public async Task<MasterForm> GetRowByCode(IDbTransaction transaction, string code)
    {
      var p = db.Symbol();
      string query =
          $@"
                        select
                                 id                                as ID
                                ,code                              as Code
                                ,name                              as Name
                                ,label                             as Label
                                ,is_active                         as IsActive
                        from
                        {tableName}
                    where
                        code = {p}Code";
      var result = await _command.GetRow<MasterForm>(
          transaction,
          query,
          new { Code = code }
      );
      return result;
    }

    public async Task<int> Insert(IDbTransaction transaction, MasterForm module)
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
                                ,code
                                ,name             
                                ,label      
                                ,is_active        
                            ) values (
                                 {p}ID
                                ,{p}CreDate
                                ,{p}CreBy
                                ,{p}CreIPAddress
                                ,{p}ModDate
                                ,{p}ModBy
                                ,{p}ModIPAddress
                                ,{p}Code
                                ,{p}Name
                                ,{p}Label
                                ,{p}IsActive
                            )";
      var result = await _command.Insert(transaction, query, module);
      return result;
    }

    public async Task<int> UpdateByID(IDbTransaction transaction, MasterForm module)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableName} 
                            set
                                 name               = {p}Name   
                                ,label              = {p}Label
                                ,is_active          = {p}IsActive
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
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
                                id = {p}ID";
      var result = await _command.DeleteByID(transaction, query, id);
      return result;
    }

    public async Task<int> ChangeStatus(IDbTransaction transaction, MasterForm model)
    {
      var p = db.Symbol();

      string query =
          $@"
                            update {tableName} 
                            set 
                                is_active           = is_active * -1
                                ,mod_date           = {p}ModDate
                                ,mod_by             = {p}ModBy
                                ,mod_ip_address     = {p}ModIPAddress
                            where
                                id = {p}ID";
      var result = await _command.Update(transaction, query, model);
      return result;
    }

    public async Task LockRow(IDbTransaction transaction, string tableName, string ID)
    {
      await _command.LockRow(transaction, tableName, ID);
    }
    public async Task<int?> CountByCode(IDbTransaction transaction, string code)
    {
      var p = db.Symbol();

      string query = $@"
                        select
                                count(id)	as ID
                        from
                                {tableName}
                        where
                                {tableName}.code = {p}Code  
                    ";

      var parameters = new
      {
        Code = code
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }

    public async Task<int?> CountByName(IDbTransaction transaction, string name, string ID)
    {
      var p = db.Symbol();

      string query = $@"
                        select
                                count(id)	as ID
                        from
                                {tableName}
                        where
                                {tableName}.name = {p}Name
                        and     {tableName}.id != {p}ID   
                    ";

      var parameters = new
      {
        Name = name
          ,
        ID = ID
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }

    public async Task<int?> CountByLabel(IDbTransaction transaction, string label, string ID)
    {
      var p = db.Symbol();

      string query = $@"
                        select
                                count(id)	as ID
                        from
                                {tableName}
                        where
                                {tableName}.label = {p}Label
                        and     {tableName}.id != {p}ID   
                    ";

      var parameters = new
      {
        Label = label
          ,
        ID = ID
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }
  }
}
