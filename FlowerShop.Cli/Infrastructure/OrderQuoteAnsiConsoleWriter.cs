using FlowerShop.Application.UseCases.Common;
using Spectre.Console;

namespace FlowerShop.Cli.Infrastructure;

public class OrderQuoteAnsiConsoleWriter : IOrderQuoteCliWriter
{
    public void Write(IEnumerable<QuoteRowDto> quoteRows)
    {
        foreach (var quoteRow in quoteRows)
        {
            Console.WriteLine($"{quoteRow.OrderRowDto.Quantity} {quoteRow.OrderRowDto.ProductCode} {quoteRow.TotalPrice:c}");

            foreach (var bundleDetail in quoteRow.BundleDetails.OrderByDescending((bundleDetail) => bundleDetail.Bundle.Quantity))
            {
                Console.WriteLine($"    {bundleDetail.Count} x {bundleDetail.Bundle.Quantity} {bundleDetail.Bundle.Price:c}");
            }
        }
    }
}