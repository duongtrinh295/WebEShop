using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
         .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

//Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
//var mvcBuilder = builder.Services.AddRazorPages();

//if (builder.Environment.IsDevelopment())
//{
//    mvcBuilder.AddRazorRuntimeCompilation();
//}
builder.Services.AddHttpClient();

builder.Services.AddTransient<IUserApiClient, UserApiClient>();

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
