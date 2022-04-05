using FlowerShop.Application.UseCases.CreateOrderQuote;
using FlowerShop.Cli.Infrastructure;
using FlowerShop.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace FlowerShop.Cli.Commands.CreateOrderQuote;

public class CreateOrderQuoteCommand : AsyncCommand<CreateOrderQuoteCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<order-rows>")]
        public string[] OrderRows { get; set; }
    }

    private readonly ILogger<CreateOrderQuoteCommand> _logger;
    private readonly IOrderRowsParserFromCliArgument _orderRowsParserFromCliArgument;
    private readonly IMediator _mediator;
    private readonly IOrderQuoteCliWriter _orderQuoteCliWriter;
    private readonly ICliWriter _cliWriter;
    
    public CreateOrderQuoteCommand(ILogger<CreateOrderQuoteCommand> logger, IOrderRowsParserFromCliArgument orderRowsParserFromCliArgument, IMediator mediator, IOrderQuoteCliWriter orderQuoteCliWriter, ICliWriter cliWriter)
    {
        _logger = logger;
        _orderRowsParserFromCliArgument = orderRowsParserFromCliArgument;
        _mediator = mediator;
        _orderQuoteCliWriter = orderQuoteCliWriter;
        _cliWriter = cliWriter;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            _logger.LogInformation("Beginning quote creation for {@OrderRows}", settings.OrderRows);

            var orderRowsAsCliArgument = settings.OrderRows;
            var orderRows = _orderRowsParserFromCliArgument.Parse(orderRowsAsCliArgument);

            var estimateOrderRequest = new CreateOrderQuoteRequest {OrderRows = orderRows};
            var response = await _mediator.Send(estimateOrderRequest);

            _orderQuoteCliWriter.Write(response.QuoteRows);

            _logger.LogInformation("Completed quote creation for {@OrderRows} with {@QuoteRows}", settings.OrderRows,
                response.QuoteRows);

            return 0;
        }
        catch (NoFillingBundleConfigurationExistsException ex)
        {
            _logger.LogError(ex, "No bundle configuration among {@AvailableBundles} could fill {@OrderRow}", ex.AvailableBundles, ex.OrderRow);
            
            _cliWriter.WriteErrorLine($"No bundle configuration among available bundles could fill the order row for {ex.OrderRow.Quantity} {ex.OrderRow.ProductCode}");

            return 3;
        }
        catch (OrderRowsParsingException ex)
        {
            _logger.LogError(ex, "Quote creation could not complete because some {@OrderRows} could not be parsed",
                settings.OrderRows);

            _cliWriter.WriteErrorLine($"{ex.Message}");
            foreach (var failure in ex.Failures)
            {
                _cliWriter.WriteLine($"'{failure.OriginalData}': {failure.FailureCause}");
            }

            return 2;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Quote creation could not complete because of an unexpected error");
            
            _cliWriter.WriteException(ex);
            
            return 1;
        }
    }
}