using System.Data;
using iFinancing360.DAL.Helper;
using Domain.Abstract.Repository;
using Domain.Models;

namespace DAL
{
  public class SysDocumentUploadRepository : BaseRepository, ISysDocumentUploadRepository
  {
    private readonly string tableBase = "sys_document_upload";

    #region GetRows
    public async Task<List<SysDocumentUpload>> GetRows(IDbTransaction transaction, string? keyword, int offset, int limit)
    {
      var p = db.Symbol();

      string query = $@"
					select
						id							as	ID
						,reff_no				as	ReffNo
						,reff_name		  as	ReffName
						,reff_trx_code	as	ReffTrxCode
            ,file_name			as	FileName
					from
						{tableBase}
					where
						(
							lower({tableBase}.reff_name)								like lower({p}Keyword)
							or lower({tableBase}.file_name)			like lower({p}Keyword)
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

      return await _command.GetRows<SysDocumentUpload>(transaction, query, parameters);
    }
    #endregion

    #region GetRowByID
    public async Task<SysDocumentUpload> GetRowByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
					select
						id							as	ID
						,reff_no				as	ReffNo
						,reff_name		  as	ReffName
						,reff_trx_code	as	ReffTrxCode
            ,file_name			as	FileName
            ,doc_file       as  DocFile
					from
						{tableBase}
					where
						{tableBase}.id = {p}ID
							";

      var result = await _command.GetRow<SysDocumentUpload>(
        transaction, query, new { ID = id }
      );
      return result;
    }
    #endregion

    #region GetRowByName
    public async Task<SysDocumentUpload> GetRowByName(IDbTransaction transaction, string reffName, string fileName)
    {
      var p = db.Symbol();

      string query = $@"
					select
						id							as	ID
            ,file_name			as	FileName
            ,doc_file       as  DocFile
					from
						{tableBase}
					where
            reff_name = {p}ReffName
          and 
            file_name = {p}FileName
          ";

      var parameters = new Dictionary<string, object>
      {
        ["ReffName"] = reffName,
        ["FileName"] = fileName
      };

      var result = await _command.GetRow<SysDocumentUpload>(transaction, query, parameters);
      return result;
    }
    #endregion

    #region GetRowByTrxAndName
    public async Task<SysDocumentUpload> GetRowByTrxAndName(IDbTransaction transaction, string trxCode, string fileName)
    {
      var p = db.Symbol();

      string query = $@"
					select
						id							as	ID
            ,file_name			as	FileName
            ,doc_file       as  DocFile
            ,mime_type       as  MimeType
					from
						{tableBase}
					where
            reff_trx_code = {p}ReffTrxCode
          and 
            file_name = {p}FileName
          ";

      var parameters = new Dictionary<string, object>
      {
        ["ReffTrxCode"] = trxCode,
        ["FileName"] = fileName
      };

      var result = await _command.GetRow<SysDocumentUpload>(transaction, query, parameters);
      return result;
    }
    #endregion

    #region Insert
    public async Task<int> Insert(IDbTransaction transaction, SysDocumentUpload model)
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
															--
															,reff_no             
															,reff_name
                              ,reff_trx_code
                              ,file_name
                              ,doc_file
                              ,mime_type
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
                              ,{p}ReffNo
                              ,{p}ReffName
                              ,{p}ReffTrxCode
                              ,{p}FileName
                              ,{p}DocFile
                              ,{p}MimeType
                            )";
      return await _command.Insert(transaction, query, model);
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(IDbTransaction transaction, SysDocumentUpload model)
    {
      var p = db.Symbol();

      string query = $@"
												update {tableBase} 
												set
														file_name				    =	{p}FileName
                            ,doc_file					  =	{p}DocFile
														--
														,mod_date           = {p}ModDate
														,mod_by             = {p}ModBy
														,mod_ip_address     = {p}ModIpAddress
												where
														id = {p}ID";
      return await _command.Update(transaction, query, model);
    }
    #endregion

    #region DeleteByID
    public async Task<int> DeleteByID(IDbTransaction transaction, string id)
    {
      var p = db.Symbol();

      string query = $@"
												delete from {tableBase}
												where
														id = {p}ID";
      return await _command.DeleteByID(transaction, query, id);
    }
    #endregion

    #region DeleteByFileName
    public async Task<int> DeleteByFileName(IDbTransaction transaction, string fileName)
    {
      var p = db.Symbol();

      string query = $@"
												delete from {tableBase}
												where
														file_name = {p}FileName";
      return await _command.Delete(transaction, query, new
      {
        FileName = fileName
      });
    }
    #endregion

    #region DeleteByTrxAndFileName
    public async Task<int> DeleteByTrxAndFileName(IDbTransaction transaction, string trxCode, string fileName)
    {
      var p = db.Symbol();

      string query = $@"
												delete from {tableBase}
												where
														file_name = {p}FileName 
                        and
                        reff_trx_code = {p}ReffTrxCode";
      return await _command.Delete(transaction, query, new
      {
        FileName = fileName,
        ReffTrxCode = trxCode
      });
    }
    #endregion
  }
}