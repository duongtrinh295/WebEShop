using eShopSolution.Application.Catalog.Products;
using eShopSolution.Data.EF;
using eShopSolution.Utilities.Constants;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với SQL Server
builder.Services.AddDbContext<EShopDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

//Declare DI
builder.Services.AddTransient<IPublicProductService, PublicProductService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
