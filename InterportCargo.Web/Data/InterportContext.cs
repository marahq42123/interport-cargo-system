using Microsoft.EntityFrameworkCore;
using InterportCargo.Web.Models;

namespace InterportCargo.Web.Data
{
    public class InterportContext : DbContext
    {
        public InterportContext(DbContextOptions<InterportContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<QuotationRequest> QuotationRequests { get; set; }
    }
}
