using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Repositories;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var a = String.IsNullOrEmpty(builder.Configuration.GetSection("SqlConnectionString").ToString());
if (!String.IsNullOrEmpty(builder.Configuration.GetSection("SqlConnectionString").ToString()))
{
    builder.Services.AddDbContext<DateBaseOrderContext>(opt => opt.UseInMemoryDatabase(nameof(DateBaseOrderContext)));
}
else
{
    builder.Services.AddDbContext<DateBaseOrderContext>(opt => opt.UseSqlite(builder.Configuration.GetSection("sqlConnectionString").ToString()));
}
builder.Services.AddScoped<OrdersRepository>();
builder.Services.AddScoped<OrderItemRepository>();
builder.Services.AddScoped<ProvidersRepository>();
var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");

app.Run();
