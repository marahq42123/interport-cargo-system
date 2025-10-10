using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.Web.Data;
using InterportCargo.Web.Services;

namespace InterportCargo.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly InterportContext _db;
        public LoginModel(InterportContext db) => _db = db;

        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public void OnGet() {}

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return Page();
            }

            var user = _db.Customers.FirstOrDefault(c => c.Email == Email);
            if (user == null || !AuthService.Verify(Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return Page();
            }

            HttpContext.Session.SetInt32("CustomerId", user.Id);
            return RedirectToPage("/Customer/RequestQuotation");
        }
    }
}
