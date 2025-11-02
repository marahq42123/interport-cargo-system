using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.Web.Data;
using InterportCargo.Web.Helpers;
using InterportCargo.Web.Models;
using System.Linq;
using System.Collections.Generic;

namespace InterportCargo.Web.Pages.Customer
{
    // page model for listing all quotation requests submitted by the logged-in customer
    public class QuotationsModel : PageModel
    {
        private readonly InterportContext _db;

        public QuotationsModel(InterportContext db)
        {
            _db = db;
        }

        // list of quotation requests belonging to the current customer
        public List<QuotationRequest> Items { get; set; } = new();

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
