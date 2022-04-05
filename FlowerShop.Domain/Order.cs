namespace FlowerShop.Domain;

public partial class Order
{
    private readonly HashSet<Row> _rows = new ();
    
    public IReadOnlySet<Row> Rows => _rows;

    public void AddRow(int quantity, string productCode)
    {
        if (quantity < 0)
            throw new InvalidBundleCountException();

        var newRow = new Row(quantity, productCode);
        _rows.Add(newRow);
    }
}