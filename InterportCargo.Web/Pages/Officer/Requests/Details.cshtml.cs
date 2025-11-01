using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Officer.Requests
{
    public class DetailsModel : PageModel
    {
        private readonly InterportContext _context;

        public DetailsModel(InterportContext context)
        {
            _context = context;
        }

        // Holds the current quotation being displayed
        [BindProperty]
        public QuotationRequest Quotation { get; set; } = default!;

        // Officer can leave notes or responses
        [BindProperty]
        public string? OfficerMessage { get; set; }

        // Load quotation details with linked customer info
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quotation = await _context.QuotationRequests
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Quotation == null)
                return NotFound();

            return Page();
        }

        // Officer submits approval or rejection
        public async Task<IActionResult> OnPostAsync(int id, string action)
        {
            var request = await _context.QuotationRequests
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (request == null)
                return NotFound();

            // Use ENUM instead of string
            if (action == "Approve")
                request.Status = QuotationStatus.Accepted;
            else if (action == "Reject")
                request.Status = QuotationStatus.Rejected;
            else
                request.Status = QuotationStatus.Pending; // fallback safety

            // Save officer response
            request.OfficerMessage = OfficerMessage;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Officer/Requests/Index");
        }
    }
}
