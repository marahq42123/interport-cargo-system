#nullable enable
using Microsoft.EntityFrameworkCore;
using InterportCargo.Web.Models;

namespace InterportCargo.Web.Data
{
    /// <summary>
    /// The main Entity Framework Core database context for the Interport Cargo system.
    /// </summary>
    /// <remarks>
    /// This class manages database access, entity configurations, and seed data.
    /// It maps C# model classes (Customer, Employee, QuotationRequest, etc.) to database tables.
    /// </remarks>
    public class InterportContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterportContext"/> class using the specified options.
        /// </summary>
        /// <param name="options">Configuration options such as database provider and connection string.</param>
        public InterportContext(DbContextOptions<InterportContext> options) : base(options) { }

        // ---------------------- DbSets ----------------------

        /// <summary>
        /// Represents the <c>Customers</c> table in the database.
        /// </summary>
        public DbSet<Customer> Customers => Set<Customer>();

        /// <summary>
        /// Represents the <c>Employees</c> table in the database.
        /// </summary>
        public DbSet<Employee> Employees => Set<Employee>();

        /// <summary>
        /// Stores quotation requests submitted by customers.
        /// </summary>
        public DbSet<QuotationRequest> QuotationRequests => Set<QuotationRequest>();

        /// <summary>
        /// Stores baseline rate schedules for different container types.
        /// </summary>
        public DbSet<RateSchedule> RateSchedules => Set<RateSchedule>();

        /// <summary>
        /// Stores quotations prepared by Quotation Officers.
        /// </summary>
        public DbSet<Quotation> Quotations => Set<Quotation>();

        // ---------------------- Model Configuration ----------------------

        /// <summary>
        /// Configures model relationships, keys, database constraints and seeds initial rate data.
        /// </summary>
        /// <param name="b">ModelBuilder used to configure entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // --- Customer configuration ---
            b.Entity<Customer>(e =>
            {
                // Unique email to prevent duplicate accounts
                e.HasIndex(c => c.Email).IsUnique();

                // Enforce same rules at DB level as in Data Annotations
                e.Property(c => c.FirstName).HasMaxLength(50).IsRequired();
                e.Property(c => c.LastName).HasMaxLength(50).IsRequired();
                e.Property(c => c.Email).HasMaxLength(254).IsRequired();
                e.Property(c => c.Phone).HasMaxLength(30);
                e.Property(c => c.CompanyName).HasMaxLength(100);
                e.Property(c => c.Address).HasMaxLength(200);
                e.Property(c => c.PasswordHash).IsRequired();
            });

            // --- Index QuotationRequests by customer and status (faster lookups) ---
            b.Entity<QuotationRequest>()
             .HasIndex(q => new { q.CustomerId, q.Status });

            // --- Seed basic rate schedule into database ---
            b.Entity<RateSchedule>().HasData(
                new RateSchedule { Id = 1, ContainerType = "20GP", BaseCharge = 1200, DepotCharge = 180, LclDeliveryCharge = 0   },
                new RateSchedule { Id = 2, ContainerType = "40GP", BaseCharge = 1800, DepotCharge = 240, LclDeliveryCharge = 0   },
                new RateSchedule { Id = 3, ContainerType = "LCL",  BaseCharge =    0, DepotCharge =   0, LclDeliveryCharge = 250 }
            );
        }
    }
}
