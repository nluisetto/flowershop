using FlowerShop.Application;
using FlowerShop.Cli.Commands.CreateOrderQuote;
using FlowerShop.Cli.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using FlowerShop.Cli.Infrastructure.DependencyInjection;
using Serilog;

try
{
    SetupLogger();

    var app = BuildConsoleApp();

    return await app.RunAsync(args);
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Unexpected error occurred");

    return 1;
}
finally
{
    Log.CloseAndFlush();
}



void SetupLogger()
{
    var logFilePath = Path.Combine(Path.GetTempPath(), "flowershop-cli.log");
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File(
            logFilePath,
            rollingInterval: RollingInterval.Day
        )
        .CreateLogger();
}

IServiceCollection GetServiceCollection()
{
    var services = new ServiceCollection();

    services
        .AddFlowerShopApplication()
        .AddConsoleAppInfrastructure();

    return services;
}

ICommandApp BuildConsoleApp()
{
    var serviceCollection = GetServiceCollection();
    var registrar = new TypeRegistrar(serviceCollection);
    var app = new CommandApp(registrar);

    app.Configure((consoleApp) =>
    {
        consoleApp.AddCommand<CreateOrderQuoteCommand>("create-quote");
    });

    return app;
}