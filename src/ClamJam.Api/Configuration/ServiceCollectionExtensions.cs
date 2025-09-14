
using ClamJam.Domain.Entities;
using CleanJam.Application.Cart;
using CleanJam.Application.Discount;
using CleanJam.Application.Export;
using CleanJam.Application.Pricing;
using CleanJam.Application.Product;
using CleanJam.Application.Tax;
using CleanJam.Infrastructure;
using CleanJam.Infrastructure.Export;
using CleanJam.Infrastructure.Repository;

namespace ClamJam.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClamJamServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.AddSingleton(provider => new Dictionary<EnumProductType, ITaxStrategy>()
        {
            [EnumProductType.Food] = new StandardVatStrategy(5),
            [EnumProductType.NonFood] = new StandardVatStrategy(19)
        });
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddSingleton<ICartRepository, InMemoryCartRepository>();

        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<ITaxService, TaxService>();

        services.AddScoped<IExportService, JsonExportService>();
        services.AddScoped<IExportService, CSVExportService>();
        services.AddScoped<IExportService, TextExportService>();

        services.AddScoped<IExportServiceFactory, ExportServiceFactory>();

        return services;
    }
}