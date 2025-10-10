using Microsoft.EntityFrameworkCore;
using InterportCargo.Web.Models;

namespace InterportCargo.Web.Data
{
    public class InterportContext : DbContext
    {
        public InterportContext(DbContextOptions<InterportContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<QuotationRequest> QuotationRequests => Set<QuotationRequest>();
        public DbSet<RateSchedule> RateSchedules => Set<RateSchedule>();
        public DbSet<Quotation> Quotations => Set<Quotation>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<RateSchedule>().HasData(
                new RateSchedule { Id = 1, ContainerType = "20GP", BaseCharge = 1200, DepotCharge = 180, LclDeliveryCharge = 0 },
                new RateSchedule { Id = 2, ContainerType = "40GP", BaseCharge = 1800, DepotCharge = 240, LclDeliveryCharge = 0 },
                new RateSchedule { Id = 3, ContainerType = "LCL",  BaseCharge = 0,    DepotCharge = 0,   LclDeliveryCharge = 250 }
            );
        }
    }
}
