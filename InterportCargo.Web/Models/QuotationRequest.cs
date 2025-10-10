using System.ComponentModel.DataAnnotations;

namespace InterportCargo.Web.Models
{
    public class QuotationRequest
    {
        public int Id { get; set; }
        [Required] public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        [Required] public string Source { get; set; } = "";
        [Required] public string Destination { get; set; } = "";
        [Range(1,100)] public int NumberOfContainers { get; set; }
        [Required] public string PackageNature { get; set; } = "";
        [Required] public string JobNature { get; set; } = "";
        public QuotationStatus Status { get; set; } = QuotationStatus.Pending;
        public string? OfficerMessage { get; set; }
    }
}
