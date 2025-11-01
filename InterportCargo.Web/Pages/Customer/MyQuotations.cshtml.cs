using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Customer
{
    public class MyQuotationsModel : PageModel
    {
        private readonly InterportContext _db;
        public MyQuotationsModel(InterportContext db) => _db = db;

        public IList<QuotationRequest> Requests { get; set; } = new List<QuotationRequest>();

        public async Task<IActionResult> OnGetAsync()
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            if (cid is null) return RedirectToPage("/Account/Login");

            Requests = await _db.QuotationRequests
                .Where(q => q.CustomerId == cid.Value)
                .OrderByDescending(q => q.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            if (cid is null) return RedirectToPage("/Account/Login");

            var req = await _db.QuotationRequests
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == cid.Value);

            if (req == null) return NotFound();

            if (req.Status == QuotationStatus.Pending)
            {
                req.Status = QuotationStatus.Rejected; // or a dedicated "Cancelled" value
                await _db.SaveChangesAsync();
                TempData["Msg"] = "Request cancelled.";
            }
            else
            {
                TempData["Msg"] = "This request can no longer be cancelled.";
            }

            return RedirectToPage();
        }
    }
}
