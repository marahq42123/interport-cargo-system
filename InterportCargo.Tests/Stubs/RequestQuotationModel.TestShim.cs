using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InterportCargo.Web.Pages.Customer
{
    // partial to satisfy older tests that still call OnPost()
    public partial class RequestQuotationModel
    {
        public IActionResult OnPost()
        {
            var task = OnPostAsync();
            task.GetAwaiter().GetResult();
            return task.Result;
        }
    }
}
