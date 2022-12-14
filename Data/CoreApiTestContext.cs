using CoreApiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreApiTest.Data
{
    public class CoreApiTestContext : DbContext
    {
        //public DbSet<User> Users => Set<User>();
        //public DbSet<Order> Orders => Set<Order>();
        private readonly IConfiguration _configuration;

        public CoreApiTestContext(DbContextOptions<CoreApiTestContext> options, IConfiguration configuration)
             : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration["ConnectionStrings:default"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders);
            modelBuilder.Entity<Order>()
                 .Property(s => s.CreatedAt)
                 .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
                 .Property(s => s.CreatedAt)
                 .HasDefaultValueSql("GETDATE()");
        }
    }
}