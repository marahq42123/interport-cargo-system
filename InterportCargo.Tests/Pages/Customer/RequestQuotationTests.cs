using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using InterportCargo.Web.Pages.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InterportCargo.Tests.Pages.Customer
{
    public class RequestQuotationTests
    {
        // in-memory EF Core context
        private InterportContext GetDb()
        {
            var options = new DbContextOptionsBuilder<InterportContext>()
                .UseInMemoryDatabase("interport-tests")
                .Options;
            return new InterportContext(options);
        }

        // attach TempData to the page (needed because the page does TempData["Success"] = ...)
        private static void AttachTempData(RequestQuotationModel page, DefaultHttpContext http)
        {
            var provider = new SessionStateTempDataProvider(new TempDataSerializer());
            page.TempData = new TempDataDictionary(http, provider);
        }

        [Fact]
        public async Task Post_redirects_to_login_when_not_logged_in()
        {
            // arrange
            var db = GetDb();
            var page = new RequestQuotationModel(db)
            {
                Input = new RequestQuotationModel.InputModel
                {
                    Source = "A",
                    Destination = "B",
                    PackageNature = "General",
                    JobNature = "Import",
                    NumberOfContainers = 1
                }
            };

            // we still need a session object, just no CustomerId set
            var session = new TestSession();
            var http = new DefaultHttpContext { Session = session };
            page.PageContext = new PageContext { HttpContext = http };
            AttachTempData(page, http);

            // act
            var result = await page.OnPostAsync();

            // assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Account/Login", redirect.PageName);
        }

        [Fact]
        public async Task Post_saves_and_redirects_when_logged_in()
        {
            // arrange
            var db = GetDb();
            var page = new RequestQuotationModel(db)
            {
                Input = new RequestQuotationModel.InputModel
                {
                    Source = "Brisbane",
                    Destination = "Singapore",
                    PackageNature = "General",
                    JobNature = "Export",
                    NumberOfContainers = 2
                }
            };

            // fake logged-in customer
            var session = new TestSession();
            session.SetInt32("CustomerId", 5);

            var http = new DefaultHttpContext { Session = session };
            page.PageContext = new PageContext { HttpContext = http };
            AttachTempData(page, http);

            // act
            var result = await page.OnPostAsync();

            // assert: correct redirect
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Customer/Quotations", redirect.PageName);

            // assert: it actually saved
            var saved = await db.QuotationRequests.FirstOrDefaultAsync(q => q.CustomerId == 5);
            Assert.NotNull(saved);
            Assert.Equal("Brisbane", saved!.Source);
            Assert.Equal("Singapore", saved.Destination);
            Assert.Equal(QuotationStatus.Pending, saved.Status);
        }

        [Fact]
        public async Task Post_returns_page_when_model_invalid()
        {
            var db = GetDb();
            var page = new RequestQuotationModel(db);

            var session = new TestSession();
            session.SetInt32("CustomerId", 1);

            var http = new DefaultHttpContext { Session = session };
            page.PageContext = new PageContext { HttpContext = http };
            AttachTempData(page, http);

            // force invalid
            page.ModelState.AddModelError("Input.Source", "Required");

            var result = await page.OnPostAsync();

            Assert.IsType<PageResult>(result);
        }
    }
}
