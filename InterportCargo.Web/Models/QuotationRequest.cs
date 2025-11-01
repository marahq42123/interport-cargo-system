using System;
using System.ComponentModel.DataAnnotations;

namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Represents a quotation request submitted by a customer (User Story I2).
    /// This includes shipment details, routing, type of cargo, and processing status.
    /// </summary>
    public class QuotationRequest
    {
        /// <summary>
        /// Primary key for the quotation request in the database.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ID of the customer who submitted this request (foreign key).
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Optional snapshot of the customer's full name at the time of request.
        /// Useful for display purposes without extra database calls.
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// Origin point of the shipment (e.g., Sydney, Australia).
        /// </summary>
        [Required, StringLength(120)]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Destination point of the shipment (e.g., Dubai, UAE).
        /// </summary>
        [Required, StringLength(120)]
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Type of container used for this shipment (20GP, 40GP, or LCL).
        /// </summary>
        [Required, StringLength(12)]
        public string ContainerType { get; set; } = "20GP";

        /// <summary>
        /// Number of containers required for the request. Must be at least 1.
        /// </summary>
        [Range(1, 999)]
        public int NumberOfContainers { get; set; } = 1;

        /// <summary>
        /// Brief description of the type of goods or package (e.g., Machinery, Furniture).
        /// </summary>
        [Required, StringLength(160)]
        public string PackageNature { get; set; } = "General";

        /// <summary>
        /// Nature of the job (e.g., Import, Export, Packing, Unpacking, Quarantine handling).
        /// </summary>
        [Required, StringLength(300)]
        public string JobNature { get; set; } = "Import";

        /// <summary>
        /// Optional additional notes or special instructions provided by the customer.
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Status of the quotation request. Values include: Pending, Approved, or Rejected.
        /// </summary>
        [Required, StringLength(16)]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// The UTC timestamp when the request was initially created.
        /// </summary>
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
