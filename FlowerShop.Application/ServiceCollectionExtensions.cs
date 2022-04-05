using FlowerShop.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FlowerShop.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlowerShopApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddFlowerShopDomain();
    }
}