using FeestBeest.Data;
using FeestBeest.Data.Models;
using FeestBeest.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FeestBeestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<FeestBeestContext>()
    .AddDefaultTokenProviders();

// Register application services
builder.Services.AddScoped< AccountService>();
builder.Services.AddScoped< ProductService>(); 
builder.Services.AddScoped< OrderService>(); 

// Add MVC services
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "boeking",
    pattern: "Boeking/{action=Index}/{id?}",
    defaults: new { controller = "Boeking" });

app.Run();