using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Filters;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);
//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1mQ1hbdF9AXnNIfll0RGlbek4BCV5EYF5SRHNeQlxjS3ZQcUBlUXg=;Mgo+DSMBPh8sVXJ1S0R+X1pCaVZdX2NLfUN3RWlbflRycUU3HVdTRHRcQlhiSX9TdE1gXnpXcnM=;ORg4AjUWIQA/Gnt2VFhiQlJPcEBLQmFJfFBmRGNTfV96dlBWESFaRnZdQV1mSH5SdkdrWnhYcH1S;MTk5NzM1NkAzMjMxMmUzMjJlMzNQU3RmdWdPd1NSa3JUcGQrK3hMTzJMRE9xZzR5ZlJwR0tvZ000bWtycXZvPQ==;MTk5NzM1N0AzMjMxMmUzMjJlMzNZK2hvVVV1OHFJa2RucENja20xOEsrT1RmSlBaMnVqSkxUYk8zR28rbm13PQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfVldVHxLflF1VWJZdVxxfldDcC0sT3RfQF5jTH9Td0VhUXxZdn1VQQ==;MTk5NzM1OUAzMjMxMmUzMjJlMzNRdjVCNjVRWjVWMXhnUGlndlNON2VIeDR1WENZNVdQcW1SdnNKa1REK05ZPQ==;MTk5NzM2MEAzMjMxMmUzMjJlMzNZS3hQZnM0RUNLUGtzdHVlVURadTF2UU1IWW1VMGczdzNZMmtWbGlxRXhFPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPcEBLQmFJfFBmRGNTfV96dlBWESFaRnZdQV1mSH5SdkdrWnhZdnFQ;MTk5NzM2MkAzMjMxMmUzMjJlMzNQcW1HUTdMV1FGekF4eUVsekNmNUpNeVhXZ2NwN2d0NFhJM0xqTmFkZUhjPQ==;MTk5NzM2M0AzMjMxMmUzMjJlMzNsOE9oRUhiRU5iRFJkUFNXcGJUaVlQSjkrRzU2NWFyeGNGb0N5Q1dPdWlVPQ==;MTk5NzM2NEAzMjMxMmUzMjJlMzNRdjVCNjVRWjVWMXhnUGlndlNON2VIeDR1WENZNVdQcW1SdnNKa1REK05ZPQ==");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<MyDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:PersonalFinanceConnection"]);
});
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new RequireLoginFilter()); // Add the filter to all controller actions
});


    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Configure the password requirements if necessary
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();
    
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<ISpendingRepository, SpendingRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


builder.Services.AddScoped<SignInManager<User>, SignInManager<User>>();
builder.Services.AddScoped<UserManager<User>, UserManager<User>>();

var app = builder.Build();

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

app.UseAuthentication(); 
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    endpoints.MapControllerRoute(
        name: "transaction-create",
        pattern: "Transaction/Create",
        defaults: new { controller = "Transaction", action = "Create" });

    endpoints.MapControllerRoute(
        name: "transaction-index",
        pattern: "Transaction",
        defaults: new { controller = "Transaction", action = "Index" });

    endpoints.MapControllerRoute(
        name: "category-create",
        pattern: "Category/Create",
        defaults: new { controller = "Category", action = "CreateOrEdit" });

    endpoints.MapControllerRoute(
        name: "category-edit",
        pattern: "Category/Edit/{id?}",
        defaults: new { controller = "Category", action = "CreateOrEdit" });

    endpoints.MapControllerRoute(
        name: "category-index",
        pattern: "Category",
        defaults: new { controller = "Category", action = "Index" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "account",
        pattern: "Account/Login",
        defaults: new { controller = "Account", action = "Login" });
    
    endpoints.MapControllerRoute(
        name: "account",
        pattern: "Account/Register",
        defaults: new { controller = "Account", action = "Register" });
    endpoints.MapControllerRoute(
        name: "account",
        pattern: "Account/Logout",
        defaults: new { controller = "Account", action = "Logout" });
    endpoints.MapControllerRoute(
        name: "account",
        pattern: "Account/Profile",
        defaults: new { controller = "Account", action = "Profile" });
});


app.Run();