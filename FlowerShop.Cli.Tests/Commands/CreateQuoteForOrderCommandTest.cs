using System.Threading;
using System.Threading.Tasks;
using FlowerShop.Application.UseCases.Common;
using FlowerShop.Application.UseCases.CreateOrderQuote;
using FlowerShop.Cli.Commands.CreateOrderQuote;
using FlowerShop.Cli.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Spectre.Console.Cli;
using Xunit;

namespace FlowerShop.Cli.Tests.Commands;

public class CreateQuoteForOrderCommandTest
{
    [Fact]
    public void Should_Output_Parsing_Errors_Details_And_Avoid_Further_Processing_If_Any_Parsing_Error_Occurs()
    {
        var givenOrderRows = new[] { "CLEARLY AN INVALID ORDER ROW" };
        var parsingErrorException = new OrderRowsParsingException(new () { OrderRowParsingResult.InvalidRowFormat(givenOrderRows[0], "IT'S A TEST") });
        
        var logger = Mock.Of<ILogger<CreateOrderQuoteCommand>>();
        var orderRowsParser = Mock.Of<IOrderRowsParserFromCliArgument>();
        // orderRowsParser Parse method will throw an OrderRowsParsingException
        Mock.Get(orderRowsParser).Setup(orderRowsParserMock => orderRowsParserMock.Parse(givenOrderRows)).Throws(parsingErrorException);
        var mediator = Mock.Of<IMediator>();
        var orderQuoteCliWriter = Mock.Of<IOrderQuoteCliWriter>();
        var genericCliWriter = Mock.Of<ICliWriter>();

        var createQuoteForOrderCommand = new CreateOrderQuoteCommand(
            logger,
            orderRowsParser,
            mediator,
            orderQuoteCliWriter,
            genericCliWriter
        );

        var commandContext = GetFakeCommandContext();
        var settings = new CreateOrderQuoteCommand.Settings { OrderRows = givenOrderRows };
        createQuoteForOrderCommand.ExecuteAsync(commandContext, settings);
        
        // Verifying that parsing error is written to the CLI
        Mock.Get(genericCliWriter).Verify(genericCliWriterMock => genericCliWriterMock.WriteErrorLine(It.IsAny<string>()), Times.Once);
        Mock.Get(genericCliWriter).Verify(genericCliWriterMock => genericCliWriterMock.WriteLine(It.IsAny<string>()), Times.Once);
        
        // Verifying that the request for quote creation has not been sent through the mediator
        Mock.Get(mediator).Verify(mediatorMock => mediatorMock.Send(It.IsAny<CreateOrderQuoteRequest>(), default(CancellationToken)), Times.Never);
    }
    
    [Fact]
    public void Should_Output_Quote_Rows_When_Processing_Complete_Correctly()
    {
        var givenOrderRows = new[] { "1 R12" };
        var parsedOrderRows = new[] {new OrderRowDto(1, "R12")};
        var responseFromApplicationHandler = new CreateOrderQuoteResponse(
            new [] { new QuoteRowDto(
                new OrderRowDto(1, "R12"),
                new [] { new QuoteRowDto.BundleDetailDto(1, new BundleDto(1, 1), 1) },
                1
            )}
        );
        
        var logger = Mock.Of<ILogger<CreateOrderQuoteCommand>>();
        // orderRowParser Parse method will return parsedOrderRows
        var orderRowsParser = Mock.Of<IOrderRowsParserFromCliArgument>((orderRowsParserMock) => orderRowsParserMock.Parse(givenOrderRows) == parsedOrderRows);
        var mediator = Mock.Of<IMediator>((mediatorMock) => mediatorMock.Send(It.IsAny<CreateOrderQuoteRequest>(), default(CancellationToken)) == Task.FromResult(responseFromApplicationHandler));
        var orderQuoteCliWriter = Mock.Of<IOrderQuoteCliWriter>();
        var genericCliWriter = Mock.Of<ICliWriter>();

        var createQuoteForOrderCommand = new CreateOrderQuoteCommand(
            logger,
            orderRowsParser,
            mediator,
            orderQuoteCliWriter,
            genericCliWriter
        );

        var commandContext = GetFakeCommandContext();
        var settings = new CreateOrderQuoteCommand.Settings { OrderRows = givenOrderRows };
        createQuoteForOrderCommand.ExecuteAsync(commandContext, settings);

        // Verifying that the order quote has been written to cli through orderQuoteCliWriter
        Mock.Get(orderQuoteCliWriter).Verify(orderQuoteCliWriterMock => orderQuoteCliWriterMock.Write(responseFromApplicationHandler.QuoteRows), Times.Once);
    }

    private CommandContext GetFakeCommandContext()
    {
        return new CommandContext(Mock.Of<IRemainingArguments>(), "A COMMAND NAME", null);
    }
}