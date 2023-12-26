using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSmsApiClient();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(5, 7, 0)),
        option =>
        {
            option.EnableRetryOnFailure();
        })
    .EnableDetailedErrors();
});
builder.Services.AddHostedService<TokenCacheHostedService>();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AppUsers>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    AppDbSeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
//app.MapAreaControllerRoute("user_route", "User", "{controller=Home}/{action=Index}/{keyId?}");
//app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{keyId?}"/*, defaults: new { area = "User" }*/);
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{keyId?}", "/client");
app.Run();
