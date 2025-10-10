using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;

namespace InterportCargo.Web.Pages.Customer
{
    public class RequestQuotationModel : PageModel
    {
        private readonly InterportContext _db;
        public RequestQuotationModel(InterportContext db) => _db = db;

        [BindProperty] public string Source { get; set; } = "";
        [BindProperty] public string Destination { get; set; } = "";
        [BindProperty] public int NumberOfContainers { get; set; } = 1;
        [BindProperty] public string PackageNature { get; set; } = "";
        [BindProperty] public string JobNature { get; set; } = "";

        public IActionResult OnGet()
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            if (cid is null) return RedirectToPage("/Account/Login");
            return Page();
        }

        public IActionResult OnPost()
        {
            var cid = HttpContext.Session.GetInt32("CustomerId");
            if (cid is null) return RedirectToPage("/Account/Login");

            if (string.IsNullOrWhiteSpace(Source) ||
                string.IsNullOrWhiteSpace(Destination) ||
                NumberOfContainers <= 0 ||
                string.IsNullOrWhiteSpace(PackageNature) ||
                string.IsNullOrWhiteSpace(JobNature))
            {
                ModelState.AddModelError(string.Empty, "All fields are required and containers must be > 0.");
                return Page();
            }

            var req = new QuotationRequest
            {
                CustomerId = cid.Value,
                Source = Source,
                Destination = Destination,
                NumberOfContainers = NumberOfContainers,
                PackageNature = PackageNature,
                JobNature = JobNature,
                Status = QuotationStatus.Pending
            };

            _db.QuotationRequests.Add(req);
            _db.SaveChanges();

            TempData["Msg"] = $"Quotation request submitted. Request ID: {req.Id}";
            return RedirectToPage("/Customer/RequestQuotation");
        }
    }
}
