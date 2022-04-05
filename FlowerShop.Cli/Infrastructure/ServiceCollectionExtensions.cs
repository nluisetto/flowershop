using FlowerShop.Application.UseCases.CreateOrderQuote;
using FlowerShop.Domain;
using FlowerShop.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FlowerShop.Cli.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleAppInfrastructure(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddLogging((_) => _.AddSerilog())
            
            .AddSingleton<IOrderRowsParserFromCliArgument, OrderRowsParserFromCliArgument>()
            .AddSingleton<ICliWriter, AnsiConsoleWriter>()
            .AddSingleton<IOrderQuoteCliWriter, OrderQuoteAnsiConsoleWriter>()
            
            .AddFlowerShopApplicationInfrastructure(
                new HashSet<Bundle>
                {
                    new ("R12",  5,  6.99m),
                    new ("R12", 10, 12.99m),
                    
                    new ("L09", 3,  9.95m),
                    new ("L09", 6, 16.95m),
                    new ("L09", 9, 24.95m),
                    
                    new ("T58", 3,  5.95m),
                    new ("T58", 5,  9.95m),
                    new ("T58", 9, 16.99m)
                },
                typeof(CreateOrderQuoteHandler)
            );
    }
}