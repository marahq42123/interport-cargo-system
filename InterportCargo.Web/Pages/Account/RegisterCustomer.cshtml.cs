using System.ComponentModel.DataAnnotations;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Account
{
    /// <summary>
    /// Handles customer registration, including:
    /// - Validating input
    /// - Checking for duplicate emails
    /// - Hashing passwords
    /// - Storing new customers in the database
    /// - Setting login session after successful registration.
    /// </summary>
    public class RegisterCustomerModel : PageModel
    {
        private readonly InterportContext _db;

        /// <summary>
        /// Injects the database context for persistence operations.
        /// </summary>
        public RegisterCustomerModel(InterportContext db) => _db = db;

        /// <summary>
        /// Input data entered by the user in the registration form.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = new();

        /// <summary>
        /// Optional field used to display custom error messages on the page.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Displays the registration page (GET request).
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// Handles form submissions (POST request).
        /// If valid, creates a new customer account, hashes the password,
        /// saves the user to the database, and logs them in via session.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // 1. Normalize email and check if user already exists
            var emailNorm = Input.Email.Trim().ToLowerInvariant();
            var exists = await _db.Customers.AnyAsync(c => c.Email.ToLower() == emailNorm);
            if (exists)
            {
                ModelState.AddModelError("Input.Email", "An account with this email already exists.");
                return Page();
            }

            // 2. Hash password securely
            var hash = BCrypt.Net.BCrypt.HashPassword(Input.Password);

            // 3. Create Customer object
            var c = new InterportCargo.Web.Models.Customer
            {
                FirstName    = Input.FirstName.Trim(),
                LastName     = Input.LastName.Trim(),
                Email        = emailNorm,
                Phone        = string.IsNullOrWhiteSpace(Input.Phone) ? null : Input.Phone.Trim(),
                CompanyName  = string.IsNullOrWhiteSpace(Input.CompanyName) ? null : Input.CompanyName.Trim(),
                Address      = string.IsNullOrWhiteSpace(Input.Address) ? null : Input.Address.Trim(),
                PasswordHash = hash
            };

            // 4. Save to database
            _db.Customers.Add(c);
            await _db.SaveChangesAsync();

            // 5. Store session so the user is logged in immediately
            HttpContext.Session.SetInt32("CustomerId", c.Id);
            HttpContext.Session.SetString("CustomerName", c.FirstName);

            // 6. Success message and redirect
            TempData["Success"] = "Registration successful. You are now logged in.";
            return RedirectToPage("/Index");
        }

        /// <summary>
        /// Defines the form fields that will be bound to user input.
        /// Includes validation rules using data annotations.
        /// </summary>
        public class InputModel
        {
            [Required, Display(Name = "First Name"), StringLength(50)]
            public string FirstName { get; set; } = string.Empty;

            [Required, Display(Name = "Family Name"), StringLength(50)]
            public string LastName { get; set; } = string.Empty;

            [Required, EmailAddress, StringLength(254)]
            public string Email { get; set; } = string.Empty;

            [Phone, StringLength(30)]
            public string? Phone { get; set; }

            [Display(Name = "Company Name"), StringLength(100)]
            public string? CompanyName { get; set; }

            [StringLength(200)]
            public string? Address { get; set; }

            [Required, DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
            public string Password { get; set; } = string.Empty;

            [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}
