using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.Web.Data;
using InterportCargo.Web.Helpers;
using InterportCargo.Web.Models;
using System.Linq;
using System.Collections.Generic;

namespace InterportCargo.Web.Pages.Customer
{
    /// <summary>
    /// PageModel for listing all quotation requests submitted by the logged-in customer.
    /// </summary>
    public class QuotationsModel : PageModel
    {
        private readonly InterportContext _db;

        /// <summary>
        /// Injects the database context.
        /// </summary>
        /// <param name="db">The application's database context.</param>
        public QuotationsModel(InterportContext db)
        {
            _db = db;
        }

        /// <summary>
        /// A list of quotation requests belonging to the current customer.
        /// </summary>
        public List<QuotationRequest> Items { get; set; } = new();

        /// <summary>
        /// Handles GET requests to load the customer's quotation history.
        /// Redirects to login if no active customer session is found.
        /// </summary>
        /// <returns>The page displaying the quotations or a redirect to login.</returns>
        public IActionResult OnGet()
        {
            if (!Auth.IsCustomer(HttpContext))
                return RedirectToPage("/Account/Login");

            var customerId = Auth.GetCustomerId(HttpContext);

            Items = _db.QuotationRequests
                       .Where(q => q.CustomerId == customerId)
                       .OrderByDescending(q => q.CreatedUtc)
                       .ToList();

            return Page();
        }
    }
}
