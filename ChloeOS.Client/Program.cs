using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.DataAccess.Contexts;
using ChloeOS.DataAccess.Repositories.OS;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllersWithViews();

// Database connection.
string connectionString = builder.Configuration.GetConnectionString("OSDB")!;
builder.Services.AddDbContext<FileSystemContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<OperatingSystemContext>(options => options.UseSqlServer(connectionString));

// Database access repositories.
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

WebApplication app = builder.Build();

#region Middlewares

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Desktop/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Desktop}/{action=Index}/{id?}");

#endregion

await app.RunAsync();