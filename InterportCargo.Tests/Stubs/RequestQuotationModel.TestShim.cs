using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InterportCargo.Web.Pages.Customer
{
    // test only partial class to satisfy older tests that still call OnPost()
    public partial class RequestQuotationModel
    {
        public IActionResult OnPost()
        {
            // forward to the real async handler that the page actually uses
            Task<IActionResult> task = OnPostAsync();
            task.GetAwaiter().GetResult();
            return task.Result;
        }
    }
}
