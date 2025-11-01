using InterportCargo.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Web.Pages.Customer
{
    /// <summary>
    /// Razor PageModel for the Customer Dashboard.
    /// Displays a greeting and provides navigation to quotation features.
    /// </summary>
    public class DashboardModel : PageModel
    {
        /// <summary>
        /// The first name of the currently logged-in customer, retrieved from session.
        /// Defaults to "Customer" if session value is missing.
        /// </summary>
        public string CustomerName { get; set; } = "Customer";

        /// <summary>
        /// Handles GET requests to the Customer Dashboard.
        /// Verifies that a customer is logged in, then loads their name from session.
        /// If the user is not authenticated, redirects to the login page.
        /// </summary>
        /// <returns>
        /// <see cref="IActionResult"/> that renders the page if authenticated,
        /// otherwise redirects to the Login page.
        /// </returns>
        public IActionResult OnGet()
        {
            // Ensure only authenticated customers can access this page.
            if (!Auth.IsCustomer(HttpContext))
                return RedirectToPage("/Account/Login");

            // Load the customer's name from session; fallback to "Customer" if not found.
            CustomerName = Auth.GetCustomerName(HttpContext) ?? "Customer";

            return Page();
        }
    }
}
