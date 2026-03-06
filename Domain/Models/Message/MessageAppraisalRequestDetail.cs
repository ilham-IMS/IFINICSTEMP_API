namespace Domain.Models.Message
{
  public class MessageAppraisalRequestDetail : BaseModel
  {
    public string? SurveyRequestID { get; set; }
    public string? CriteriaID { get; set; }
    public string? CriteriaCode { get; set; }
    public string? CriteriaDescription { get; set; }
    public string? CriteriaValue { get; set; }
  }
}