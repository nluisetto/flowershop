using FlowerShop.Application.UseCases.Common;

namespace FlowerShop.Cli.Infrastructure;

public class OrderRowParsingResult
{
    public string OriginalData { get; }
    public bool IsSuccessful => ParsedRowDto != null;
    public OrderRowDto? ParsedRowDto { get; }
    public string? FailureCause { get; }

    private OrderRowParsingResult(string originalData, OrderRowDto orderRowDto)
    {
        OriginalData = originalData;
        ParsedRowDto = orderRowDto;
    }

    private OrderRowParsingResult(string originalData, string failureCause)
    {
        OriginalData = originalData;
        FailureCause = failureCause;
    }

    public static OrderRowParsingResult Successful(string originalData, OrderRowDto orderRowDto)
    {
        return new OrderRowParsingResult(originalData, orderRowDto);
    }

    public static OrderRowParsingResult InvalidRowFormat(string originalData, string failureCause)
    {
        return new OrderRowParsingResult(originalData, failureCause);
    }
}