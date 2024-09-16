using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AD;
using AD.Data;
using Microsoft.AspNetCore.Authentication.Negotiate;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using AD.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Google.Apis.Admin.Directory.directory_v1;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using NuGet.Packaging;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ADContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ADContext") ?? throw new InvalidOperationException("Connection string 'ADContext' not found.")));


builder.Services.AddDbContext<Identity>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ADContext") ?? throw new InvalidOperationException("Connection string 'Identity' not found.")));



builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 0;      
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddRoles<IdentityRole>()
.AddUserManager<ADUserManager<ApplicationUser>>()
.AddEntityFrameworkStores<Identity>();

builder.Services.AddAuthentication(/*NegotiateDefaults.AuthenticationScheme*/options =>
{
    //options.DefaultScheme = Identity;
})
   .AddNegotiate()  
   .AddGoogle(options =>
   {
   options.ClientId = "";
   options.ClientSecret = "";
   options.CallbackPath = "";                                            
    List<string> scopes = new List<string>
    {
        DirectoryService.Scope.AdminDirectoryUserReadonly,
        DirectoryService.Scope.AdminDirectoryUser,
        DirectoryService.Scope.AdminDirectoryGroup,
        DirectoryService.Scope.AdminDirectoryGroupMember,
        DirectoryService.Scope.AdminDirectoryOrgunit
       
    };
           options.Scope.AddRange(scopes);

   options.SaveTokens = true;
   options.AccessType = "offline";       
   });
builder.Services.AddHttpClient();
builder.Services.AddScoped<Helper>();
builder.Services.AddSingleton<CookieProtector>();
builder.Services.AddSingleton<TokenProtector>();
var app = builder.Build();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
