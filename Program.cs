using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DateBaseOrderContext>(opt => opt.UseInMemoryDatabase(nameof(DateBaseOrderContext)));
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
