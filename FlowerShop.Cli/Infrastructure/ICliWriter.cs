namespace FlowerShop.Cli.Infrastructure;

public interface ICliWriter
{
    public void WriteErrorLine(string value);
    public void WriteLine(string value);
    public void WriteException(Exception exception);
}