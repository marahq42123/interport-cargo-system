using System;
using System.ComponentModel.DataAnnotations;

namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Represents a customer of the Interport Cargo system.
    /// Stores essential user information required for login,
    /// quotation requests, and future system interactions.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unique primary key for the customer in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Customer’s first name. Cannot be empty.
        /// </summary>
        [Required, Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Customer’s family/last name. Cannot be empty.
        /// </summary>
        [Required, Display(Name = "Family Name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address used as the username for login.
        /// Must be a valid email format and unique.
        /// </summary>
        [Required, EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Customer’s phone number (optional but validated if entered).
        /// </summary>
        [Phone, StringLength(30)]
        public string? Phone { get; set; }

        /// <summary>
        /// Optional company name if the customer belongs to an organization.
        /// </summary>
        [Display(Name = "Company Name"), StringLength(100)]
        public string? CompanyName { get; set; }

        /// <summary>
        /// Optional physical address of the customer.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }

        /// <summary>
        /// Securely hashed password. Plain text should never be stored.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// The date and time this customer’s account was created.
        /// Stored in UTC for consistency.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
