using ClamJam.Library;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var shop = new ShopManager();

app.MapPost("/add", (string name, double price) => shop.Add(name, price));
app.MapPost("/checkout", (string? coupon) => shop.Checkout(coupon ?? ""));
app.MapGet("/export", () => shop.Export());

app.Run();
