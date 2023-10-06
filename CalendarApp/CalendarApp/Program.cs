using CalendarApp.Data;
using CalendarApp.Helpers;
using CalendarApp.Hub;
using CalendarApp.Models;
using CalendarApp.Service.Abtract;
using CalendarApp.Service.Implements;
using Hangfire;
using Hangfire.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Dbcontext config
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString)
	.UseLazyLoadingProxies());

builder.Services.AddDbContext<CalenderAppContext>(options =>
	options.UseSqlServer(connectionString));
// identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


//Hangfire Services
builder.Services.AddHangfire(options =>
{
	options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// SignalR
builder.Services.AddSignalR()
  .AddJsonProtocol(options =>
   {
	   options.PayloadSerializerOptions.WriteIndented = false;
   });

//DI
builder.Services.AddScoped<IDAL, DAL>();
builder.Services.AddScoped<IJobService, JobService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHangfireDashboard();
app.UseHangfireServer();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// create maphub
app.MapHub<NotifyHub>("notify-hub");

app.Run();
