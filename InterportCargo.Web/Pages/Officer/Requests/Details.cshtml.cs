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
        public DetailsModel(InterportContext context) => _context = context;

        [BindProperty]
        public QuotationRequest Quotation { get; set; } = default!;
        
        [BindProperty]
        public string? OfficerMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quotation = await _context.QuotationRequests
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Quotation == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string action)
        {
            var request = await _context.QuotationRequests.FindAsync(id);
            if (request == null) return NotFound();

            if (action == "Approve")
                request.Status = QuotationStatus.Accepted;
            else if (action == "Reject")
                request.Status = QuotationStatus.Rejected;

            request.OfficerMessage = OfficerMessage;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Officer/Requests/Index");
        }
    }
}
