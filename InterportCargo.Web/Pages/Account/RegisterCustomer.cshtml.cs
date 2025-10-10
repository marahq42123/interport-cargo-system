using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using InterportCargo.Web.Services;

namespace InterportCargo.Web.Pages.Account
{
    public class RegisterCustomerModel : PageModel
    {
        private readonly InterportContext _db;
        public RegisterCustomerModel(InterportContext db) => _db = db;

        [BindProperty] public string FirstName { get; set; } = "";
        [BindProperty] public string FamilyName { get; set; } = "";
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Phone { get; set; } = "";
        [BindProperty] public string? CompanyName { get; set; }
        [BindProperty] public string Address { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public void OnGet() {}

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(FamilyName) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return Page();
            }

            var customer = new InterportCargo.Web.Models.Customer {
                FirstName = FirstName,
                FamilyName = FamilyName,
                Email = Email,
                Phone = Phone,
                CompanyName = CompanyName,
                Address = Address,
                PasswordHash = AuthService.Hash(Password)
            };

            _db.Customers.Add(customer);
            _db.SaveChanges();
            TempData["Msg"] = "Registration successful. Please log in.";
            return RedirectToPage("/Account/Login");
        }
    }
}
