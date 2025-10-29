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
        private readonly InterportContext _db;
        public DetailsModel(InterportContext db) { _db = db; }

        public QuotationRequest Item { get; set; } = default!;

        [BindProperty] public int Id { get; set; }
        [BindProperty] public string? OfficerMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var req = await _db.QuotationRequests
                               .Include(r => r.Customer)
                               .FirstOrDefaultAsync(r => r.Id == id);
            if (req == null) return NotFound();

            Item = req;
            Id = id;
            OfficerMessage = req.OfficerMessage;
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync()
        {
            var req = await _db.QuotationRequests.FirstOrDefaultAsync(r => r.Id == Id);
            if (req == null) return NotFound();

            if (req.Status == QuotationStatus.Pending)
            {
                req.Status = QuotationStatus.Accepted;
                req.OfficerMessage = OfficerMessage;
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("/Officer/Requests/Details", new { id = Id });
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            var req = await _db.QuotationRequests.FirstOrDefaultAsync(r => r.Id == Id);
            if (req == null) return NotFound();

            if (req.Status == QuotationStatus.Pending)
            {
                req.Status = QuotationStatus.Rejected;
                req.OfficerMessage = OfficerMessage;
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("/Officer/Requests/Details", new { id = Id });
        }
    }
}
