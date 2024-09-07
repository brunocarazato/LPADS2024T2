using LPADS2024T2.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ConnectionContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("ConnectionContext")
        )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/*
 * Adicionar nova rota
app.MapControllerRoute(
    name: "ads",
    pattern: "FemaCursos/{action=ADS}",
    defaults: new { controller = "Fema" });
*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
