using FeestBeest.Data;
using FeestBeest.Data.Models;
using FeestBeest.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FeestBeestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<FeestBeestContext>()
    .AddDefaultTokenProviders();

//Services
builder.Services.AddScoped<AccountService>(); // Scoped because we want to create a new account for each session    
builder.Services.AddScoped<ProductService>(); // Scoped because we want to create a new product for each session    
builder.Services.AddScoped<OrderService>(); // Scoped because we want to create a new order for each session    
builder.Services.AddSingleton<BasketService>(); // Singleton because we want to keep the basket throughout the session  

// Add MVC services
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

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

app.Run();