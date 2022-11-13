using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC;
using OrderProcessingMVC.Context;
using OrderProcessingMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase("OrderContext"));
builder.Services.AddScoped(typeof(OrderContext));
var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");

app.Run();
