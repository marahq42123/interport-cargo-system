using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Web.Pages.Account
{
    /// <summary>
    /// Handles customer logout functionality by clearing session data.
    /// </summary>
    [ValidateAntiForgeryToken]  // Ensures logout requests are secure (prevents CSRF attacks)
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// Processes POST requests to log the user out, clears session data,
        /// sets a temporary success message, and redirects to the homepage.
        /// </summary>
        /// <returns>Redirects the user to the site's Index page.</returns>
        public IActionResult OnPost()
        {
            // Clears all session data (customer ID, name, role, etc.)
            HttpContext.Session.Clear();

            // One-time message shown after redirect
            TempData["Success"] = "You have been logged out.";

            // Redirect to homepage
            return RedirectToPage("/Index");
        }
    }
}
