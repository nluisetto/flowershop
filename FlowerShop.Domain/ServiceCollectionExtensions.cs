using Microsoft.Extensions.DependencyInjection;

namespace FlowerShop.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlowerShopDomain(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IOrderQuoteService, OrderQuoteService>();
    }
}