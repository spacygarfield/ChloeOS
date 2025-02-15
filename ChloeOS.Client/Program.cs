using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.DataAccess.Contexts;
using ChloeOS.DataAccess.Repositories.FS;
using ChloeOS.DataAccess.Repositories.OS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Services

// MVC.
builder.Services.AddControllersWithViews();
builder.Services.AddAntiforgery(options => {
    options.HeaderName = "X-CSRF";
    options.SuppressXFrameOptionsHeader = true;

    // Cookie setup.
    options.Cookie.Name = "ChloeOS.Antiforgery";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Session.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // Cookie setup.
    options.Cookie.Name = "ChloeOS.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Authentication and authorization.
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => {
    options.LoginPath = "/signon";
    options.LogoutPath = "/signon/signout";
    options.AccessDeniedPath = "/signon/denied";
    options.ReturnUrlParameter = "referrer";

    // Cookie setup.
    options.Cookie.Name = "ChloeOS.Authentication";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddAuthorization(options => {
    // Ensures the user can access certain controllers and views while being 100% signed out.
    // Check the HTTP context's signed-in user.
    options.AddPolicy("Signed-Out", policy => {
        policy.RequireAssertion(context => !context.User.Identity!.IsAuthenticated);
    });
});

// Database connection.
Action<DbContextOptionsBuilder> contextCallback = options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("OSDB")!);
builder.Services.AddDbContext<FileSystemContext>(contextCallback);
builder.Services.AddDbContext<OperatingSystemContext>(contextCallback);

// Database access repositories.
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

WebApplication app = builder.Build();

#region Middlewares

// Environment setup.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Desktop/Error");
    app.UseHsts();
}

// HTTP handlers.
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// User handlers.
app.UseSession();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

// URI maps.
app.MapControllerRoute("default", "{controller=Desktop}/{action=Index}/{id?}");

#endregion

await app.RunAsync();