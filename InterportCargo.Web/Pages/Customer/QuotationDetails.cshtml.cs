using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Customer
{
    public class QuotationDetailsModel : PageModel
    {
        private readonly InterportContext _db;
        public QuotationDetailsModel(InterportContext db) => _db = db;

        public QuotationRequest? Request { get; set; }
        public Quotation? Quotation { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            if (cid is null) return RedirectToPage("/Account/Login");

            Request = await _db.QuotationRequests
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == cid.Value);

            if (Request == null) return NotFound();

            // load actual quotation if officer created one
            Quotation = await _db.Quotations
                .FirstOrDefaultAsync(q => q.QuotationRequestId == id);

            return Page();
        }
    }
}
