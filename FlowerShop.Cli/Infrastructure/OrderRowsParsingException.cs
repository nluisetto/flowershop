namespace FlowerShop.Cli.Infrastructure;

public class OrderRowsParsingException : Exception
{
    public IEnumerable<OrderRowParsingResult> Failures { get; }

    public OrderRowsParsingException(List<OrderRowParsingResult> failures)
    : base("It was not possible to correctly parse all order rows.")
    {
        Failures = failures;
    }
}