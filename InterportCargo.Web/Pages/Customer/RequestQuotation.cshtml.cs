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
    /// <summary>
    /// Handles the creation and submission of a new quotation request by a customer or guest user.
    /// </summary>
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

        public void OnGet() { }

        // Keep a sync entry point for tests
        public IActionResult OnPost()
        {
            var t = OnPostAsync();
            t.GetAwaiter().GetResult();
            return t.Result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1) Auth check – tests expect redirect to /Account/Login when not logged in
            bool loggedIn = false;
            int? customerId = null;
            try
            {
                if (HttpContext?.Session != null)
                {
                    var val = HttpContext.Session.GetInt32("CustomerId");
                    if (val.HasValue)
                    {
                        loggedIn = true;
                        customerId = val.Value;
                    }
                }
            }
            catch
            {
                // If session middleware is absent in tests, treat as not logged in
                loggedIn = false;
            }

            if (!loggedIn)
            {
                return RedirectToPage("/Account/Login");
            }

            // 2) Model validation
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 3) Save request (null-safe)
            var quotation = new QuotationRequest
            {
                CustomerId = customerId,
                CustomerName = customerId.HasValue ? null : "Guest User",
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

            // 4) Tests expect PageResult on success (not a redirect)
            // Guard TempData to avoid NRE in test envs
            if (TempData != null)
            {
                TempData["Success"] = "✅ Quotation request submitted successfully.";
            }

            // Optionally refresh the form with a cleared model so Page() renders clean
            Input = new InputModel();
            return Page();
        }
    }
}
