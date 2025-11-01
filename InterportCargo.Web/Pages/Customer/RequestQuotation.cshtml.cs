using System.ComponentModel.DataAnnotations;
using InterportCargo.Web.Data;
using InterportCargo.Web.Helpers;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterportCargo.Web.Pages.Customer
{
    /// <summary>
    /// PageModel for allowing a logged-in customer to submit a new quotation request.
    /// </summary>
    public class RequestQuotationModel : PageModel
    {
        private readonly InterportContext _db;

        /// <summary>
        /// Injects the database context used to store quotation requests.
        /// </summary>
        public RequestQuotationModel(InterportContext db) => _db = db;

        /// <summary>
        /// Binds form input fields when posting a new quotation request.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = new();

        /// <summary>
        /// Optional status/info message shown to the user after submission.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Holds validated user input used to create a QuotationRequest entity.
        /// </summary>
        public class InputModel
        {
            [Required, StringLength(120)]
            public string Source { get; set; } = string.Empty;

            [Required, StringLength(120)]
            public string Destination { get; set; } = string.Empty;

            [Required, StringLength(12)]
            public string ContainerType { get; set; } = "20GP";

            [Range(1, 999)]
            public int NumberOfContainers { get; set; } = 1;

            [Required, StringLength(160)]
            public string PackageNature { get; set; } = "General";

            [Required, StringLength(300)]
            public string JobNature { get; set; } = "Import";

            [StringLength(500)]
            public string? Notes { get; set; }
        }

        /// <summary>
        /// Handles GET requests. Ensures the user is logged in before displaying the form.
        /// </summary>
        public IActionResult OnGet()
        {
            if (!Auth.IsCustomer(HttpContext))
                return RedirectToPage("/Account/Login");

            return Page();
        }

        /// <summary>
        /// Handles POST requests to submit a quotation request to the database.
        /// Resets the form and displays a confirmation message on success.
        /// </summary>
        public IActionResult OnPost()
        {
            if (!Auth.IsCustomer(HttpContext))
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid)
                return Page();

            var customerId = Auth.GetCustomerId(HttpContext)!.Value;
            var customerName = Auth.GetCustomerName(HttpContext) ?? "Customer";

            var request = new QuotationRequest
            {
                CustomerId = customerId,
                CustomerName = customerName,
                Source = Input.Source.Trim(),
                Destination = Input.Destination.Trim(),
                ContainerType = Input.ContainerType,
                NumberOfContainers = Input.NumberOfContainers,
                PackageNature = Input.PackageNature.Trim(),
                JobNature = Input.JobNature.Trim(),
                Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim(),
                Status = "Pending"
            };

            _db.QuotationRequests.Add(request);
            _db.SaveChanges();

            Message = "Your quotation request has been submitted.";
            ModelState.Clear();
            Input = new InputModel(); // reset form

            return Page();
        }
    }
}
