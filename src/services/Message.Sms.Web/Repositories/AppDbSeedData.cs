using Message.Sms.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Repositories
{
    public class AppDbSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
           serviceProvider.GetRequiredService<
               DbContextOptions<AppDbContext>>()))
            {
                // Look for any movies.
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }
                context.Users.Add(new Entity.Users
                {
                    KeyId = Guid.Parse("b3d3b3a0-0f1a-4b9e-8b9a-0b9b6b9b6b9b"),
                    UserName = "admin",
                    Balance = 100,
                    Discount = 1,
                    IsAdmin = true,
                    IsVip = true,
                    PassWork = "123456",
                    UserMobile = "13800138000"
                });
                context.SaveChanges();
            }
        }
    }
}
