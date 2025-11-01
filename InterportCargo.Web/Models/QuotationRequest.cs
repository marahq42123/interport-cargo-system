using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Represents a quotation request submitted by a customer OR a guest.
    /// </summary>
    public class QuotationRequest
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nullable – because a request can be made by a guest (no login).
        /// </summary>
        public int? CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        /// <summary>
        /// Saves customer name directly for display even if account gets deleted.
        /// </summary>
        public string? CustomerName { get; set; }

        [Required, StringLength(120)]
        public string Source { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string Destination { get; set; } = string.Empty;

        [Required, StringLength(12)]
        public string ContainerType { get; set; } = "20GP";

        [Range(1, 999)]
        public int NumberOfContainers { get; set; } = 1;

        [Required, StringLength(160)]
        public string PackageNature { get; set; } = "General";

        [Required, StringLength(300)]
        public string JobNature { get; set; } = "Import";

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public QuotationStatus Status { get; set; } = QuotationStatus.Pending;

        [StringLength(1000)]
        public string? OfficerMessage { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
