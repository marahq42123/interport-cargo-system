using System.Linq;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Customer
{
    /// <summary>
    /// Displays details of a customer's quotation request and allows accept/reject actions.
    /// </summary>
    public class QuotationDetailsModel : PageModel
    {
        private readonly InterportContext _context;

        /// <summary>
        /// Initializes the page model with database context.
        /// </summary>
        public QuotationDetailsModel(InterportContext context) => _context = context;

        /// <summary>
        /// Holds the quotation details for the current customer.
        /// </summary>
        [BindProperty]
        public QuotationRequest Quotation { get; set; }

        /// <summary>
        /// Loads the quotation details based on ID only if the user is logged in.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToPage("/Account/Login");

            Quotation = await _context.QuotationRequests
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == customerId.Value);

            if (Quotation == null) return NotFound();
            return Page();
        }

        /// <summary>
        /// Processes quotation approval or rejection from customer side.
        /// </summary>
        public async Task<IActionResult> OnPostAsync(int id, string action)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToPage("/Account/Login");

            var request = await _context.QuotationRequests
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == customerId.Value);

            if (request == null) return NotFound();

            request.Status = action == "Accept" ? QuotationStatus.Accepted : QuotationStatus.Rejected;
            await _context.SaveChangesAsync();
            return RedirectToPage("/Customer/Quotations");
        }
    }
}
