// using System.Text.Json;
// using System.Text.Json.Nodes;
// using System.Text.Json.Serialization;
// using Domain.Abstract.Repository;
// using Domain.Abstract.Service;
// using Domain.Models;
// using iFinancing360.Helper;
// using iFinancing360.Service.Helper;

// // private readonly ISuspendMainService _suspendMainService = suspendMainService;
// namespace Service
// {
//     public class ChartService(ISurveyMainRepository surveyMainrepo, IAppraisalMainRepository appraisalMainrepo, GlobalConfig globalConfig) : BaseService, IChartService
//     {
//         private readonly ISurveyMainRepository _surveyMainrepo = surveyMainrepo;

//         private readonly IAppraisalMainRepository _appraisalMainrepo = appraisalMainrepo;
//         private readonly GlobalConfig _globalConfig = globalConfig;

//         #region not implemented function
//         public Task<int> DeleteByID(string[] idList)
//         {
//             throw new NotImplementedException();
//         }

//         public Task<BaseModel> GetRowByID(string id)
//         {
//             throw new NotImplementedException();
//         }

//         public Task<List<BaseModel>> GetRows(string? keyword, int offset, int limit)
//         {
//             throw new NotImplementedException();
//         }

//         public async Task<ChartConfig> GetSurveyReqBySurveyor(ChartConfig chartFilter, string[] userBranches)
//         {
//             var filterData = JsonSerializer.Deserialize<JsonObject>(chartFilter.FilterData ?? "{}") ?? [];

//             using var connection = _surveyMainrepo.GetDbConnection();
//             using var transaction = connection.BeginTransaction();

//             try
//             {
//                 // var yearFilter = filterData["Year"]?.ToString();
//                 int year = filterData["Year"] != null ? int.Parse(filterData["Year"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("yyyy") ?? DateTime.Now.Year.ToString());
//                 int month = filterData["Month"] != null ? int.Parse(filterData["Month"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("MM") ?? DateTime.Now.Month.ToString());

//                 var rows = await _surveyMainrepo.GetSurveyReqBySurveyor(transaction, year, month, userBranches);

//                 var series = rows.Select(row => new ChartOption
//                 {
//                     Name = row.DataName,
//                     Data = [(decimal)(row.DataValue ?? 0)],
//                 });

//                 if (rows.Count == 0)
//                 {
//                     return new ChartConfig
//                     {
//                         Option = new ChartOption
//                         {
//                             Name = "SURVEY REQUEST BY SURVEYOR",
//                             Series = new List<ChartOption>
//                             {
//                                 new ChartOption
//                                 {
//                                     Name = "No Data",
//                                     Data = [0]
//                                 }
//                         }

//                         }
//                     };


//                 }

//                 return new ChartConfig
//                 {
//                     Option = new ChartOption
//                     {
//                         Name = "SURVEY REQUEST BY SURVEYOR",
//                         Series = series
//                     }
//                 };
//             }
//             catch (Exception)
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }

//         public async Task<ChartConfig> GetSurveyReqByBranch(ChartConfig chartFilter, string[] userBranches)
//         {
//             var filterData = JsonSerializer.Deserialize<JsonObject>(chartFilter.FilterData ?? "{}") ?? [];

//             using var connection = _surveyMainrepo.GetDbConnection();
//             using var transaction = connection.BeginTransaction();

//             try
//             {
//                 // var yearFilter = filterData["Year"]?.ToString();
//                 int year = filterData["Year"] != null ? int.Parse(filterData["Year"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("yyyy") ?? DateTime.Now.Year.ToString());
//                 int month = filterData["Month"] != null ? int.Parse(filterData["Month"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("MM") ?? DateTime.Now.Month.ToString());

//                 var rows = await _surveyMainrepo.GetSurveyReqByBranch(transaction, year, month, userBranches);

//                 var series = rows.Select(row => new ChartOption
//                 {
//                     Name = row.DataName,
//                     Data = [(decimal)(row.DataValue ?? 0)],
//                 });

//                 if (rows.Count == 0)
//                 {
//                     return new ChartConfig
//                     {
//                         Option = new ChartOption
//                         {
//                             Name = "SURVEY REQUEST BY BRANCH",
//                             Series = new List<ChartOption>
//                             {
//                                 new ChartOption
//                                 {
//                                     Name = "No Data",
//                                     Data = [0]
//                                 }
//                         }

//                         }
//                     };


//                 }

//                 return new ChartConfig
//                 {
//                     Option = new ChartOption
//                     {
//                         Name = "SURVEY REQUEST BY BRANCH",
//                         Series = series
//                     }
//                 };
//             }
//             catch (Exception)
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }

//         public async Task<ChartConfig> GetAppraisalReqByBranch(ChartConfig chartFilter, string[] userBranches)
//         {
//             var filterData = JsonSerializer.Deserialize<JsonObject>(chartFilter.FilterData ?? "{}") ?? [];

//             using var connection = _appraisalMainrepo.GetDbConnection();
//             using var transaction = connection.BeginTransaction();

//             try
//             {
//                 // var yearFilter = filterData["Year"]?.ToString();
//                 int year = filterData["Year"] != null ? int.Parse(filterData["Year"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("yyyy") ?? DateTime.Now.Year.ToString());
//                 int month = filterData["Month"] != null ? int.Parse(filterData["Month"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("MM") ?? DateTime.Now.Month.ToString());

//                 var rows = await _appraisalMainrepo.GetAppraisalReqByBranch(transaction, year, month, userBranches);

//                 var series = rows.Select(row => new ChartOption
//                 {
//                     Name = row.DataName,
//                     Data = [(decimal)(row.DataValue ?? 0)],
//                 });

//                 if (rows.Count == 0)
//                 {
//                     return new ChartConfig
//                     {
//                         Option = new ChartOption
//                         {
//                             Name = "APPRAISAL REQUEST BY BRANCH",
//                             Series = new List<ChartOption>
//                             {
//                                 new ChartOption
//                                 {
//                                     Name = "No Data",
//                                     Data = [0]
//                                 }
//                         }

//                         }
//                     };


//                 }

//                 return new ChartConfig
//                 {
//                     Option = new ChartOption
//                     {
//                         Name = "APPRAISAL REQUEST BY BRANCH",
//                         Series = series
//                     }
//                 };
//             }
//             catch (Exception)
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }

//         public async Task<ChartConfig> GetAppraisalReqByAppraiser(ChartConfig chartFilter, string[] userBranches)
//         {
//             var filterData = JsonSerializer.Deserialize<JsonObject>(chartFilter.FilterData ?? "{}") ?? [];

//             using var connection = _appraisalMainrepo.GetDbConnection();
//             using var transaction = connection.BeginTransaction();

//             try
//             {
//                 // var yearFilter = filterData["Year"]?.ToString();
//                 int year = filterData["Year"] != null ? int.Parse(filterData["Year"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("yyyy") ?? DateTime.Now.Year.ToString());
//                 int month = filterData["Month"] != null ? int.Parse(filterData["Month"]!.ToString()!) : int.Parse(_globalConfig.SystemDate?.ToString("MM") ?? DateTime.Now.Month.ToString());

//                 var rows = await _appraisalMainrepo.GetAppraisalReqByAppraiser(transaction, year, month, userBranches);

//                 var series = rows.Select(row => new ChartOption
//                 {
//                     Name = row.DataName,
//                     Data = [(decimal)(row.DataValue ?? 0)],
//                 });

//                 if (rows.Count == 0)
//                 {
//                     return new ChartConfig
//                     {
//                         Option = new ChartOption
//                         {
//                             Name = "APPRAISAL REQUEST BY APPRAISER",
//                             Series = new List<ChartOption>
//                             {
//                                 new ChartOption
//                                 {
//                                     Name = "No Data",
//                                     Data = [0]
//                                 }
//                         }

//                         }
//                     };


//                 }

//                 return new ChartConfig
//                 {
//                     Option = new ChartOption
//                     {
//                         Name = "APPRAISAL REQUEST BY APPRAISER",
//                         Series = series
//                     }
//                 };
//             }
//             catch (Exception)
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }

//         public Task<int> Insert(BaseModel model)
//         {
//             throw new NotImplementedException();
//         }

//         public Task<int> UpdateByID(BaseModel model)
//         {
//             throw new NotImplementedException();
//         }
//         #endregion

//     }
// }