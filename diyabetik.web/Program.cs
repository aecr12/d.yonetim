using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Cookie tabanlı basit authorization işlemi, ihtiyaca göre identity kullanılabilir
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login/Login";
            options.LogoutPath = "/Login/Logout";
        });
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
// Session config
builder.Services.AddSession(options=>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
var app = builder.Build();
app.UseSession();
app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
   FileProvider = new PhysicalFileProvider(
      Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
      RequestPath="/modules"
});
app.UseEndpoints(endpoints =>
{
   endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
   );
});
app.Run();
