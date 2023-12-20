using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Salon.Models;

namespace Salon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Doljnost> Doljnosts { get; set; }

        public DbSet<Visit_CountOtchet> Visit_CountOtchet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().ToTable(tb => tb.HasTrigger("Check_Group"));
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Service>().ToTable(tb => tb.HasTrigger("AddDelCount"));
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Service>().ToTable(tb => tb.HasTrigger("UpdCount"));
            base.OnModelCreating(modelBuilder);
        }
    }
}