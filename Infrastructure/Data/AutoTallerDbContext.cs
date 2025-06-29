using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Entities;
using Infrastructure.Interceptors;

namespace Infrastructure.Data
{
    public class AutoTallerDbContext : DbContext
    {
        public readonly AuditInterceptor _auditInterceptor;
        public AutoTallerDbContext(DbContextOptions<AutoTallerDbContext> options, AuditInterceptor auditInterceptor)
            : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }
        public DbSet<Specialization> Specialization { get; set; }
        public DbSet<ServiceType> ServiceType { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserSpecialization> UserSpecialization { get; set; }
        public DbSet<Diagnostic> Diagnostic { get; set; }
        public DbSet<Auditory> Auditory { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DetailsDiagnostic> DetailsDiagnostic { get; set; }
        public DbSet<InventoryDetail> InventoryDetail { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Replacement> Replacement { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<ServiceOrder> ServiceOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }
    }
}