namespace FlowerShop.Domain;

public partial class OrderQuote
{
    public partial class Row
    {
        public class BundleDetail
        {
            public int Count { get; private set; }
            public Bundle Bundle { get; }
            public int TotalQuantity => Count * Bundle.Quantity;
            public decimal TotalPrice => Count * Bundle.Price;

            public BundleDetail(Bundle bundle)
            {
                Count = 0;
                Bundle = bundle;
            }

            public void Add(int count)
            {
                Count += count;
            }
        }
    }
}