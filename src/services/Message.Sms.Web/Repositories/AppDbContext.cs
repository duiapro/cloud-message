using Message.Sms.Web.Repositories.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Message.Sms.Web.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UsersUseMobileHistory> UsersUseMobileHistorys { get; set; }
        public DbSet<UsersSmsCodeLogs> UsersSmsCodeLogs { get; set; }
        public DbSet<ApiServiceProvider> ApiServiceProviders { get; set; }
        public DbSet<RechargeCard> RechargeCards { get; set; }
        public DbSet<UsersRechargeLogs> UsersRechargeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .HasOne(p => p.ApiServiceProvider)
                .WithMany(p => p.Channels)
                .HasForeignKey(p => p.ApiServiceProviderId);

            modelBuilder.Entity<UsersSmsCodeLogs>()
                .HasOne(p => p.Users)
                .WithMany(p => p.CodeLogs)
                .HasForeignKey(p => p.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }

}
