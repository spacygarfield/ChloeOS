using ChloeOS.Client.Database;
using ChloeOS.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllersWithViews();

// Database connection.
string connectionString = builder.Configuration.GetConnectionString("OSDB")!;
builder.Services.AddDbContext<FileSystemContext>(options => options.UseMySQL(connectionString));
builder.Services.AddDbContext<OperatingSystemContext>(options => options.UseMySQL(connectionString));

#endregion

WebApplication app = builder.Build();

#region Middlewares

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

await app.RunAsync();