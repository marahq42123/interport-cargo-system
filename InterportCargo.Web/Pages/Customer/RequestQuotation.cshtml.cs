// Pages/Customer/RequestQuotation.cshtml.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Web.Pages.Customer
{
    /// <summary>
    /// Handles the creation and submission of a new quotation request by a logged-in customer.
    /// </summary>
    public class RequestQuotationModel : PageModel
    {
        private readonly InterportContext _context;

        /// <summary>Injects the database context.</summary>
        public RequestQuotationModel(InterportContext context) => _context = context;

        /// <summary>Form input model bound to the request form fields.</summary>
        [BindProperty]
        public InputModel Input { get; set; } = new();

        /// <summary>Optional message displayed to the user after submission.</summary>
        public string? Message { get; set; }

        /// <summary>Form fields submitted by user on quotation request page.</summary>
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

        /// <summary>Displays the quotation request form.</summary>
        public void OnGet() { }

        /// <summary>Sync handler used by tests; delegates to async logic.</summary>
        public IActionResult OnPost() => OnPostAsync().GetAwaiter().GetResult();

        /// <summary>Validates and stores a new quotation request in the database.</summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Require logged-in customer (matches tests’ expectation)
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var role = HttpContext.Session.GetString("UserRole");
            if (customerId is null || !string.Equals(role, "Customer", StringComparison.Ordinal))
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid)
                return Page();

            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            var customerName = customer is null ? "Customer" : $"{customer.FirstName} {customer.LastName}".Trim();

            var quotation = new QuotationRequest
            {
                CustomerId = customerId,
                CustomerName = customerName,
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

            TempData["Success"] = "✅ Quotation request submitted successfully.";
            return RedirectToPage("/Customer/RequestQuotation");
        }
    }
}
