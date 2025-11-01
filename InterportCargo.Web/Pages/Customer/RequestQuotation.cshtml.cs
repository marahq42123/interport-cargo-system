using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Web.Pages.Customer
{
    public class RequestQuotationModel : PageModel
    {
        private readonly InterportContext _context;
        public RequestQuotationModel(InterportContext context) => _context = context;

        [BindProperty]
        public QuotationRequest Quotation { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        public void OnGet() {}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please correct the errors in the form.";
                return Page();
            }

            // Save the quotation request to the database
            Quotation.Status = QuotationStatus.Pending;
            _context.QuotationRequests.Add(Quotation);
            await _context.SaveChangesAsync();

            Message = "Quotation request submitted successfully!";
            return RedirectToPage("/Customer/Quotations");
        }
    }
}
