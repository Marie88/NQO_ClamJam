using ClamJam.Api.Configuration;
using ClamJam.Library;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add ClamJam services
builder.Services.AddClamJamServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClamJam");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();


var shop = new ShopManager();

app.MapPost("/add", (string name, double price) => shop.Add(name, price));
app.MapPost("/checkout", (string? coupon) => shop.Checkout(coupon ?? ""));
app.MapGet("/export", () => shop.Export());

app.Run();
