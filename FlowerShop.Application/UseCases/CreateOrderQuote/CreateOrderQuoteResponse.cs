using FlowerShop.Application.UseCases.Common;

namespace FlowerShop.Application.UseCases.CreateOrderQuote;

public class CreateOrderQuoteResponse
{
    public IEnumerable<QuoteRowDto> QuoteRows { get; }

    public CreateOrderQuoteResponse(IEnumerable<QuoteRowDto> quoteRows)
    {
        QuoteRows = quoteRows;
    }
}