using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterportCargo.Web.Data;
using InterportCargo.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.Web.Pages.Officer.Requests
{
    public class IndexModel : PageModel
    {
        private readonly InterportContext _db;
        public IndexModel(InterportContext db) { _db = db; }

        public IList<QuotationRequest> Items { get; set; } = new List<QuotationRequest>();
        public QuotationStatus? StatusFilter { get; set; }

        public async Task OnGetAsync(QuotationStatus? status)
        {
            StatusFilter = status;

            var q = _db.QuotationRequests
                       .Include(r => r.Customer) 
                       .AsQueryable();

            if (status.HasValue)
                q = q.Where(r => r.Status == status.Value);

            Items = await q.OrderByDescending(r => r.Id).ToListAsync();
        }
    }
}
