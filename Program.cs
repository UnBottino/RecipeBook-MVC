using Microsoft.EntityFrameworkCore;
using RecipeBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddDbContext<RecipeBookContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services.AddDefaultIdentity<RecipeBookUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<RecipeBookContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recipe}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
