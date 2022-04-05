using FlowerShop.Application.UseCases.Common;

namespace FlowerShop.Cli.Infrastructure;

public interface IOrderRowsParserFromCliArgument
{
    public IEnumerable<OrderRowDto> Parse(string[] cliArguments);
}