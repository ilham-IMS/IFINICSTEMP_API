using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class MessagePaymentRequestDetail : BaseModel
    {
        public string? PaymentRequestID { get; set; }
        public string? BranchID { get; set; }
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
        public string? GLLinkID { get; set; }
        public string? GLLinkCode { get; set; }
        public string? GLLinkName { get; set; }
        public string? AgreementNo { get; set; }
        public string? FacilitiyID { get; set; }
        public string? FacilitiyCode { get; set; }
        public string? FacilitiyName { get; set; }
        public string? PurposeLoanID { get; set; }
        public string? PurposeLoanCode { get; set; }
        public string? PurposeLoanName { get; set; }
        public string? PurposeLoanDetailID { get; set; }
        public string? PurposeLoanDetailCode { get; set; }
        public string? PurposeLoanDetailName { get; set; }
        public string? OrigCurrencyID { get; set; }
        public string? OrigCurrencyCode { get; set; }
        public string? OrigCurrencyName { get; set; }
        public decimal? EXCHRate { get; set; }
        public decimal? OrigAmount { get; set; }
        public string? DivisionID { get; set; }
        public string? DivisionCode { get; set; }
        public string? DivisionName { get; set; }
        public string? DepartmentID { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DepartmentName { get; set; }
        public string? Remarks { get; set; }
        public int? IsTaxable { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TaxPct { get; set; }

        //
        public string? PaymentTransactionID { get; set; }
        //
        public string? ClientID { get; set; }
        public string? ClientName { get; set; }
    }
}