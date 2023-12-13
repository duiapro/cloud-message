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
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UsersUseMobileHistory> UsersUseMobileHistorys { get; set; }
        public DbSet<UsersSmsCodeLogs> UsersSmsCodeLogs { get; set; }
        public DbSet<ApiAuthority> ApiAuthoritys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
