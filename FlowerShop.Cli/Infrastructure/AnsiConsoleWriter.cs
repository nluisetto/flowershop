using Spectre.Console;

namespace FlowerShop.Cli.Infrastructure;

public class AnsiConsoleWriter : ICliWriter
{
    public void WriteErrorLine(string value)
    {
        AnsiConsole.MarkupLine($"[red]{value}[/]");
    }

    public void WriteLine(string value)
    {
        AnsiConsole.WriteLine(value);
    }

    public void WriteException(Exception exception)
    {
        AnsiConsole.WriteException(exception);
    }
}