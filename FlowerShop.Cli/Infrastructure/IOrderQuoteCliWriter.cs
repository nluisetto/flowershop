using FlowerShop.Application.UseCases.Common;

namespace FlowerShop.Cli.Infrastructure;

public interface IOrderQuoteCliWriter
{
    public void Write(IEnumerable<QuoteRowDto> quoteRows);
}