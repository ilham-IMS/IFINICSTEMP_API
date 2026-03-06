// using iFinancing360.API.Helper;
// using Microsoft.AspNetCore.Mvc;
// using Domain.Models;
// using Domain.Abstract.Service;
// using iFinancing360.Helper;
// using System.Text.Json;
// using System.Text.Json.Nodes;
// using Domain.Abstract.Repository;

// namespace API.Controllers
// {
//     [Route("/api/[controller]")]
//     [SetBaseModelProperties]
//   [DecryptQueryString]
// 	[DecryptRequestBody]
//     [ApiController]
//     public class ChartController(
//       IChartService service,

//       IConfiguration configuration,

//       InternalAPIClient internalAPIClient

//     ) : BaseController(configuration)
//     {
//         private readonly IChartService _service = service;
//         private readonly InternalAPIClient _internalAPIClient = internalAPIClient;

//         [HttpGet("GetSurveyReqBySurveyor")]
//         public async Task<IActionResult> GetSurveyReqBySurveyor([FromQuery] ChartConfig chartFilter)
//         {
//             var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
//             try
//             {

//                 var GetUserBranch = await _internalAPIClient.GetRows("IFINSYS", "SysBranch", "GetALLUserBranch", new { sysEmployeeCode = GetUser() }, headers);

//                 var userBranchList = GetUserBranch.Data ?? new List<JsonObject>();
//                 var result = await _service.GetSurveyReqBySurveyor(chartFilter, userBranchList.Select(x => x["ID"]!.ToString()).ToArray());
//                 return ResponseSuccess(result);
//             }
//             catch (Exception ex)
//             {
//                 return ResponseError(ex);
//             }
//         }

//         [HttpGet("GetSurveyReqByBranch")]
//         public async Task<IActionResult> GetSurveyReqByBranch([FromQuery] ChartConfig chartFilter)
//         {
//             var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
//             try
//             {
//                 var GetUserBranch = await _internalAPIClient.GetRows("IFINSYS", "SysBranch", "GetALLUserBranch", new { sysEmployeeCode = GetUser() }, headers);

//                 var userBranchList = GetUserBranch.Data ?? new List<JsonObject>();
//                 var result = await _service.GetSurveyReqByBranch(chartFilter, userBranchList.Select(x => x["ID"]!.ToString()).ToArray());
//                 return ResponseSuccess(result);
//             }
//             catch (Exception ex)
//             {
//                 return ResponseError(ex);
//             }
//         }

//         [HttpGet("GetAppraisalReqByBranch")]
//         public async Task<IActionResult> GetAppraisalReqByBranch([FromQuery] ChartConfig chartFilter)
//         {
//             var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
//             try
//             {
//                 var GetUserBranch = await _internalAPIClient.GetRows("IFINSYS", "SysBranch", "GetALLUserBranch", new { sysEmployeeCode = GetUser() }, headers);

//                 var userBranchList = GetUserBranch.Data ?? new List<JsonObject>();
//                 var result = await _service.GetAppraisalReqByBranch(chartFilter, userBranchList.Select(x => x["ID"]!.ToString()).ToArray());
//                 return ResponseSuccess(result);
//             }
//             catch (Exception ex)
//             {
//                 return ResponseError(ex);
//             }
//         }

//         [HttpGet("GetAppraisalReqByAppraiser")]
//         public async Task<IActionResult> GetAppraisalReqByAppraiser([FromQuery] ChartConfig chartFilter)
//         {
//             var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
//             try
//             {
//                 var GetUserBranch = await _internalAPIClient.GetRows("IFINSYS", "SysBranch", "GetALLUserBranch", new { sysEmployeeCode = GetUser() }, headers);

//                 var userBranchList = GetUserBranch.Data ?? new List<JsonObject>();
//                 var result = await _service.GetAppraisalReqByAppraiser(chartFilter, userBranchList.Select(x => x["ID"]!.ToString()).ToArray());
//                 return ResponseSuccess(result);
//             }
//             catch (Exception ex)
//             {
//                 return ResponseError(ex);
//             }
//         }
//     }
// }