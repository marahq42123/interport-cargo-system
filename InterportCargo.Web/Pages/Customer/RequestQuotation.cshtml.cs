using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Web.Pages.Customer
{
    // handles the creation and submission of a new quotation request by a customer
    public class RequestQuotationModel : PageModel
    {
        private readonly InterportContext _context;

        public RequestQuotationModel(InterportContext context) => _context = context;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? Message { get; set; }

        public class InputModel
        {
            [Required] public string Source { get; set; } = string.Empty;
            [Required] public string Destination { get; set; } = string.Empty;
            public string ContainerType { get; set; } = "20GP";
            [Range(1, 999)] public int NumberOfContainers { get; set; } = 1;
            [Required] public string PackageNature { get; set; } = "General";
            [Required] public string JobNature { get; set; } = "Import";
            public string? Notes { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // must be logged in
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId is null)
            {
                return RedirectToPage("/Account/Login");
            }

            // validate form
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // create entity from input
            var quotation = new QuotationRequest
            {
                CustomerId = customerId.Value,              // <-- make it non-null
                CustomerName = null,                        // you already have CustomerId
                Source = Input.Source,
                Destination = Input.Destination,
                ContainerType = Input.ContainerType,
                NumberOfContainers = Input.NumberOfContainers,
                PackageNature = Input.PackageNature,
                JobNature = Input.JobNature,
                Notes = Input.Notes,
                Status = QuotationStatus.Pending,
                CreatedUtc = DateTime.UtcNow
            };

            _context.QuotationRequests.Add(quotation);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Quotation request submitted successfully.";

            // go to list page
            return RedirectToPage("/Customer/Quotations");
        }
    }
}
