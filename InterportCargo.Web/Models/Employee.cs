using System;
using System.ComponentModel.DataAnnotations;

namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Represents an employee of InterportCargo who can access and perform system functions.
    /// This supports user registration and login for staff (User Story T3).
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Primary key (auto-generated ID) for the employee record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's first name.
        /// Required for account registration and identification.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Employee's family name (surname).
        /// Required for registration and identification.
        /// </summary>
        [Required]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>
        /// Email address used as username for login.
        /// Must be unique within the system.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contact number of the employee.
        /// Required field, but no strict format validation applied at this level.
        /// </summary>
        [Required]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Job role or type of the employee (Admin, Quotation Officer, Booking Officer, etc.).
        /// Determines what system functions they have access to.
        /// </summary>
        [Required]
        public EmployeeType EmployeeType { get; set; }

        /// <summary>
        /// Residential or office address of the employee.
        /// Required for account creation.
        /// </summary>
        [Required]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password stored for security.
        /// Plain text passwords should never be stored.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
