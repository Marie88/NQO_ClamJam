
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
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();

        return services;
    }
}