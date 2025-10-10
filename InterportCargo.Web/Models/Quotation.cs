using System;

namespace InterportCargo.Web.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public int QuotationRequestId { get; set; }
        public QuotationRequest? Request { get; set; }
        public string QuotationNumber { get; set; } = "";
        public DateTime DateIssued { get; set; } = DateTime.UtcNow;
        public string ContainerType { get; set; } = "20GP";
        public string Scope { get; set; } = "";
        public decimal ChargesBase { get; set; }
        public decimal ChargesDepot { get; set; }
        public decimal ChargesLcl { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal Total => ChargesBase + ChargesDepot + ChargesLcl - DiscountApplied;
        public QuotationStatus Status { get; set; } = QuotationStatus.Sent;
    }
}
