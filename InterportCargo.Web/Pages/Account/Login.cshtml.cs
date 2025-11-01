using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InterportCargo.Web.Data;
using InterportCargo.Web.Helpers;

namespace InterportCargo.Web.Pages.Account
{
    /// <summary>
    /// Handles customer login functionality, including validating credentials,
    /// setting session values, and redirecting to the dashboard.
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly InterportContext _db;

        /// <summary>
        /// Initializes the login page model with access to the database context.
        /// </summary>
        /// <param name="db">Entity Framework database context.</param>
        public LoginModel(InterportContext db) => _db = db;

        /// <summary>
        /// Holds user login input values submitted from the form.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = new();

        /// <summary>
        /// Handles GET requests — simply renders the login page.
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// Handles POST requests — validates credentials, authenticates the customer,
        /// sets session variables, and redirects to the proper page.
        /// </summary>
        /// <param name="returnUrl">
        /// Optional return URL to redirect after login (used for protected pages).
        /// </param>
        /// <returns>
        /// Redirects to returnUrl or customer dashboard if successful;
        /// reloads the page with errors otherwise.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            var emailNorm = Input.Email.Trim().ToLowerInvariant();

            // Search for matching customer by email
            var user = await _db.Customers
                .FirstOrDefaultAsync(c => c.Email.ToLower() == emailNorm);

            // Validate password with BCrypt
            if (user == null || !BCrypt.Net.BCrypt.Verify(Input.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            // Set session using Helpers.Auth keys
            HttpContext.Session.SetInt32(Auth.SessionCustomerId, user.Id);
            HttpContext.Session.SetString(Auth.SessionCustomerName, user.FirstName);
            HttpContext.Session.SetString(Auth.SessionUserRole, "Customer");

            TempData["Success"] = "You are now logged in.";

            // Redirect to a requested returnUrl if it's safe
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            // Default redirect to customer dashboard
            return RedirectToPage("/Customer/Dashboard");
        }

        /// <summary>
        /// Nested class that stores email and password
        /// entered in the login form.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Customer email used to log in.
            /// </summary>
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;

            /// <summary>
            /// Customer password in plain text (validated with hashed password).
            /// </summary>
            [Required, DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }
    }
}
