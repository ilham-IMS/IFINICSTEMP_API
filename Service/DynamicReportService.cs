using Domain.Abstract.Service;
using Domain.Abstract.Repository;
using Domain.Models;
using iFinancing360.Service.Helper;
using ClosedXML.Excel;
using System.Data;
using MoreLinq;
using System.Text.RegularExpressions;
using DotNetEnv;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using iFinancing360.Helper;
using System.Globalization;


namespace Service
{
  public class DynamicReportService(
    IDynamicReportRepository _repo,
    IDynamicReportExtRepository _repoExt,
    IDynamicReportTableRepository _dynamicReportTableRepo,
    IDynamicReportTableRelationRepository _dynamicReportTableRelationRepo,
    IDynamicReportColumnRepository _dynamicReportColumnRepo,
    IMasterDynamicReportColumnRepository _masterDynamicReportColumnRepo,
    IDynamicReportParameterRepository _dynamicReportParameterRepo,
    IDynamicReportColumnOrderRepository _dynamicReportColumnOrderRepo
  ) : BaseService, IDynamicReportService
  {
    #region DeleteByID
    public async Task<int> DeleteByID(string[] idList)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        int result = 0;
        foreach (var id in idList)
        {
          result += await _repo.DeleteByID(transaction, id);
        }

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region GetRowByID
    public async Task<DynamicReport> GetRowByID(string id)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var result = await _repo.GetRowByID(transaction, id);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region GetRowsPublished
    public async Task<List<DynamicReport>> GetRowsPublishedByUser(string? keyword, int offset, int limit, string userCode)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var result = await _repo.GetRowsPublishedByUser(transaction, keyword, offset, limit, userCode);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region GetRows
    public async Task<List<DynamicReport>> GetRows(string? keyword, int offset, int limit)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var result = await _repo.GetRows(transaction, keyword, offset, limit);

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region Insert
    public async Task<int> Insert(DynamicReport model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        model.Title = model.Title?.ToUpper();
        model.IsPublished = -1;
        var result = await _repo.Insert(transaction, model);

        var tempProperties = model.Properties;
        model.Properties = null;
        await InsertExt(transaction, tempProperties, model); // Panggil InsertExt()

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion 

    #region Print
    public async Task<object> Print(DynamicReport model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        var parameters = model.Parameters;
        model = await _repo.GetRowByID(transaction, model.ID ?? throw new Exception("ID cannot be null")) ?? throw new Exception("Data not found");

        List<DynamicReportColumn> userColumns = await _dynamicReportColumnRepo.GetRowsByDynamicReport(transaction, model.ID ?? "");

        List<DynamicReportParameter> userParameters = await _dynamicReportParameterRepo.GetRowsByDynamicReport(transaction, model.ID ?? "");

        List<string> headers = userColumns.OrderBy(x => x.OrderKey).Select(x => x.HeaderTitle ?? "").ToList();

        string query = await _repo.GetQuery(transaction, model.ID!);

        var data = await _repo.GetDataFromQuery(transaction, query, parameters!);

        List<DynamicReportColumn> shouldBeMaskColumns = userColumns.FindAll(x => x.IsMasking == 1);

        foreach (var column in shouldBeMaskColumns)
        {
          data.ForEach(x => x[column.HeaderTitle ?? ""] = Mask(x[column.HeaderTitle ?? ""]?.ToString(), 6));
        }

        // parameters = userParameters.ToDictionary(x => x.Label!, x => parameters![x.Name!]);
        // using (var memoryStream = new MemoryStream())
        // {
        //   var convertedData = data?
        //     .Select(d => d.ToDictionary(kvp => kvp.Key, kvp => (object)(kvp.Value ?? "")))
        //     .ToList() ?? new List<Dictionary<string, object>>();

        //   // byte[] excelBytes = GenerateExcelWithOpenXml(model.Title ?? "Report", headers, convertedData);
        //   var paramDict = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)(kvp.Value ?? ""));

        //   byte[] excelBytes = GenerateExcelWithOpenXml(model.Title ?? "Report", headers, convertedData, paramDict);

        //   transaction.Commit();

        //   return new FileInput
        //   {
        //     Content = excelBytes,
        //     Name = (model.Title ?? "Report").Replace(" ", "_") + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".xlsx"
        //   };
        // }
        // Buat dictionary sementara yang menyimpan Value + IsDefaultValue
        var paramWithDefault = userParameters.ToDictionary(
            x => x.Label!,
            x => new
            {
              Value = parameters![x.Name!],
              IsDefaultValue = x.IsDefaultValue ?? 0
            }
        );

        using (var memoryStream = new MemoryStream())
        {
          var convertedData = data?
              .Select(d => d.ToDictionary(kvp => kvp.Key, kvp => (object)(kvp.Value ?? "")))
              .ToList() ?? new List<Dictionary<string, object>>();

          // Filter dan konversi kembali ke bentuk Dictionary<string, object?>
          var paramDict = paramWithDefault
              .Where(kvp => kvp.Value.IsDefaultValue == -1)
              .ToDictionary(
                  kvp => kvp.Key,
                  kvp => (object?)(kvp.Value.Value ?? "")
              );

          byte[] excelBytes = GenerateExcelWithOpenXml(model.Title ?? "Report", headers, convertedData, paramDict);

          transaction.Commit();

          return new FileInput
          {
            Content = excelBytes,
            Name = (model.Title ?? "Report").Replace(" ", "_") + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".xlsx"
          };
        }
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region GetQuery
    public async Task<object> GetQuery(string ID)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        List<DynamicReportTable> userTables = await _dynamicReportTableRepo.GetRowsByDynamicReport(transaction, ID ?? "");

        if (userTables.Count <= 0) throw new Exception("Please add table first.");

        userTables.ForEach(table => table.RelatedTables = [.. _dynamicReportTableRelationRepo.GetRows(transaction, table.ID!).Result]);

        List<DynamicReportColumn> userColumns = await _dynamicReportColumnRepo.GetRowsByDynamicReport(transaction, ID ?? "");

        if (userColumns.Count <= 0) throw new Exception("Please add column first.");

        List<DynamicReportParameter> userParameters = await _dynamicReportParameterRepo.GetRowsByDynamicReport(transaction, ID ?? "");

        List<DynamicReportColumnOrder> userColumnOrders = await _dynamicReportColumnOrderRepo.GetRowsByDynamicReport(transaction, ID ?? "");

        List<MasterDynamicReportColumn> masterColumns = await _masterDynamicReportColumnRepo.GetRowsByTables(transaction, userTables.Select(x => x.MasterDynamicReportTableID ?? ""));


        var (errors, query) = GenerateQuery(userTables, userColumns, masterColumns, userParameters, userColumnOrders);


        transaction.Commit();
        return new
        {
          Errors = errors,
          Query = query
        };
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region UpdateByID
    public async Task<int> UpdateByID(DynamicReport model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        model.Title = model.Title?.ToUpper();
        var result = await _repo.UpdateByID(transaction, model);

        result += await _repo.UpdateByID(transaction, model);

        // Update Dynamic Form data
        var extProperties = await _repoExt.GetRowForParent(transaction, model.ID!);
        var tempProperties = model.Properties;
        model.Properties = null;
        await UpdateExt(transaction, tempProperties, extProperties, model); // Panggil UpdateExt()

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region ChangePublishStatus
    public async Task<int> ChangePublishStatus(DynamicReport model)
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        List<string> listerrors = [];

        var result = await _repo.ChangePublishedStatus(transaction, model);
        var row = await _repo.GetRowByID(transaction, model.ID!);

        if (row.IsPublished == 1)
        {
          var tempValidateTableRelation = await _dynamicReportTableRepo.GetRowsForValidateTableRelation(transaction, model.ID ?? "");
          if (tempValidateTableRelation.Count > 0)
          {
            foreach (var dataValidate in tempValidateTableRelation)
            {
              listerrors.Add($@"Table {dataValidate.Alias} is marked as Reference but has invalid relation: Please complete ColumnSource or ColumnReference.");
            }
          }
          if (listerrors.Count > 0) throw new Exception(string.Join(";\n", listerrors));

          List<DynamicReportTable> userTables = await _dynamicReportTableRepo.GetRowsByDynamicReport(transaction, model.ID ?? "");

          if (userTables.Count <= 0) throw new Exception("Please add table first.");

          userTables.ForEach(table => table.RelatedTables = [.. _dynamicReportTableRelationRepo.GetRows(transaction, table.ID!).Result]);

          List<DynamicReportColumn> userColumns = await _dynamicReportColumnRepo.GetRowsByDynamicReport(transaction, model.ID ?? "");

          if (userColumns.Count <= 0) throw new Exception("Please add column first.");

          List<DynamicReportParameter> userParameters = await _dynamicReportParameterRepo.GetRowsByDynamicReport(transaction, model.ID ?? "");

          List<DynamicReportColumnOrder> userColumnOrders = await _dynamicReportColumnOrderRepo.GetRowsByDynamicReport(transaction, model.ID ?? "");

          List<MasterDynamicReportColumn> masterColumns = await _masterDynamicReportColumnRepo.GetRowsByTables(transaction, userTables.Select(x => x.MasterDynamicReportTableID ?? ""));

          var (errors, query) = GenerateQuery(userTables, userColumns, masterColumns, userParameters, userColumnOrders);

          if (errors.Count > 0)
          {
            throw new Exception(errors.First().Message);
          }

          model.Query = query;

          result += await _repo.UpdateQuery(transaction, model);
        }

        transaction.Commit();
        return result;
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
    #endregion

    #region Mask
    private static string? Mask(string? value, int maskLength)
    {
      if (string.IsNullOrWhiteSpace(value)) return null;
      return value.FirstOrDefault() + new string('*', maskLength) + value.LastOrDefault();
    }
    #endregion

    #region GenerateQuery
    private static (List<Exception>, string) GenerateQuery(List<DynamicReportTable> userTables, List<DynamicReportColumn> userColumns, List<MasterDynamicReportColumn> masterColumns, List<DynamicReportParameter> userParameters, List<DynamicReportColumnOrder> columnOrders)
    {
      List<Exception> errors = []; // List untuk menampung error
      List<string> columnsQueries = []; // List untuk menampung query column
      List<string> tablesQueries = []; // List untuk menampung query table
      List<string> whereQueries = []; // List untuk menampung query where
      List<string> orderQueries = []; // List untuk menampung query order
      List<string> groupByQueries = []; // List untuk menampung query order


      string selectQuery = ""; // Query Select
      string tableQuery = ""; // Query Table
      string whereQuery = ""; // Query Where
      string orderQuery = ""; // Query Order
      string groupByQuery = ""; // Query Group

      var tables = ReOrderTables(userTables, masterColumns);

      // Format Column Name
      userColumns.ForEach(x =>
      {

        var table = tables.Find(y => x.DynamicReportTableID == y.ID);
        // Jika Menggunakan Group Function 

        if (x.Formula is not null)
        {
          string input = x.Formula;
          string pattern = @"\{([^}]+)\}";
          MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(10));

          matches.ForEach(match =>
          {
            string value = match.Value.Replace("{", "").Replace("}", "");
            string[] split = value.Split(".");
            string tableAlias = split.First();
            string columnAlias = split.Last();

            var masterColumn = masterColumns.Find(mc => mc.Name?.Equals(columnAlias, StringComparison.OrdinalIgnoreCase) == true && tables.Any(y => y.Alias?.Equals(tableAlias, StringComparison.OrdinalIgnoreCase) == true && y.MasterDynamicReportTableID == mc.MasterDynamicReportTableID));

            if (masterColumn == null)
            {
              errors.Add(new Exception($"Column {value} not found in MasterColumn"));
            }


            if (masterColumn?.IsMasking == 1)
            {
              x.IsMasking = 1;
            }

            input = input.Replace(match.Value, $"\"{tableAlias}\".{columnAlias}");
          });

          x.ColumnName = input;

          if (x.GroupFunction is not null)
            x.ColumnName = $"{x.GroupFunction}({x.ColumnName})";
        }
        else
          x.ColumnName = $"\"{table?.Alias}\".{x.ColumnName}";

        if (x.GroupFunction is not null)
          x.ColumnName = $"{x.GroupFunction}({x.ColumnName})";
      });

      // Formatting Parameters
      userParameters.ForEach(param =>
      {
        if (param.Formula is not null)
        {
          string input = param.Formula;
          string pattern = @"\{([^}]+)\}";
          MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(10));

          matches.ForEach(match =>
          {
            string value = match.Value.Replace("{", "").Replace("}", "");
            string[] split = value.Split(".");
            string tableAlias = split.First();
            string columnAlias = split.Last();

            var masterColumn = masterColumns.Find(mc => mc.Name?.Equals(columnAlias, StringComparison.OrdinalIgnoreCase) == true && tables.Any(y => y.Alias?.Equals(tableAlias, StringComparison.OrdinalIgnoreCase) == true && y.MasterDynamicReportTableID == mc.MasterDynamicReportTableID));

            if (masterColumn == null)
            {
              errors.Add(new Exception($"Column {value} not found in MasterColumn"));
            }

            input = input.Replace(match.Value, $"\"{tableAlias}\".{columnAlias}");
          });

          param.ColumnName = input;
        }
        else
        {
          var masterColumn = masterColumns.Find(mc => mc.ID == param.MasterDynamicReportColumnID);
          var table = tables.Find(y => param.DynamicReportTableID == y.ID);
          param.ColumnName = $"\"{table?.Alias}\".{masterColumn?.Name}";

        }
      });

      // Menambahkan query column dengan aliasnya
      columnsQueries.AddRange(userColumns.Select(x => $"{x.ColumnName} as \"{x.HeaderTitle}\""));

      // Menambahkan query Where
      whereQueries.AddRange(userParameters.Select(x => $"{x.ColumnName} {x.Operator} @{x.Name}"));

      // Menambahkan query column Group By
      if (userColumns.Any(x => x.GroupFunction is not null))
      {
        groupByQueries.AddRange(userColumns.Where(x => x.GroupFunction is null).Select(x => $"{x.ColumnName}"));
      }

      // Menambahkan query orderBy
      if (columnOrders.Count > 0)
      {
        orderQueries.AddRange(columnOrders.Select(order =>
        {
          var column = userColumns.Find(x => x.ID == order.DynamicReportColumnID);
          return $"{column?.ColumnName} {order.OrderBy}";
        }));
      }


      // Mapping table dan columns
      var tablesWithColumns = tables.Select(table => new
      {
        table.TableName,
        table.MasterDynamicReportTableID,
        TableAlias = table.Alias,
        table.RelatedTables,
        table.JoinClause,
        // table.AdditionalJoinClause
      });



      // Looping table untuk membentuk query
      foreach (var (table, i) in tablesWithColumns.Select((table, index) => (table, index)))
      {

        if (table.TableName is null) continue;

        // Menambahkan query table
        string tableQ = $"{table.TableName} \"{table.TableAlias}\"";

        // Index lebih dari 0 sebagai Join
        if (i > 0)
        {
          if (table.RelatedTables.Count == 0) errors.Add(new Exception($"Relation could not be found for {table.TableAlias}"));

          // Aggregate Join Query
          string referenceJoin = string.Join(" and ", table.RelatedTables.Select(x =>
          {
            var t = tables.Find(y => x.ReferenceMasterDynamicReportColumnValue == y.ID);

            // return $"\"{table.TableAlias}\".{x.ColumnName} {x.Operator} \"{(!string.IsNullOrEmpty(t?.ReferenceTableAlias) ? t.ReferenceTableAlias : x.ReferenceTableAlias)}\".{x.ReferenceColumnName}";
            return $"\"{table.TableAlias}\".{x.ColumnName} {x.Operator} " +
                        (x.ReferenceMasterDynamicReportColumnID == null
                              ? x.ReferenceMasterDynamicReportColumnValue
                              : $"\"{(!string.IsNullOrEmpty(t?.ReferenceTableAlias) ? t.ReferenceTableAlias : x.ReferenceTableAlias)}\".{x.ReferenceColumnName}");
          }));

          tableQ = $"{table.JoinClause ?? "INNER"} join \n\t{tableQ} on ({referenceJoin})";
        }
        tablesQueries.Add(tableQ);
      }

      // Menggabungkan query dengan syntax sql
      selectQuery = $"select \n\t{string.Join("\n\t,", columnsQueries)}";
      tableQuery = $"\nfrom \n\t{string.Join("\n", tablesQueries)}";
      if (whereQueries.Count > 0) whereQuery = $"\nwhere \n\t{string.Join("\n\tand ", whereQueries)}";
      if (groupByQueries.Count > 0) groupByQuery = $"\ngroup by \n\t{string.Join("\n\t,", groupByQueries)}";
      if (orderQueries.Count > 0) orderQuery = $"\norder by \n\t{string.Join("\n\t,", orderQueries)}";

      // Mengembalikan output tuple error dan query
      return (errors, $"{selectQuery}{tableQuery}{whereQuery}{groupByQuery}{orderQuery}");
    }
    #endregion

    #region ReOrderTables
    private static List<DynamicReportTable> ReOrderTables(List<DynamicReportTable> reportTables, List<MasterDynamicReportColumn> masterColumns)
    {

      // Parent Table merupakan Table yang tidak memiliki element pada properti RelatedTables
      // Step 1 : Memastikan tidak ada lebih dari 1 Parent Table, Jika ada maka salah satu Parent Table diharuskan mengambil element RelatedTable dari Child nya

      // Step 2 : Sorting Table berdasarkan urutan dari properti RelatedTables
      var sorted = new List<DynamicReportTable>();
      var visited = new HashSet<DynamicReportTable>();
      var stack = new Stack<DynamicReportTable>();

      foreach (var table in reportTables)
      {
        if (visited.Contains(table)) continue;

        stack.Push(table);

        while (stack.Count > 0)
        {
          var current = stack.Peek();
          bool allParentsVisited = true;

          foreach (var parent in current.RelatedTables ?? [])
          {
            // if (visited.Any(x => x.ID == parent.ReferenceDynamicReportTableID)) continue;
            if (visited.Any(x => x.ReferenceDynamicReportTableID == null)) continue;

            // stack.Push(reportTables.Find(x => x.ID == parent.ReferenceDynamicReportTableID)!);
            stack.Push(reportTables.Find(x => x.ID == parent.ReferenceDynamicReportTableID)!);
            allParentsVisited = false;
          }

          if (allParentsVisited)
          {
            stack.Pop();

            if (visited.Contains(current)) continue;
            visited.Add(current);
            sorted.Add(current);
          }
        }
      }

      return sorted;
    }
    #endregion

    #region GenerateDynamicExcel
    private static XLWorkbook GenerateDynamicExcel(DynamicReport model, List<string> headers, List<IDictionary<string, object>> data, IDictionary<string, object> parameters)
    {
      var workbook = new XLWorkbook();
      var worksheet = workbook.Worksheets.Add("Report");


      int posY = 1;


      // Title
      worksheet.Cell(posY, 1).Value = model.Title;
      worksheet.Range(posY, 1, 1, headers.Count).Merge();
      worksheet.Cell(posY, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
      worksheet.Cell(posY, 1).Style.Font.Bold = true;
      worksheet.Cell(posY, 1).Style.Font.FontSize = 14;
      posY += 2;

      if (parameters.Count > 0)
      {
        var keys = parameters.Keys.ToList();
        for (int i = posY; i < parameters.Count + posY; i++)
        {
          var key = keys[i - posY];
          var value = parameters[key];

          worksheet.Cell(i, 1).Value = key;
          FormatCell(worksheet.Cell(i, 2), value);
        }

        posY += parameters.Count;
      }



      var range = worksheet.Range(posY, 1, data.Count + posY, headers.Count);

      range.CreateTable();

      for (int i = 0; i < headers.Count; i++)
      {
        // Headers
        worksheet.Cell(posY, i + 1).Value = headers[i];
        worksheet.Cell(posY, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell(posY, i + 1).Style.Font.Bold = true;

        int pos = posY + 1;
        // Data 
        for (int j = pos; j < data.Count + pos; j++)
        {
          var d = data[j - pos];
          FormatCell(worksheet.Cell(j, i + 1), d[headers[i]]);
        }
      }

      worksheet.Columns().AdjustToContents();
      worksheet.Rows().AdjustToContents();

      return workbook;
    }
    #endregion

    #region FormatCell
    private static void FormatCell(IXLCell cell, object? value)
    {
      if (value?.GetType() == typeof(DateTime))
      {
        cell.Value = ((DateTime)value).ToString("dd/MM/yyyy");
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
      }
      else if (value?.GetType() == typeof(int))
      {
        cell.Value = (int)value;
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
      }
      else if (value?.GetType() == typeof(decimal))
      {
        cell.Value = (decimal)value;
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        cell.Style.NumberFormat.Format = "#,##0.00";
      }
      else
      {
        cell.Value = value?.ToString();
      }
    }

    #endregion

    public async Task<FileDoc> GetReportData()
    {
      using var connection = _repo.GetDbConnection();
      using var transaction = connection.BeginTransaction();

      try
      {
        string title = "Dynamic Report User List";
        List<string> headers = new() { "Title" }; // Header disesuaikan dengan jumalh kolom yang ditampilkan dan menggunakan PascalCase

        var reportData = await _repo.GetReportData(transaction);
        // Jika data pada database ingin dirubah misalnya 1 menjadi Yes dapat menggunakan cara mapping seperti dibawah ini
        var formattedData = reportData.Select(tempData => new
        {
          Title = tempData.Title,
        }).ToList();

        MemoryStream ms = new MemoryStream();
        ms = await GenerateExcelTemplateWithData(title, headers, formattedData);

        transaction.Commit();
        return new FileDoc
        {
          Content = ms.ToArray(),
          Name = $"dynamic_report_user_{Static.FileTimeStamp}.xlsx",
          MimeType = FileMimeType.Xlsx
        };

      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
    }

    private byte[] GenerateExcelWithOpenXml(
    string title,
    List<string> headers,
    List<Dictionary<string, object>> data,
    IDictionary<string, object?>? parameters = null)
    {
      using (var memoryStream = new MemoryStream())
      {
        using (var spreadsheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
          WorkbookPart workbookPart = spreadsheet.AddWorkbookPart();
          workbookPart.Workbook = new Workbook();
          WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
          SheetData sheetData = new SheetData();

          // Add Sheet
          Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
          Sheet sheet = new Sheet()
          {
            Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = "Report"
          };
          sheets.Append(sheet);

          // ===== TITLE ROW =====
          Row titleRow = new Row();
          titleRow.Append(new Cell
          {
            CellValue = new CellValue(title),
            DataType = CellValues.String
          });
          sheetData.Append(titleRow);

          // ===== PARAMETER ROWS =====
          if (parameters != null && parameters.Count > 0)
          {
            foreach (var param in parameters)
            {
              Row paramRow = new Row();

              // First cell = parameter name
              paramRow.Append(new Cell
              {
                CellValue = new CellValue(param.Key),
                DataType = CellValues.String
              });

              // Second cell = parameter value (formatted like data rows)
              if (param.Value is DateTime dt)
              {
                paramRow.Append(new Cell
                {
                  CellValue = new CellValue(dt.ToString("dd/MM/yyyy")),
                  DataType = CellValues.String
                });
              }
              else if (param.Value is IFormattable num)
              {
                paramRow.Append(new Cell
                {
                  CellValue = new CellValue(num.ToString(null, CultureInfo.InvariantCulture)),
                  DataType = CellValues.Number
                });
              }
              else
              {
                paramRow.Append(new Cell
                {
                  CellValue = new CellValue(param.Value?.ToString() ?? ""),
                  DataType = CellValues.String
                });
              }

              sheetData.Append(paramRow);
            }
          }

          // Empty row before table
          sheetData.Append(new Row());

          // ===== HEADER ROW =====
          Row headerRow = new Row();
          foreach (var header in headers)
          {
            headerRow.Append(new Cell
            {
              CellValue = new CellValue(header),
              DataType = CellValues.String
            });
          }
          sheetData.Append(headerRow);

          // ===== DATA ROWS =====
          foreach (var rowDict in data)
          {
            Row newRow = new Row();
            foreach (var header in headers)
            {
              var value = rowDict[header];

              if (value is DateTime dt)
              {
                newRow.Append(new Cell
                {
                  CellValue = new CellValue(dt.ToString("dd/MM/yyyy")),
                  DataType = CellValues.String
                });
              }
              else if (value is IFormattable num)
              {
                newRow.Append(new Cell
                {
                  CellValue = new CellValue(num.ToString(null, CultureInfo.InvariantCulture)),
                  DataType = CellValues.Number
                });
              }
              else
              {
                newRow.Append(new Cell
                {
                  CellValue = new CellValue(value?.ToString() ?? ""),
                  DataType = CellValues.String
                });
              }
            }
            sheetData.Append(newRow);
          }

          worksheetPart.Worksheet = new Worksheet(sheetData);
          workbookPart.Workbook.Save();
        }
        return memoryStream.ToArray();
      }
    }

    #region GetRowForParent
		public async Task<List<ExtendModel>> GetRowForParent(string ParentID)
		{
		  using var connection = _repo.GetDbConnection();
		  using var transaction = connection.BeginTransaction();
		
		  try
		  {
			DynamicReport model = await _repo.GetRowByID(transaction, ParentID);
		
			var result = await _repoExt.GetRowForParent(transaction, model.ID);
		
			transaction.Commit();
			return result;
		  }
		  catch (Exception)
		  {
			transaction.Rollback();
			throw;
		  }
		}
		#endregion

    #region InsertExt
		private async Task<int> InsertExt(IDbTransaction transaction, dynamic tempProperties, DynamicReport model)
		{
		  var properties = await DeserializeProperties(tempProperties);
		
		  if (properties == null) return 0;
		
		  int resultExt = 0;
		
		  foreach (var property in properties)
		  {
			var extendModel = new ExtendModel
			{
			  ID = Guid.NewGuid().ToString("N").ToLower(),
			  CreDate = model.CreDate,
			  CreBy = model.CreBy,
			  CreIPAddress = model.CreIPAddress,
			  ModDate = model.ModDate,
			  ModBy = model.ModBy,
			  ModIPAddress = model.ModIPAddress,
			  ParentID = model.ID,
			  Keyy = property.Key,
			  Value = property.Value,
			  Properties = null
			};
		
			resultExt += await _repoExt.Insert(transaction, extendModel);
		  }
		
		  return resultExt;
		}
		#endregion

    #region UpdateExt
		private async Task<int> UpdateExt<T>(IDbTransaction transaction, dynamic tempProperties, List<ExtendModel> extProperties, T model) where T : ExtendModel
		{
		  int result = 0;
		
		  var properties = await DeserializeProperties(tempProperties);
		
		  foreach (var property in properties)
		  {
			var keyy = property.Key;
			var value = property.Value;
		
			if (value is not string)
			{
			  value = value.ToString();
			}
		
			if (extProperties.Any(e => e.Keyy == keyy))
			{
			  var extendModel = new ExtendModel
			  {
				ParentID = model.ID,
				ModDate = model.ModDate,
				ModBy = model.ModBy,
				ModIPAddress = model.ModIPAddress,
				Keyy = keyy,
				Value = value,
				Properties = null
			  };
		
			  result += await _repoExt.UpdateByID(transaction, extendModel);
			}
			else
			{
			  var extendModel = new ExtendModel
			  {
				ID = Guid.NewGuid().ToString("N").ToLower(),
				CreDate = model.CreDate,
				CreBy = model.CreBy,
				CreIPAddress = model.CreIPAddress,
				ModDate = model.ModDate,
				ModBy = model.ModBy,
				ModIPAddress = model.ModIPAddress,
				ParentID = model.ID,
				Keyy = keyy,
				Value = value,
				Properties = null
			  };
		
			  result += await _repoExt.Insert(transaction, extendModel);
			}
		  }
		
		  return result;
		}
		#endregion

  }
}