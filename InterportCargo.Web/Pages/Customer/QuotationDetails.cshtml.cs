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
    // displays details of a customer's quotation request and allows accept/reject actions.
    public class QuotationDetailsModel : PageModel
    {
        private readonly InterportContext _context;

        //nstarts the page model with database context
        public QuotationDetailsModel(InterportContext context) => _context = context;
        
		// holds the quotation details for the current customer.
        [BindProperty]
        public QuotationRequest Quotation { get; set; }

        // Loads the quotation details based on ID only if the user is logged in.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToPage("/Account/Login");

            Quotation = await _context.QuotationRequests
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == customerId.Value);

            if (Quotation == null) return NotFound();
            return Page();
        }

        //  process quotation approval or rejection from customer side
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
