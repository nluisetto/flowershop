namespace FlowerShop.Domain;

public partial class OrderQuote
{
    private readonly Dictionary<Order.Row, OrderQuote.Row> _rows = new ();
    
    public HashSet<Row> Rows => _rows.Values.ToHashSet();
    
    public void AddBundle(Order.Row orderRow, int count, Bundle bundle)
    {
        if (orderRow == null) throw new ArgumentNullException(nameof(orderRow));
        if (count < 0) throw new InvalidBundleCountException();

        var row = GetQuoteRowFor(orderRow);
        row.AddBundle(count, bundle);
    }
    
    public int MissingQuantityFor(Order.Row orderRow)
    {
        return GetQuoteRowFor(orderRow).MissingQuantity;
    }
    
    public bool IsFilled(Order.Row orderRow)
    {
        return GetQuoteRowFor(orderRow).IsFilled;
    }

    private Row GetQuoteRowFor(Order.Row orderRow)
    {
        if (!_rows.ContainsKey(orderRow))
            _rows.Add(orderRow, new Row(orderRow));

        return _rows[orderRow];
    }
}