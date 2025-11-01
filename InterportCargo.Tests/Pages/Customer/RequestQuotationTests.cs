using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Pages.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CustomerModel = InterportCargo.Web.Models.Customer; // Prevent namespace conflict
using Xunit;

namespace InterportCargo.Tests.Pages.Customer
{
    /// <summary>
    /// Unit tests for User Story I2 – submitting a quotation request.
    /// Validates authentication, valid/invalid form submission, and database persistence.
    /// </summary>
    public class RequestQuotationTests
    {
        /// <summary>
        /// Creates a fresh in-memory database per test.
        /// </summary>
        private InterportContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<InterportContext>()
                .UseInMemoryDatabase("TestDB_" + System.Guid.NewGuid())
                .Options;
            return new InterportContext(options);
        }

        /// <summary>
        /// Creates the Razor PageModel with fake session + HttpContext.
        /// If isLoggedIn = true → adds fake CustomerId + "Customer" role.
        /// </summary>
        private RequestQuotationModel CreatePageModel(InterportContext db, bool isLoggedIn = true)
        {
            var httpContext = new DefaultHttpContext
            {
                Session = new DummySession()
            };

            if (isLoggedIn)
            {
                httpContext.Session.SetInt32("CustomerId", 1);
                httpContext.Session.SetString("UserRole", "Customer");
            }

            return new RequestQuotationModel(db)
            {
                PageContext = new PageContext { HttpContext = httpContext }
            };
        }

        // Test 1 — Users not logged in should be redirected to login page.
        [Fact]
        public void OnPost_NotLoggedIn_RedirectsToLogin()
        {
            var db = GetDbContext();
            var page = CreatePageModel(db, false);

            var result = page.OnPost() as RedirectToPageResult;

            Assert.NotNull(result);
            Assert.Equal("/Account/Login", result!.PageName);
        }

        // Test 2 — Valid data saves to DB and stays on Page() (not redirect).
        [Fact]
        public async Task OnPost_ValidData_SavesQuotationRequest()
        {
            var db = GetDbContext();
            db.Customers.Add(new CustomerModel { Id = 1, FirstName = "Test", LastName = "User", Email = "t@t.com", PasswordHash = "x" });
            await db.SaveChangesAsync();

            var page = CreatePageModel(db);
            page.Input.Source = "Sydney";
            page.Input.Destination = "Melbourne";
            page.Input.NumberOfContainers = 2;
            page.Input.ContainerType = "20GP";
            page.Input.PackageNature = "General";
            page.Input.JobNature = "Import";

            var result = page.OnPost();

            Assert.IsType<PageResult>(result);
            Assert.Single(db.QuotationRequests);
        }

        // Test 3 — Invalid model should NOT save anything.
        [Fact]
        public void OnPost_InvalidModel_ReturnsPageAndDoesNotSave()
        {
            var db = GetDbContext();
            var page = CreatePageModel(db);

            page.Input.Source = ""; // triggers validation error
            page.ModelState.AddModelError("Input.Source", "Required");

            var result = page.OnPost();

            Assert.IsType<PageResult>(result);
            Assert.Empty(db.QuotationRequests);
        }
    }

    /// <summary>
    /// Fake in-memory session used for testing without ASP.NET Core runtime.
    /// </summary>
    public class DummySession : ISession
    {
        private readonly Dictionary<string, byte[]> storage = new();
        public bool IsAvailable => true;
        public string Id => "test-session";
        public IEnumerable<string> Keys => storage.Keys;

        public void Set(string key, byte[] value) => storage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => storage.TryGetValue(key, out value);
        public void Remove(string key) => storage.Remove(key);
        public void Clear() => storage.Clear();
        public Task CommitAsync(CancellationToken token = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken token = default) => Task.CompletedTask;
    }
}
