using FlowerShop.Application.UseCases.Common;

namespace FlowerShop.Cli.Infrastructure;

public class OrderRowsParserFromCliArgument : IOrderRowsParserFromCliArgument
{
    private static readonly string RowTokenSeparator = " ";

    public IEnumerable<OrderRowDto> Parse(string[] cliArguments)
    {
        var parseResults = cliArguments
            .Select(ParseRow)
            .ToList();

        var failures = parseResults.Where(result => !result.IsSuccessful).ToList();
        if (failures.Any()) throw new OrderRowsParsingException(failures);

        return parseResults
            .Select(result => result.ParsedRowDto!)
            .ToList();
    }

    private OrderRowParsingResult ParseRow(string rowAsString)
    {
        var rowTokens = rowAsString.Split(RowTokenSeparator);

        if (rowTokens.Length != 2)
            return OrderRowParsingResult.InvalidRowFormat(rowAsString, $"Row do not respect the expected format");

        if (!int.TryParse(rowTokens[0], out var quantity))
            return OrderRowParsingResult.InvalidRowFormat(rowAsString, "Quantity must be numeric");
                
        var productCode = rowTokens[1];
        if (string.IsNullOrWhiteSpace(productCode))
            return OrderRowParsingResult.InvalidRowFormat(rowAsString, "Product code can't be empty");

        return OrderRowParsingResult.Successful(rowAsString, new OrderRowDto(quantity, productCode));
    }
}