using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class SysDocumentNumberRepository : BaseRepository, ISysDocumentNumberRepository
  {
    private readonly string tableName = "sys_document_number";
    public async Task<List<SysDocumentNumber>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();

      // Query
      string query = $@"
		                select
				            id					    as ID
				            ,code	          as Code
				            ,code_document	as CodeDocument
				            ,description	  as Description
										,is_active      as IsActive
		from
				{tableName}
		where
				(
					lower({tableName}.code)		          			like	lower({p}Keyword)
					or lower({tableName}.code_document)				like	lower({p}Keyword)
					or lower({tableName}.description)	  			like	lower({p}Keyword)
					or case {tableName}.is_active
                        when 1 then 'yes'
                        else 'no'
                    end                             like lower({p}Keyword)
				)
		order by
				mod_date desc
	";

      // Gunakan QueryLimitOffset untuk menambahkan
      // query offset dan rows fetch next
      query = QueryLimitOffset(query);
      // Binding Parameters
      var parameters = new
      {
        Keyword = $"%{keyword}%",
        Offset = offset,
        Limit = limit
      }
      ;

      var result = await _command.GetRows<SysDocumentNumber>(transaction, query, parameters);
      return result;
    }
    public async Task<SysDocumentNumber> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
		  select
			  id				      as ID
			  ,code           as Code
				,code_document  as CodeDocument
				,description    as Description
				,is_active      as IsActive
		from
			{tableName}
		where
			id = {p}ID
		";

      // Binding Parameters
      var parameters = new
      {
        ID = id
      };

      var result = await _command.GetRow<SysDocumentNumber>(transaction, query, parameters);
      return result;
    }

    public async Task<SysDocumentNumber> GetRowByCode(IDbTransaction transaction, string code)
    {
      var p = db.Symbol();

      string query = $@"
		  select
			  id				      as ID
			  ,code           as Code
				,code_document  as CodeDocument
				,description    as Description
				,is_active      as IsActive
		from
			{tableName}
		where
			code = {p}Code
		";

      // Binding Parameters
      var parameters = new
      {
        Code = code
      };

      var result = await _command.GetRow<SysDocumentNumber>(transaction, query, parameters);
      return result;
    }
    public async Task<int> Insert(IDbTransaction transaction, SysDocumentNumber model)
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
				--
				,code
				,code_document
				,description
				,is_active
			)
			values
			(
				 {p}ID
				,{p}CreDate
				,{p}CreBy
				,{p}CreIpAddress
				,{p}ModDate
				,{p}ModBy
				,{p}ModIpAddress
				--
				,{p}Code
				,{p}CodeDocument
				,{p}Description
				,{p}IsActive
			)";

      return await _command.Insert(transaction, query, model);
    }
    public async Task<int> UpdateByID(IDbTransaction transaction, SysDocumentNumber model)
    {
      var p = db.Symbol();

      string query = $@"
					update {tableName}
					set
						 code_document  = {p}CodeDocument
            ,description    = {p}Description
					where
						id = {p}ID";

      return await _command.Update(transaction, query, model);
    }
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
					delete from {tableName}
					where
						id = {p}ID";
      return await _command.DeleteByID(transaction, query, id);
    }

    public async Task<int?> CountCode(IDbTransaction transaction, string id, string code)
    {
      var p = db.Symbol();

      string query = $@"
					select
									count(code)	as Code
					from
							{tableName}
					where
              {tableName}. id != {p}ID
          and    
							{tableName}.code = {p}Code
				";

      var parameters = new
      {
        ID = id,
        Code = code
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }

    public async Task<int?> CountCodeDocument(IDbTransaction transaction, string id, string codeDocument)
    {
      var p = db.Symbol();

      string query = $@"
					select
									count(code_document)	as CodeDocument
					from
							{tableName}
					where
              {tableName}.id != {p}ID
          and    
							{tableName}.code_document = {p}CodeDocument
				";

      var parameters = new
      {
        ID = id,
        CodeDocument = codeDocument
      };

      return await _command.GetRow<int?>(transaction, query, parameters);
    }
    public async Task<int> ChangeStatusActive(IDbTransaction transaction, SysDocumentNumber model)
    {
      var p = db.Symbol();

      string query = $@"
                            update {tableName}
                            set
                                is_active           =  is_active * -1
                                ,mod_date           =  {p}ModDate
                                ,mod_by             =  {p}ModBy
                                ,mod_ip_address     =  {p}ModIPAddress
                            where
                                id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
  }
}