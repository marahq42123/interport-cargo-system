using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.Web.Data;
using InterportCargo.Web.Helpers;
using InterportCargo.Web.Models;
using System.Linq;

namespace InterportCargo.Web.Pages.Customer
{
    /// <summary>
    /// PageModel that displays details of a specific quotation request for a logged-in customer.
    /// Also allows the customer to accept or reject an issued quotation.
    /// </summary>
    public class QuotationDetailsModel : PageModel
    {
        private readonly InterportContext _db;

        /// <summary>
        /// Constructor to inject the database context.
        /// </summary>
        /// <param name="db">The application's database context.</param>
        public QuotationDetailsModel(InterportContext db)
        {
            _db = db;
        }

        /// <summary>
        /// The quotation request being displayed.
        /// </summary>
        [BindProperty]
        public QuotationRequest Quotation { get; set; }

        /// <summary>
        /// Handles GET requests to display a quotation's details.
        /// Ensures the user is logged in and owns the quotation.
        /// </summary>
        /// <param name="id">The ID of the quotation request.</param>
        /// <returns>
        /// Renders the page if valid; otherwise, redirects to login or returns NotFound.
        /// </returns>
        public IActionResult OnGet(int id)
        {
            if (!Auth.IsCustomer(HttpContext))
                return RedirectToPage("/Account/Login");

            var customerId = Auth.GetCustomerId(HttpContext);

            Quotation = _db.QuotationRequests
                           .FirstOrDefault(q => q.Id == id && q.CustomerId == customerId);

            if (Quotation == null)
                return NotFound();

            return Page();
        }

        /// <summary>
        /// Handles POST requests to update the quotation status (Accept/Reject).
        /// </summary>
        /// <param name="action">The action the customer takes ("Accept" or "Reject").</param>
        /// <param name="id">The ID of the quotation request.</param>
        /// <returns>
        /// Redirects to the quotations list after updating status, or returns NotFound if invalid.
        /// </returns>
        public IActionResult OnPost(string action, int id)
        {
            var customerId = Auth.GetCustomerId(HttpContext);

            var quotation = _db.QuotationRequests
                               .FirstOrDefault(q => q.Id == id && q.CustomerId == customerId);

            if (quotation == null)
                return NotFound();

            quotation.Status = action == "Accept" ? "Accepted" : "Rejected";
            _db.SaveChanges();

            return RedirectToPage("/Customer/Quotations");
        }
    }
}
