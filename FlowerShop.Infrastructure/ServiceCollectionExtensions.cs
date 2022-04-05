using FlowerShop.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FlowerShop.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlowerShopApplicationInfrastructure(this IServiceCollection serviceCollection, ISet<Bundle> bundles, params Type[] requestHandlers)
    {
        return serviceCollection
            .AddMediatR(requestHandlers)
            .AddScoped<IBundleRepository, InMemoryBundleRepository>((_) => new InMemoryBundleRepository(bundles))
            .AddScoped<IOrderRowFiller, GoogleOrToolsOrderRowFiller>();
    }
}