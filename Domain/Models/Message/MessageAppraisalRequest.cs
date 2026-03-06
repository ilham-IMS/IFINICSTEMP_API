namespace Domain.Models.Message
{
  public class MessageAppraisalRequest : BaseModel
  {
    public string? SurveyorID { get; set; }
    public string? Code { get; set; }
    public string? BranchID { get; set; }
    public string? BranchCode { get; set; }
    public string? BranchName { get; set; }
    public string? ReffID { get; set; }
    public string? ReffCode { get; set; }
    public string? ReffName { get; set; }
    public string? ReffObject { get; set; }
    public string? ReffRemarks { get; set; }
    public string? Status { get; set; }
    public DateTime? Date { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyID { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? SurveyFeeAmount { get; set; }
    public decimal? MarketValue { get; set; }
    public decimal? FIDAmount { get; set; }
    public DateTime? ProcessDate { get; set; }
    public string? ProcessReffNo { get; set; }
    public string? ProcessReffName { get; set; }
    public string? SurveyorCode { get; set; }
    public DateTime? SurveyResultDate { get; set; }
    public decimal? SurveyResultAmount { get; set; }
    public string? SurveyResultRemarks { get; set; }
    public string? SurveyResultValue { get; set; }
    public string? ApplicationNo { get; set; }
    public string? ContactPersonName { get; set; }
    public string? ContactPersonAreaPhoneNo { get; set; }
    public string? ContactPersonPhoneNo { get; set; }
    public string? CollateralNo { get; set; }
    public string? CollateralName { get; set; }
    public string? CollateralLocation { get; set; }
    public string? CollateralDescription { get; set; }
    
    public string? Address { get; set; }
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public string? ModuleCode { get; set; }
    public List<MessageAppraisalRequestDetail>? RequestDetails { get; set; }

    #region MasterSurveyor
    public int? IsExternal { get; set; }
    #endregion
  }
}