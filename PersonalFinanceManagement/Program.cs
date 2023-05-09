using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:PersonalFinanceConnection"]);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<ISpendingRepository, SpendingRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();



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

app.UseAuthorization();


  
app.UseEndpoints(endpoints =>
{
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
        defaults: new { controller = "Category", action = "Create" });

    endpoints.MapControllerRoute(
        name: "category-index",
        pattern: "Category",
        defaults: new { controller = "Category", action = "Index" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();