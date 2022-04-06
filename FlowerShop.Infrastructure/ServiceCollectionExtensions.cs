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
            
            // Here you can choose which implementation of the optimization algorithm will be used by the application
            .AddScoped<IOrderRowFiller, GoogleOrToolsOrderRowFiller>();
            // .AddScoped<IOrderRowFiller, CartesianProductOrderRowFiller>();
    }
}