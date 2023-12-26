using Message.Sms.Web.Repositories.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Message.Sms.Web.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UsersUseMobileHistory> UsersUseMobileHistorys { get; set; }
        public DbSet<UsersSmsCodeLogs> UsersSmsCodeLogs { get; set; }
        public DbSet<ApiServiceProvider> ApiServiceProviders { get; set; }
        public DbSet<RechargeCard> RechargeCards { get; set; }
        public DbSet<UsersBalanceBill> UsersBalanceBills { get; set; }

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

            modelBuilder.Entity<UsersBalanceBill>()
                .HasOne(p => p.Users)
                .WithMany(p => p.BalanceBill)
                .HasForeignKey(p => p.UserKeyId);

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var configurationBuilder = new ConfigurationBuilder();
        //    var configuration = configurationBuilder
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //    optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
        //        new MySqlServerVersion(new Version(5, 7, 0)), option => option.EnableRetryOnFailure()).EnableDetailedErrors();

        //}
    }

    public class DbContextFactory
    {
        public static AppDbContext Create(params string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(5, 7, 0)), option => option.EnableRetryOnFailure()).EnableDetailedErrors();

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
